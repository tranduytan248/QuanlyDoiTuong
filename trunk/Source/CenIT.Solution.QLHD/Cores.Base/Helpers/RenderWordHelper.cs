using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Spire.Doc;
using ZXing;
using ZXing.Common;
using Document = Spire.Doc.Document;
using Paragraph = Spire.Doc.Documents.Paragraph;

namespace Cores.Base.Helpers
{
    public static class RenderWordHelper
    {
        public static byte[] RenderModelToWordAndSave<T>(T model, string templatePath) where T : class
        {
            byte[] fileBytes;

            // Load template document
            var doc = new Document();
            doc.LoadFromFile(templatePath);

            // Create a dictionary of property placeholders
            var modelProperties = typeof(T).GetProperties().ToDictionary(p => "{" + p.Name + "}");

            // Handle QRCode replacement
            HandleQRCodeReplacement(doc, model);

            foreach (var prop in modelProperties)
            {
                var placeholder = prop.Key;
                var value = prop.Value.GetValue(model);

                // Replace placeholder with value in the document
                doc.Replace(placeholder, value?.ToString(), true, true);
            }

            // Save the modified document to a memory stream
            using (var ms = new MemoryStream())
            {
                doc.SaveToStream(ms, FileFormat.Docx);
                fileBytes = ms.ToArray();
            }

            if (doc.PageCount > 1)
            {
                var fontSize = 27;

                using (var ms = new MemoryStream(fileBytes))
                {
                    while (true)
                    {
                        var spireDoc = new Document(ms);
                        if (spireDoc.PageCount > 1)
                        {
                            ShrinkWordFile(ms, fontSize);
                            fontSize -= 1;
                        }
                        else
                        {
                            fileBytes = ms.ToArray();
                            break;
                        }
                    }
                }
            }

            return fileBytes;
        }

        private static void ShrinkWordFile(MemoryStream ms, int fontSize = 27)
        {
            //using (MemoryStream ms = new MemoryStream(arrFileBytes))
            //using (ms)
            {
                using (var docProc = WordprocessingDocument.Open(ms, true))
                {
                    // Lấy body của tài liệu
                    var body = docProc.MainDocumentPart.Document.Body;

                    // Vòng lặp giảm kích thước font đến khi vừa một trang
                    foreach (var paragraph in body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>())
                    foreach (var run in paragraph.Elements<Run>())
                    {
                        var runProps = run.RunProperties ?? new RunProperties();

                        // Thiết lập kích thước font
                        var size = runProps.GetFirstChild<FontSize>() ?? new FontSize();
                        size.Val = fontSize.ToString();
                        runProps.FontSize = size;
                        run.RunProperties = runProps;
                    }

                    docProc.MainDocumentPart.Document.Save();
                    //return ms.ToArray();
                }
            }
        }

        public static byte[] ConvertWordToPdf(byte[] wordBytes)
        {
            using (var wordStream = new MemoryStream(wordBytes))
            {
                // Load Word document using Spire.Doc
                var doc = new Document();
                doc.LoadFromStream(wordStream, FileFormat.Docx);

                // Create a MemoryStream to save the PDF
                using (var pdfStream = new MemoryStream())
                {
                    // Save the document as PDF
                    doc.SaveToStream(pdfStream, FileFormat.PDF);

                    // Read PDF file bytes
                    var pdfBytes = pdfStream.ToArray();

                    return pdfBytes;
                }
            }
        }

        private static void HandleQRCodeReplacement<T>(Document doc, T model) where T : class
        {
            var qrCodeProperty = typeof(T).GetProperty("QRCode");
            if (qrCodeProperty != null)
            {
                var qrCode = qrCodeProperty.GetValue(model)?.ToString();

                if (!string.IsNullOrEmpty(qrCode))
                {
                    // Generate QRCode image bytes
                    var qrCodeBytes = GenerateQRCodeBytes(qrCode);

                    // Replace the placeholder with QRCode image
                    var textToReplace = "${QRCode}";

                    foreach (Section sec in doc.Sections)
                    foreach (Paragraph paragraph in sec.Paragraphs)
                        if (paragraph.Text.Contains(textToReplace))
                        {
                            // Remove the placeholder text
                            paragraph.Text = paragraph.Text.Replace(textToReplace, string.Empty);

                            // Add QRCode image
                            using (var qrCodeStream = new MemoryStream(qrCodeBytes))
                            {
                                var docPicture = paragraph.AppendPicture(qrCodeStream);
                                docPicture.Width = 60; // Adjust width if necessary
                                docPicture.Height = 60; // Adjust height if necessary
                            }
                        }
                }
                else
                {
                    // Replace the placeholder with an empty string
                    var textToReplace = "${QRCode}";

                    foreach (Section sec in doc.Sections)
                    foreach (Paragraph paragraph in sec.Paragraphs)
                        if (paragraph.Text.Contains(textToReplace))
                            paragraph.Replace(textToReplace, "", false, true);
                }
            }
        }

        private static byte[] GenerateQRCodeBytes(string qrCodeText)
        {
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Width = 100,
                    Height = 100,
                    Margin = 0
                }
            };

            // Generate QRCode image
            var qrcodeBitmap = barcodeWriter.Write(qrCodeText);

            // Convert the QRCode image to byte array
            using (var stream = new MemoryStream())
            {
                qrcodeBitmap.Save(stream, ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }

        public static byte[] RenderModelToPdfAndSave<T>(T model, string templatePath) where T : class
        {
            var wordBytes = RenderModelToWordAndSave(model, templatePath);
            return ConvertWordToPdf(wordBytes);
        }
    }
}