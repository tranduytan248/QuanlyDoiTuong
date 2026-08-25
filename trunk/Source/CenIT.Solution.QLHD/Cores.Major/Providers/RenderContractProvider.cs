using System.Drawing.Imaging;
using System.IO;
using System.Web.Hosting;
using Newtonsoft.Json.Linq;
using Spire.Doc;
using Spire.Doc.Fields;
using ZXing;
using ZXing.Common;
using Document = Spire.Doc.Document;
using Paragraph = Spire.Doc.Documents.Paragraph;
using Section = Spire.Doc.Section;
using Table = Spire.Doc.Table;

namespace Cores.Major.Providers
{
    public class RenderContractProvider
    {
        public static byte[] RenderContract(string relativeTemplateFilePath, string jsonData, int indexTabel,
            int indexRowInTable, bool includeQRCode, string qrCode, string fileType = ".docx")
        {
            // Chuyển đổi đường dẫn tương đối thành đường dẫn tuyệt đối
            var templateFilePath = HostingEnvironment.MapPath(relativeTemplateFilePath);

            // Load template document
            var doc = new Document();
            doc.LoadFromFile(templateFilePath);

            // Parse JSON data
            var contractData = JObject.Parse(jsonData);

            // Replace placeholders and other values in the document with data from JSON
            ReplaceValues(doc, contractData, indexTabel, indexRowInTable, includeQRCode, qrCode);

            // Save the modified document to a MemoryStream
            var stream = new MemoryStream();
            doc.SaveToStream(stream, FileFormat.Docx);

            // Convert MemoryStream to byte array
            var bytes = stream.ToArray();

            if (fileType == ".pdf")
            {
                //return ConvertToPdfUseAspose(bytes);
            }

            return bytes;
        }

        public static byte[] RenderContract(string relativeTemplateFilePath, string jsonData, int indexTabel,
            int indexRowInTable, string qrCode)
        {
            // Chuyển đổi đường dẫn tương đối thành đường dẫn tuyệt đối
            var templateFilePath = HostingEnvironment.MapPath(relativeTemplateFilePath);

            // Load template document
            var doc = new Document();
            doc.LoadFromFile(templateFilePath);

            // Parse JSON data
            var contractData = JObject.Parse(jsonData);

            // Replace placeholders and other values in the document with data from JSON
            ReplaceValues(doc, contractData, indexTabel, indexRowInTable, qrCode);

            // Save the modified document to a MemoryStream
            var stream = new MemoryStream();
            doc.SaveToStream(stream, FileFormat.Docx);

            // Convert MemoryStream to byte array
            var bytes = stream.ToArray();

            return bytes;
        }

        private static void ReplaceValues(Document doc, JObject contractData, int indexTabel, int indexRowInTable,
            bool includeQRCode, string qrCode)
        {
            foreach (Section section in doc.Sections)
            foreach (DocumentObject obj in section.Body.ChildObjects)
                if (obj is Table table)
                {
                    foreach (TableRow row in table.Rows)
                    foreach (TableCell cell in row.Cells)
                    foreach (Paragraph para in cell.Paragraphs)
                        ReplaceValuesInParagraph(para, contractData);
                }
                else // Xử lý cho Paragraph
                {
                    var para = obj as Paragraph;
                    ReplaceValuesInParagraph(para, contractData);
                }

            #region Render data in row of table

            var tableData = contractData["#gt"] as JArray;
            var itable = 0;
            var stt = 0;
            if (tableData != null)
            {
                var rowNeedClone = tableData.Count - 1;
                var sectionWord = doc.Sections[0];
                foreach (DocumentObject obj in sectionWord.Body.ChildObjects)
                    if (obj is Table table) //If obj is Table
                    {
                        itable++;
                        if (itable == indexTabel) //bang can thay the gia tri
                        {
                            //Clone so dong can hien thi
                            for (var i = table.Rows.Count - 1; i >= 0; i--)
                                if (i == indexRowInTable)
                                {
                                    stt++;
                                    var row = table.Rows[i];

                                    for (var r = 0; r < rowNeedClone; r++)
                                    {
                                        //Clone hang
                                        var cloneRow = row.Clone();
                                        table.Rows.Insert(i + 1 + r, cloneRow);
                                    }
                                }

                            stt = 0;

                            for (var ir = 0; ir < table.Rows.Count; ir++)
                                if (ir >= indexRowInTable && ir <= indexRowInTable + rowNeedClone)
                                {
                                    var row = table.Rows[ir];
                                    //Lap qua tat ca cac cell cua row
                                    foreach (TableCell cell in row.Cells)
                                    foreach (Paragraph para in cell.Paragraphs)
                                        if (tableData[stt] is JObject rowData)
                                            foreach (var token in rowData.Children())
                                            {
                                                var property = token as JProperty;
                                                var placeholder = $"{property?.Name}";
                                                var replacement = property?.Value.ToString();

                                                // Thực hiện thay thế dữ liệu trong văn bản của Paragraph
                                                para.Replace(placeholder, replacement, false, true);
                                            }

                                    stt++;
                                }
                        }
                    }
            }

            #endregion

            if (includeQRCode)
            {
                #region generate QRCode

                if (!string.IsNullOrEmpty(qrCode))
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

                    //Tao qrcode image
                    var qrcodeBitmap = barcodeWriter.Write(qrCode);

                    var docPicture = new DocPicture(doc);
                    docPicture.LoadImage(qrcodeBitmap);

                    //chuyen hinh anh thanh byte
                    byte[] qrCodeBytes;
                    using (var stream = new MemoryStream())
                    {
                        qrcodeBitmap.Save(stream, ImageFormat.Jpeg);
                        qrCodeBytes = stream.ToArray();
                    }

                    //doan chuoi se duoc replace
                    var textToReplace = "${QRCode}";

                    foreach (Section sec in doc.Sections)
                    foreach (Paragraph paragraph in sec.Paragraphs)
                        if (paragraph.Text.Contains(textToReplace))
                        {
                            paragraph.Text = "";
                            paragraph.AppendPicture(qrCodeBytes);
                        }
                }

                #endregion
            }
            else
            {
                //doan chuoi se duoc replace
                var textToReplace = "${QRCode}";

                foreach (Section sec in doc.Sections)
                foreach (Paragraph paragraph in sec.Paragraphs)
                    if (paragraph.Text.Contains(textToReplace))
                        paragraph.Replace(textToReplace, "", false, true);
            }
        }

        private static void ReplaceValues(Document doc, JObject contractData, int indexTabel, int indexRowInTable,
            string qrCode)
        {
            foreach (Section section in doc.Sections)
            foreach (DocumentObject obj in section.Body.ChildObjects)
                if (obj is Table table)
                {
                    foreach (TableRow row in table.Rows)
                    foreach (TableCell cell in row.Cells)
                    foreach (Paragraph para in cell.Paragraphs)
                        ReplaceValuesInParagraph(para, contractData);
                }
                else // Xử lý cho Paragraph
                {
                    var para = obj as Paragraph;
                    ReplaceValuesInParagraph(para, contractData);
                }

            #region render data in row of table

            var tableData = contractData["#gt"] as JArray;
            var itable = 0;
            var stt = 0;
            if (tableData != null)
            {
                var rowNeedClone = tableData.Count - 1;
                var sectionWord = doc.Sections[0];
                foreach (DocumentObject obj in sectionWord.Body.ChildObjects)
                    if (obj is Table table) //If obj is Table
                    {
                        itable++;
                        if (itable == indexTabel) //bang can thay the gia tri
                        {
                            //Clone so dong can hien thi
                            for (var i = table.Rows.Count - 1; i >= 0; i--)
                                if (i == indexRowInTable)
                                {
                                    stt++;
                                    var row = table.Rows[i];

                                    for (var r = 0; r < rowNeedClone; r++)
                                    {
                                        //Clone hang
                                        var cloneRow = row.Clone();
                                        table.Rows.Insert(i + 1 + r, cloneRow);
                                    }
                                }

                            stt = 0;

                            for (var ir = 0; ir < table.Rows.Count; ir++)
                                if (ir >= indexRowInTable && ir <= indexRowInTable + rowNeedClone)
                                {
                                    var row = table.Rows[ir];
                                    //Lap qua tat ca cac cell cua row
                                    foreach (TableCell cell in row.Cells)
                                    foreach (Paragraph para in cell.Paragraphs)
                                        if (tableData[stt] is JObject rowData)
                                            foreach (var token in rowData.Children())
                                            {
                                                var property = token as JProperty;
                                                var placeholder = $"{property?.Name}";
                                                var replacement = property?.Value.ToString();

                                                // Thực hiện thay thế dữ liệu trong văn bản của Paragraph
                                                para.Replace(placeholder, replacement, false, true);
                                            }

                                    stt++;
                                }
                        }
                    }
            }

            #endregion

            #region generate QRCode

            if (!string.IsNullOrEmpty(qrCode))
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

                //Tao qrcode image
                var qrcodeBitmap = barcodeWriter.Write(qrCode);

                var docPicture = new DocPicture(doc);
                docPicture.LoadImage(qrcodeBitmap);

                //chuyen hinh anh thanh byte
                byte[] qrCodeBytes;
                using (var stream = new MemoryStream())
                {
                    qrcodeBitmap.Save(stream, ImageFormat.Jpeg);
                    qrCodeBytes = stream.ToArray();
                }

                //doan chuoi se duoc replace
                var textToReplace = "${QRCode}";

                foreach (Section sec in doc.Sections)
                foreach (Paragraph paragraph in sec.Paragraphs)
                    if (paragraph.Text.Contains(textToReplace))
                    {
                        paragraph.Text = "";
                        paragraph.AppendPicture(qrCodeBytes);
                    }
            }

            #endregion
        }

        private static void ReplaceValuesInParagraph(Paragraph para, JObject contractData)
        {
            foreach (var token in contractData.Children())
            {
                var property = token as JProperty;
                var placeholder = $"{property?.Name}";
                var replacement = property?.Value.ToString();

                // Kiểm tra xem biến có dạng "#gt" không, nếu có thì bỏ qua
                if (placeholder.StartsWith("#gt")) continue;

                // Replace placeholders
                para.Replace(placeholder, replacement, false, true);
            }

            // Replace other values in the paragraph
            foreach (DocumentObject docObj in para.ChildObjects)
                if (docObj is TextRange text)
                {
                    var textValue = text.Text;

                    // Kiểm tra xem văn bản có chứa key không
                    if (textValue.Contains("${"))
                        // Tìm kiếm và thay thế key trong văn bản
                        foreach (var token in contractData.Children())
                        {
                            var property = token as JProperty;
                            var placeholder = $"{property?.Name}";
                            var replacement = property?.Value.ToString();

                            text.Text = text.Text.Replace(placeholder, replacement);
                        }
                }
        }

        public static byte[] ConvertWordToPdf(byte[] wordBytes)
        {
            //Spire.Doc.License.LicenseProvider.SetLicenseKey("Idd65VXxKpEAgBvZ1nVhUN+w7vpItcbvurq9YsmKuDda+JAEE9qF2G4YzR3o0s96HLaSfKKXq8fmv/VifgjLP/ZHrAKRewKyimE+b1l5tI82tdsWa+W3TgkLfepngT3Ui+LuaUc8pxXYEPd/bacNeg6yvWi7xVPzxDsE/m3D+OyD1ifz4S4lkOhjUS4pJ9gIKv6eIx0aXzRyczi4c+55+yRRBjUsB3AUS5C4sGq4LaSbeVLRq52visiCeMQxIkO6G38uTOyJl3mplKPrB3tpSTpmDc0j1WLuce1KIA9GbtKqOgh5vJwnXnwR3qeVgEBY2Lgrt6Gu0RModahYN6N5ODyj526SSOsz50jUQsrjfnk2JYKq3D3GA+lshknDJsSyHHkqYNxXfha7GQ4e11FhxALPu81LBXLXez4l73XCV9n6cdvHnyOerI18clWh/g6lgfEG+N+ugko2oxET/WEeIVKoIvpEw9YMv5bQrD6oWlN5GthgiXawtPQ6kM41r0MKW75+6ojDqRbOqvyVwC4HNRf2MXjni/Bdo0KBG3SD119bQfa+4zBREiEz6X26Mv7Tc0n8YzGTcK7VZcRGqI06bp4RDiFvAMrn4Y83gJaVRX6MbSJqwpKXKugSrmf0ck6XzLmhQcjsznnLkToXxvBS2jh6Vy3JZXvt4l8JUF8zE9CPix+kpDcGedXA1MmN/dju6Ps4sgGGAnjrfl1YLHvbQR8kii+h9tKrUrjTT88xvjjwz5IXmC4MX2A6HjSqabQwLVm8wfwNF22Pp1nMuX5DVP2pyNMMYMHIewGlJRSQz3j/7gVbw264aeBJPGyVpxrZCRO7byu/Z8cKTk02S+vZTazhIjV4jmn8zLOsxH0wsbcEpDLw1XnrH4tUiIRDQxRO+EBtpPklyFx9Q8AYkIv91osUiQZ14MXfysJ8oHG8gqHa7uidcd+YgFc3FRlFlVXYqqQlABFg5/ZvUHUklZdiRLenTb2yfl3RffnzA1aevJcLy2sBoWUrTxZlAFu0u8D2+swu0V3juiLM8pO9VDB4gHtQh3n/cnvShuv8hls2fi0TTZvpxLdfBw==");
            //Spire.Doc.License.LicenseProvider.LoadLicense();

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

        //public static byte[] ConvertToPdfUseAspose(byte[] wordBytes)
        //{
        //    var tmpWordFile = Path.GetTempFileName();
        //    var tmpPDFFile = Path.GetTempFileName();
        //    File.WriteAllBytes(tmpWordFile, wordBytes);
        //    byte[] pdfBytes = null;

        //    Application appWord = new Application();
        //    if (appWord.Documents != null)
        //    {
        //        //yourDoc is your word document
        //        var wordDocument = appWord.Documents.Open(tmpWordFile);
        //        if (wordDocument != null)
        //        {
        //            wordDocument.ExportAsFixedFormat(tmpPDFFile,
        //                WdExportFormat.wdExportFormatPDF);
        //            pdfBytes = File.ReadAllBytes(tmpPDFFile);
        //            File.Delete(tmpPDFFile);
        //            wordDocument.Close();
        //        }

        //        appWord.Quit();
        //    }
        //    File.Delete(tmpWordFile);

        //    return pdfBytes;
        //}

        public static byte[] RenderModelToPdfAndSave(string templatePath, string json, int iTable, int iRowInTable,
            string qrCode)
        {
            var wordBytes = RenderContract(templatePath, json, iTable, iRowInTable, qrCode);
            return ConvertWordToPdf(wordBytes);
        }
    }
}