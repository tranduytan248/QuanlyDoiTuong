using System;
using System.Collections.Generic;
using System.Net;
using Core.Inv.InvBusinessService;
using Core.Inv.InvPortalService;
using Core.Inv.InvPublishService;
using TSFramework.App.Processors;

namespace Core.Inv.Providers
{
    public class InvServiceProvider
    {
        #region Error

        private readonly Dictionary<string, Dictionary<string, string>> _arrErrorService =
            new Dictionary<string, Dictionary<string, string>>
            {
                #region UpdateCus

                {
                    "UpdateCus", new Dictionary<string, string>
                    {
                        { "-1", "Tài khoản đăng nhập sai hoặc không có quyền thêm khách hàng" },
                        { "-2", "Không import được khách hàng vào db" },
                        { "-3", "Dữ liệu xml đầu vào không đúng quy định" },
                        { "-5", "User đã tồn tại rồi" }
                    }
                },

                #endregion

                #region ImportAndPublishInv

                {
                    "ImportAndPublishInv", new Dictionary<string, string>
                    {
                        { "ERR:1", "Tài khoản đăng nhập sai hoặc không có quyền thêm khách hàng" },
                        { "ERR:3", "Dữ liệu xml đầu vào không đúng quy định" },
                        { "ERR:7", "User name không phù hợp, không tìm thấy company tương ứng cho user." },
                        {
                            "ERR:20",
                            "Pattern và serial không phù hợp, hoặc không tồn tại hóa đơn đã đăng kí có sử dụng Pattern và serial truyền vào"
                        },
                        { "ERR:5", "Không phát hành được hóa đơn" },
                        { "ERR:10", "Lô có số hóa đơn vượt quá max cho phép" },
                        { "ERR:6", "Dải hóa đơn không đủ số hóa đơn cho lô phát hành" },
                        { "ERR:13", "Lỗi trùng fkey" },
                        { "ERR:21", "Lỗi trùng số hóa đơn" },
                        { "ERR:29", "Lỗi chứng thư hết hạn" },
                        { "ERR:30", "Danh sách hóa đơn tồn tại ngày hóa đơn nhỏ hơn ngày hóa đơn đã phát hành" }
                    }
                },

                #endregion

                #region confirmPaymentFkey

                {
                    "confirmPaymentFkey", new Dictionary<string, string>
                    {
                        { "ERR:1", "Tài khoản đăng nhập sai" },
                        { "ERR:2", "Chuỗi token không chính xác" },
                        { "ERR:6", "Không tìm thấy hóa đơn tương ứng chuỗi đưa vào" },
                        { "ERR:7", "Không phân phối được" },
                        { "ERR:13", "Hóa đơn đã gạch nợ rồi" }
                    }
                },

                #endregion

                #region confirmPayment

                {
                    "confirmPayment", new Dictionary<string, string>
                    {
                        { "ERR:1", "Tài khoản đăng nhập sai" },
                        { "ERR:2", "Chuỗi token không chính xác" },
                        { "ERR:6", "Không tìm thấy hóa đơn tương ứng chuỗi đưa vào" },
                        { "ERR:7", "Không phân phối được" },
                        { "ERR:13", "Hóa đơn đã gạch nợ rồi" }
                    }
                },

                #endregion

                #region UnConfirmPaymentFkey

                {
                    "UnConfirmPaymentFkey", new Dictionary<string, string>
                    {
                        { "ERR:1", "Tài khoản đăng nhập sai" },
                        { "ERR:2", "Chuỗi token không chính xác" },
                        { "ERR:6", "Không tìm thấy hóa đơn tương ứng chuỗi đưa vào" },
                        { "ERR:7", "Không bỏ gạch nợ được" }
                    }
                },

                #endregion

                #region AdjustInvoiceAction

                {
                    "AdjustInvoiceAction", new Dictionary<string, string>
                    {
                        { "ERR:1", "Tài khoản đăng nhập sai hoặc không có quyền" },
                        { "ERR:2", "Hóa đơn cần điều chỉnh không tồn tại" },
                        { "ERR:3", "Dữ liệu xml đầu vào không đúng quy định" },
                        { "ERR:5", "Không phát hành được hóa đơn" },
                        { "ERR:6", "Dải hóa đơn cũ đã hết" },
                        { "ERR:7", "User name không phù hợp, không tìm thấy company tương ứng cho user." },
                        { "ERR:8", "Hóa đơn cần điều chỉnh đã bị thay thế. Không thể điều chỉnh được nữa." },
                        { "ERR:9", "Trạng thái hóa đơn không được điều chỉnh" },
                        { "ERR:13", "Lỗi trùng fkey" },
                        { "ERR:14", "Lỗi trong quá trình thực hiện cấp số hóa đơn" },
                        { "ERR:15", "Lỗi khi thực hiện Deserialize chuỗi hóa đơn đầu vào" },
                        { "ERR:19", "Pattern truyền vào không giống với hóa đơn cần điều chỉnh" },
                        {
                            "ERR:20",
                            "Dải hóa đơn hết, User/Account không có quyền với Serial/Pattern và serial không phù hợp"
                        },
                        { "ERR:21", "Trùng Fkey truyền vào" },
                        { "ERR:29", "Lỗi chứng thư hết hạn" },
                        { "ERR:30", "Danh sách hóa đơn tồn tại ngày hóa đơn nhỏ hơn ngày hóa đơn đã phát hành" }
                    }
                },

                #endregion

                #region ReplaceInvoiceAction

                {
                    "ReplaceInvoiceAction", new Dictionary<string, string>
                    {
                        { "ERR:1", "Tài khoản đăng nhập sai hoặc không có quyền thêm khách hàng" },
                        { "ERR:2", "Không tồn tại hóa đơn cần thay thế" },
                        { "ERR:3", "Dữ liệu xml đầu vào không đúng quy định" },
                        { "ERR:5", "Có lỗi trong quá trình thay thế hóa đơn" },
                        { "ERR:6", "Dải hóa đơn cũ đã hết" },
                        { "ERR:7", "User name không phù hợp, không tìm thấy company tương ứng cho user." },
                        { "ERR:8", "Hóa đơn đã được thay thế rồi. Không thể thay thế nữa." },
                        { "ERR:20", "Pattern và serial không phù hợp" },
                        { "ERR:9", "Trạng thái hóa đơn ko được thay thế" }
                    }
                },

                #endregion

                #region listInvByCus

                {
                    "listInvByCus", new Dictionary<string, string>
                    {
                        { "ERR:1", "Tài khoản đăng nhập sai" },
                        { "ERR:3", "Không tồn tài khách hàng tương ứng với cusCode" },
                        { "ERR:4", "Công ty chưa được đăng kí mẫu hóa đơn nào" }
                    }
                },

                #endregion

                #region cancelInvNoPay

                {
                    "cancelInvNoPay", new Dictionary<string, string>
                    {
                        { "ERR:1", "Tài khoản đăng nhập sai hoặc không có quyền thêm khách hàng" },
                        { "ERR:2", "Không tồn tại hóa đơn cần hủy" },
                        { "ERR:8", "Hóa đơn đã được thay thế rồi, hủy rồi" },
                        { "ERR:9", "Trạng thái hóa đơn ko được hủy" }
                    }
                },

                #endregion

                #region deleteInvoiceByFkey

                {
                    "deleteInvoiceByFkey", new Dictionary<string, string>
                    {
                        { "ERR:1", "Tài khoản đăng nhập sai, hoặc không có quyền" },
                        { "ERR:7", "Không tìm thấy công ty" },
                        { "ERR:10", "Số hóa đơn truyền vào vượt quá số lượng cho phép" },
                        { "ERR:20", "Pattern và Serial không hợp lệ" },
                        { "ERR:5", "Lỗi không xác định" }
                    }
                },

                #endregion

                #region getInvViewFkey

                {
                    "getInvViewFkey", new Dictionary<string, string>
                    {
                        { "ERR:1", "Tài khoản đăng nhập sai hoặc không có quyền" },
                        { "ERR:4", "Công ty chưa được đăng kí mẫu hóa đơn nào" },
                        { "ERR:6", "Không tìm thấy hóa đơn" },
                        { "ERR:7", "Không tìm thấy thông tin công ty" },
                        { "ERR:11", "Hóa đơn chưa thanh toán nên không xem được" },
                        { "ERR:12", "Hoá đơn có mã chưa được thuế chấp nhận" },
                        { "ERR:13", "Hoá đơn không mã chưa được thuế chấp nhận" }
                    }
                },

                #endregion

                #region getInvViewFkeyNoPay

                {
                    "getInvViewFkeyNoPay", new Dictionary<string, string>
                    {
                        { "ERR:1", "Tài khoản đăng nhập sai hoặc không có quyền" },
                        { "ERR:4", "Công ty chưa được đăng kí mẫu hóa đơn nào" },
                        { "ERR:6", "Không tìm thấy hóa đơn" },
                        { "ERR:7", "Không tìm thấy thông tin công ty" },
                        { "ERR:11", "Hóa đơn chưa thanh toán nên không xem được" },
                        { "ERR:12", "Hoá đơn có mã chưa được thuế chấp nhận" },
                        { "ERR:13", "Hoá đơn không mã chưa được thuế chấp nhận" }
                    }
                },

                #endregion

                #region convertForStoreFkey

                {
                    "convertForStoreFkey", new Dictionary<string, string>
                    {
                        { "ERR:1", "Tài khoản đăng nhập sai hoặc không có quyền" },
                        { "ERR:2", "Chuỗi token không chính xác" },
                        { "ERR:7", "Không tìm thấy công ty" },
                        { "ERR:6", "Không tìm thấy hóa đơn" },
                        { "ERR:8", "Hóa đơn đã chuyển đổi" },
                        { "ERR:5", "Có lỗi xảy ra" },
                        { "ERR:12", "Hoá đơn có mã chưa được thuế chấp nhận" },
                        { "ERR:13", "Hoá đơn không mã chưa được thuế chấp nhận" }
                    }
                },

                #endregion

                #region downloadInvPDFFkey

                {
                    "downloadInvPDFFkey", new Dictionary<string, string>
                    {
                        { "ERR:1", "Tài khoản đăng nhập sai hoặc không có quyền ServiceRole" },
                        { "ERR:2", "Chuỗi token không đúng định dạng" },
                        { "ERR:4", "Không tìm thấy dải thông báo phát hành" },
                        { "ERR:6", "Không tìm thấy hóa đơn" },
                        { "ERR:7", "User name không phù hợp, không tìm thấy thông tin công ty tương ứng cho user." },
                        { "ERR:12", "Hoá đơn có mã chưa được thuế chấp nhận" },
                        { "ERR:13", "Hoá đơn không mã chưa được thuế chấp nhận" }
                    }
                },

                #endregion

                #region downloadInvFkey

                {
                    "downloadInvFkey", new Dictionary<string, string>
                    {
                        { "ERR:1", "Tài khoản đăng nhập sai hoặc không có quyền ServiceRole" },
                        { "ERR:2", "Chuỗi token không đúng định dạng" },
                        { "ERR:4", "Không tìm thấy dải thông báo phát hành" },
                        { "ERR:6", "Không tìm thấy hóa đơn" },
                        { "ERR:7", "User name không phù hợp, không tìm thấy thông tin công ty tương ứng cho user." },
                        { "ERR:11", "Hóa đơn chưa thanh toán nên không xem được" },
                        { "ERR:12", "Hoá đơn có mã chưa được thuế chấp nhận" },
                        { "ERR:13", "Hoá đơn không mã chưa được thuế chấp nhận" }
                    }
                },

                #endregion

                #region listInvByCusFkey

                {
                    "listInvByCusFkey", new Dictionary<string, string>
                    {
                        { "ERR:1", "Tài khoản đăng nhập sai hoặc không có quyền" },
                        { "ERR:4", "Công ty chưa được đăng kí mẫu hóa đơn nào" },
                        { "ERR:7", "Không tìm thấy thông tin công ty" },
                        { "ERR:", "Có lỗi xảy ra" }
                    }
                },

                #endregion

                #region getSerialByPattern

                {
                    "getSerialByPattern", new Dictionary<string, string>
                    {
                        { "ERR:1", "Tài khoản đăng nhập sai" },
                        { "ERR:20", "Không có danh sách ký hiệu hóa đơn" }
                    }
                },

                #endregion

                #region getCus

                {
                    "getCus", new Dictionary<string, string>
                    {
                        { "ERR:1", "Tài khoản đăng nhập sai hoặc không có quyền" },
                        { "ERR:7", "Không tìm thấy khách hàng hoặc công ty tương ứng" },
                        { "ERR:3", "Không tìm thấy thông tin khách hàng" },
                        { "ERR:", "Lỗi không xác định" }
                    }
                },

                #endregion
            };

        #endregion

        private readonly BusinessService _businessClient = new BusinessService();
        private readonly PortalService _portalClient = new PortalService();
        private readonly PublishService _publishClient = new PublishService();

        /*================================================================*/

        public InvServiceProvider(string urlPortalService, string urlBusinessService, string urlPublishService)
        {
            _portalClient.Url = urlPortalService;
            _publishClient.Url = urlPublishService;
            _businessClient.Url = urlBusinessService;
        }

        public bool IsOnline()
        {
            try
            {
                var clientTest = new WebClient();
                clientTest.OpenRead(_portalClient.Url);
                clientTest.OpenRead(_publishClient.Url);
                clientTest.OpenRead(_businessClient.Url);

                var portalRequest = (HttpWebRequest)WebRequest.Create(_portalClient.Url);
                var portalResponse = (HttpWebResponse)portalRequest.GetResponse();

                var publishRequest = (HttpWebRequest)WebRequest.Create(_portalClient.Url);
                var publishResponse = (HttpWebResponse)publishRequest.GetResponse();

                var businessRequest = (HttpWebRequest)WebRequest.Create(_portalClient.Url);
                var businessResponse = (HttpWebResponse)businessRequest.GetResponse();

                return portalResponse.StatusCode == HttpStatusCode.OK &&
                       publishResponse.StatusCode == HttpStatusCode.OK &&
                       businessResponse.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        public bool IsCorrectUser(string sInvServiceAccount, string sInvServiceAcPass)
        {
            try
            {
                var errCode = _publishClient.GetCertInfo(sInvServiceAccount, sInvServiceAcPass);
                return !errCode.Contains("ERR:");
            }
            catch
            {
                return false;
            }
        }

        #region Report

        /// <summary>
        ///     Lấy báo cáo sử dụng hoá đơn, tổng số hoá đơn phát hành, đã sử dụng, còn lại
        /// </summary>
        /// <param name="iYear">Năm</param>
        /// <param name="iQuater">Quý</param>
        /// <param name="iCurrentQuater">Quý hiện tại</param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <returns>Chuỗi XML </returns>
        public string ReportInvUsed(int iYear, int iQuater, int iCurrentQuater, string sInvServiceAccount,
            string sInvServiceAcPass)
        {
            try
            {
                var resultOfService = _businessClient.reportInvUsed(iYear, iQuater, iCurrentQuater, sInvServiceAccount,
                    sInvServiceAcPass);
                return resultOfService;
            }
            catch
            {
                AppProcessor.Notifider.Broadcast(
                    "Kết nối tới service Dịch vụ Hoá đơn điện tử không ổn định. Vui lòng thử lại", -1);
                throw;
            }
        }

        #endregion

        #region E-Invoice Functions

        /// <summary>
        ///     Thêm mới và phát hành các hoá đơn
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="sXmlInvData">
        ///     Danh sách các hoá đơn cần thêm mới dưới dạng chuỗi xml
        ///     ======================================================
        ///     <Invoices>
        ///         <Inv>
        ///             <key>Giá trị khóa để phân biệt hóa đơn xuất cho khách hàng nào</key>
        ///             <Invoice>
        ///                 <CusCode>Mã khách hàng*</CusCode>
        ///                 <CusName>Tên khách hàng*</CusName>
        ///                 <CusAddress>Địa chỉ khách hàng*</CusAddress>
        ///                 <CusPhone>Điện thoại khách hàng</CusPhone>
        ///                 <CusTaxCode>Mã số thuế KH (Bắt buộc với KH là Doanh nghiệp)</CusTaxCode>
        ///                 <PaymentMethod>Phương thức thanh toán</PaymentMethod>
        ///                 <KindOfService>Tháng hóa đơn</KindOfService>
        ///                 <Products>
        ///                     <Product>
        ///                         <ProdName>Tên sản phẩm*</ProdName>
        ///                         <ProdUnit>Đơn vị tính</ProdUnit>
        ///                         <ProdQuantity>Số lượng</ProdQuantity>
        ///                         <ProdPrice>Đơn giá</ProdPrice>
        ///                         <Amount>Tổng tiền*</Amount>
        ///                     </Product>
        ///                 </Products>
        ///                 <Total>Tổng tiền trước thuế*</Total>
        ///                 <DiscountAmount>Tiền giảm trừ</DiscountAmount>
        ///                 <VATRate>Thuế GTGT*</VATRate>
        ///                 <VATAmount>Tiền thuế GTGT*</VATAmount>
        ///                 <Amount>Tổng tiền*</Amount>
        ///                 <AmountInWords>Số tiền viết bằng chữ*</AmountInWords>
        ///             </Invoice>
        ///         </Inv>
        ///     </Invoices>
        /// </param>
        /// <param name="sEmpAccount">Tài khoản nhân viên được cấp quyền tạo hoá đơn</param>
        /// <param name="sEmpAcPass">Mật khẩu tài khoản nhân viên được cấp quyền tạo hoá đơn</param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <param name="sInvPattern">Mẫu hoá đơn đăng ký trên hệ thống hoá đơn điện tử</param>
        /// <param name="sInvSerial">Số serial hoá đơn đăng ký trên hệ thống hoá đơn điện tử</param>
        /// <returns>
        ///     + OK:01GTKT0/003;TP/18E;DNA0100700128_14,...        : Thêm mới và phát hành hoá đơn thành công + danh sách số hoá
        ///     đơn đã phát hành
        ///     [01GTKT0/003;TP/18E;DNA0100700128_14] = [pattern;serial;key_số hoá đơn]
        ///     + ERR:1     : Tài khoản đăng nhập sai hoặc không có quyền
        ///     + ERR:2     : Chuỗi token không chính xác
        ///     + ERR:3     : Dữ liệu xml đầu vào không đúng quy định
        ///     + ERR:4     : Công ty chưa được đăng kí mẫu hóa đơn nào
        ///     + ERR:5     : Không phát hành
        ///     + ERR:6     : Không tìm thấy hóa đơn tương ứng chuỗi đưa vào
        ///     + ERR:7     : Username không phù hợp - không tìm thấy company phù hợp với [Username]
        ///     + ERR:8     : Hóa đơn cần điều chỉnh đã bị thay thế. Không thể điều chỉnh được nữa.
        ///     + ERR:9     : Trạng thái hóa đơn không được điều chỉnh
        ///     + ERR:10    : Lô có số hóa đơn vượt quá max cho phép
        ///     + ERR:11    : Hóa đơn chưa cho deliver, ko xem được
        ///     + ERR:13    : Hóa đơn đã gạch nợ/ bỏ gạch nợ rồi
        ///     + ERR:14    : Lỗi NoFactory
        ///     + ERR:20    : Pattern và serial không phù hợp
        /// </returns>
        public string ImportAndPublishInvoice(out string sErrMsg, string sXmlInvData, string sEmpAccount,
            string sEmpAcPass,
            string sInvServiceAccount, string sInvServiceAcPass, string sInvPattern, string sInvSerial)
        {
            try
            {
                sErrMsg = "";
                var resultOfService = _publishClient.ImportAndPublishInv(
                    sEmpAccount, // Account
                    sEmpAcPass, // ACPass
                    sXmlInvData, // XMLInvData
                    sInvServiceAccount, // UserName
                    sInvServiceAcPass, // Password
                    sInvPattern, // Pattern
                    sInvSerial, // Serial
                    0);
                if (_arrErrorService.ContainsKey("ImportAndPublishInv") &&
                    _arrErrorService["ImportAndPublishInv"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["ImportAndPublishInv"]?[resultOfService];

                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        /// <summary>
        ///     Xác nhận thanh toán cho hoá đơn. Có thể xác nhận cho 1 hoặc nhiều hoá đơn
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="lstInvToken">
        ///     - Danh sách token để nhận dạng hoá đơn cần xác nhận thanh toán. Các token phân biệt nhau bằng dấu "_"
        ///     [lstInvToken = InvToken_InvToken_..]
        ///     InvToken = pattern;serial;invoiceNo        => "01GTKT0/003;TP/18E;14"
        ///     + pattern:mẫu số                          => "01GTKT0/003"
        ///     + serial:ký hiệu                          => "TP/18E"
        ///     + invoiceNo:số hoá đơn                    => "14"
        /// </param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <returns>
        ///     + OK        : Đánh dấu hóa đơn đã được xác nhận thanh toán thành công
        ///     + ERR:1     : Tài khoản đăng nhập sai hoặc không có quyền
        ///     + ERR:2     : Chuỗi token không chính xác
        ///     + ERR:3     : Dữ liệu xml đầu vào không đúng quy định
        ///     + ERR:4     : Công ty chưa được đăng kí mẫu hóa đơn nào
        ///     + ERR:5     : Không phát hành
        ///     + ERR:6     : Không tìm thấy hóa đơn tương ứng chuỗi đưa vào
        ///     + ERR:7     : Username không phù hợp - không tìm thấy company phù hợp với [Username]
        ///     + ERR:8     : Hóa đơn cần điều chỉnh đã bị thay thế. Không thể điều chỉnh được nữa.
        ///     + ERR:9     : Trạng thái hóa đơn không được điều chỉnh
        ///     + ERR:10    : Lô có số hóa đơn vượt quá max cho phép
        ///     + ERR:11    : Hóa đơn chưa cho deliver, ko xem được
        ///     + ERR:13    : Hóa đơn đã gạch nợ/ bỏ gạch nợ rồi
        ///     + ERR:14    : Lỗi NoFactory
        ///     + ERR:20    : Pattern và serial không phù hợp
        /// </returns>
        public string ConfirmPaymentForInvoice(out string sErrMsg, string lstInvToken, string sInvServiceAccount,
            string sInvServiceAcPass)
        {
            try
            {
                sErrMsg = "";
                var resultOfService =
                    _businessClient.confirmPayment(lstInvToken, sInvServiceAccount, sInvServiceAcPass);
                if (_arrErrorService.ContainsKey("confirmPayment") &&
                    _arrErrorService["confirmPayment"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["confirmPayment"]?[resultOfService];
                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        /// <summary>
        ///     Xác nhận thanh toán cho hoá đơn bằng FKey. Có thể xác nhận cho 1 hoặc nhiều hoá đơn
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="lstFKey">Chuỗi Fkey xác định hóa đơn cần lấy(các Fkey phân biệt nhau bằng “_”) </param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <returns>
        ///     + OK        : Đánh dấu hóa đơn đã được xác nhận thanh toán thành công
        ///     + ERR:1     : Tài khoản đăng nhập sai hoặc không có quyền
        ///     + ERR:2     : Chuỗi token không chính xác
        ///     + ERR:3     : Dữ liệu xml đầu vào không đúng quy định
        ///     + ERR:4     : Công ty chưa được đăng kí mẫu hóa đơn nào
        ///     + ERR:5     : Không phát hành
        ///     + ERR:6     : Không tìm thấy hóa đơn tương ứng chuỗi đưa vào
        ///     + ERR:7     : Username không phù hợp - không tìm thấy company phù hợp với [Username]
        ///     + ERR:8     : Hóa đơn cần điều chỉnh đã bị thay thế. Không thể điều chỉnh được nữa.
        ///     + ERR:9     : Trạng thái hóa đơn không được điều chỉnh
        ///     + ERR:10    : Lô có số hóa đơn vượt quá max cho phép
        ///     + ERR:11    : Hóa đơn chưa cho deliver, ko xem được
        ///     + ERR:13    : Hóa đơn đã gạch nợ/ bỏ gạch nợ rồi
        ///     + ERR:14    : Lỗi NoFactory
        ///     + ERR:20    : Pattern và serial không phù hợp
        /// </returns>
        public string ConfirmPaymentForInvoiceByFKey(out string sErrMsg, string lstFKey, string sInvServiceAccount,
            string sInvServiceAcPass)
        {
            try
            {
                sErrMsg = "";
                var resultOfService =
                    _businessClient.confirmPaymentFkey(lstFKey, sInvServiceAccount, sInvServiceAcPass);
                if (_arrErrorService.ContainsKey("confirmPaymentFkey") &&
                    _arrErrorService["confirmPaymentFkey"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["confirmPaymentFkey"]?[resultOfService];
                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        /// <summary>
        ///     Huỷ xác nhận thanh toán cho hoá đơn. Có thể huỷ xác nhận cho 1 hoặc nhiều hoá đơn
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="lstInvToken">
        ///     - Danh sách các token để nhận dạng hoá đơn cần huỷ xác nhận thanh toán. Các token phân biệt nhau bằng dấu "_"
        ///     [lstInvToken = InvToken_InvToken_..]
        ///     InvToken = pattern;serial;invoiceNo        => "01GTKT0/003;TP/18E;14"
        ///     + pattern:mẫu số                          => "01GTKT0/003"
        ///     + serial:ký hiệu                          => "TP/18E"
        ///     + invoiceNo:số hoá đơn                    => "14"
        /// </param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <returns>
        ///     + OK        : Đánh dấu hóa đơn đã được huỷ xác nhận thanh toán thành công
        ///     + ERR:1     : Tài khoản đăng nhập sai hoặc không có quyền
        ///     + ERR:2     : Chuỗi token không chính xác
        ///     + ERR:3     : Dữ liệu xml đầu vào không đúng quy định
        ///     + ERR:4     : Công ty chưa được đăng kí mẫu hóa đơn nào
        ///     + ERR:5     : Không phát hành
        ///     + ERR:6     : Không tìm thấy hóa đơn tương ứng chuỗi đưa vào
        ///     + ERR:7     : Username không phù hợp - không tìm thấy company phù hợp với [Username]
        ///     + ERR:8     : Hóa đơn cần điều chỉnh đã bị thay thế. Không thể điều chỉnh được nữa.
        ///     + ERR:9     : Trạng thái hóa đơn không được điều chỉnh
        ///     + ERR:10    : Lô có số hóa đơn vượt quá max cho phép
        ///     + ERR:11    : Hóa đơn chưa cho deliver, ko xem được
        ///     + ERR:13    : Hóa đơn đã gạch nợ/ bỏ gạch nợ rồi
        ///     + ERR:14    : Lỗi NoFactory
        ///     + ERR:20    : Pattern và serial không phù hợp
        /// </returns>
        public string UnConfirmPaymentForInvoice(out string sErrMsg, string lstInvToken, string sInvServiceAccount,
            string sInvServiceAcPass)
        {
            try
            {
                sErrMsg = "";
                var resultOfService =
                    _businessClient.UnConfirmPaymentFkey(lstInvToken, sInvServiceAccount, sInvServiceAcPass);
                if (_arrErrorService.ContainsKey("UnConfirmPaymentFkey") &&
                    _arrErrorService["UnConfirmPaymentFkey"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["UnConfirmPaymentFkey"]?[resultOfService];
                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        /// <summary>
        ///     Điều chỉnh hoá đơn.
        ///     ==========================================
        ///     Có 3 loại điều chỉnh hoá đơn:
        ///     + 2 - Điều chỉnh tăng
        ///     + 3 - Điều chỉnh giảm
        ///     + 4 - Hóa đơn điều chỉnh thông tin
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="sEmpAccount">Tài khoản nhân viên được cấp quyền tạo hoá đơn</param>
        /// <param name="sEmpAcPass">Mật khẩu tài khoản nhân viên được cấp quyền tạo hoá đơn</param>
        /// <param name="sXmlInvData">
        ///     Chuỗi xml dữ liệu hoá đơn cần điều chỉnh
        ///     ============================================
        ///     Cấu trúc chuỗi xml:
        ///     <AdjustInv>
        ///         <key>Khóa cho hóa đơn mới</key>
        ///         <CusCode>Mã khách hàng*</CusCode>
        ///         <CusName>Tên khách hàng*</CusName>
        ///         <CusAddress>Địa chỉ khách hàng*</CusAddress>
        ///         <CusPhone>Điện thoại khách hàng</CusPhone>
        ///         <CusTaxCode>Mã số thuế KH (Bắt buộc với KH là Doanh nghiệp)</CusTaxCode>
        ///         <PaymentMethod>Phương thức thanh toán</PaymentMethod>
        ///         <KindOfService>Tháng hóa đơn</KindOfService>
        ///         <Type>
        ///             Loại hóa đơn chỉnh sửa(int-mặc định lấy là 2)  2-Điều chỉnh tăng, 3-Điều chỉnh giảm, 4- Hóa đơn điều
        ///             chỉnh thông tin
        ///         </Type>
        ///         <Products>
        ///             <Product>
        ///                 <ProdName>Tên sản phẩm*</ProdName>
        ///                 <ProdUnit>Đơn vị tính</ProdUnit>
        ///                 <ProdQuantity>Số lượng</ProdQuantity>
        ///                 <ProdPrice>Đơn giá</ProdPrice>
        ///                 <Amount>Tổng tiền*</Amount>
        ///             </Product>
        ///         </Products>
        ///         <Total>Tổng tiền trước thuế*</Total>
        ///         <VATRate>Thuế GTGT*</VATRate>
        ///         <VATAmount>Tiền thuế GTGT*</VATAmount>
        ///         <Amount>Tổng tiền*</Amount>
        ///         <AmountInWords>Số tiền viết bằng chữ*</AmountInWords>
        ///     </AdjustInv>
        /// </param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <param name="sFKey">Chuỗi xác định hoá đơn cần điều chỉnh</param>
        /// <param name="sAttachFile"></param>
        /// <param name="iConvert">Mặc định là 0, 0 – Không cần convert từ TCVN3 sang Unicode. 1- Cần convert từ TCVN3 sang Unicode </param>
        /// <param name="sInvPattern"></param>
        /// <param name="sInvSerial"></param>
        /// <returns>
        ///     + OK:01GTKT0/003;TP/18E;14,...        : Phát hành hoá đơn thành công + số hoá đơn đã phát hành
        ///     [01GTKT0/003;TP/18E;14] = [pattern;serial;số hoá đơn điều chỉnh]
        ///     + ERR:1     : Tài khoản đăng nhập sai hoặc không có quyền
        ///     + ERR:2     : Hóa đơn cần điều chỉnh không tồn tại
        ///     + ERR:3     : Dữ liệu xml đầu vào không đúng quy định
        ///     + ERR:5     : Không phát hành được hóa đơn
        ///     + ERR:6     : Dải hóa đơn cũ đã hết
        ///     + ERR:7     : Username không phù hợp - không tìm thấy company phù hợp với [Username]
        ///     + ERR:8     : Hóa đơn cần điều chỉnh đã bị thay thế. Không thể điều chỉnh được nữa.
        ///     + ERR:9     : Trạng thái hóa đơn không được điều chỉnh
        /// </returns>
        public string AdjustInvoice(out string sErrMsg, string sEmpAccount, string sEmpAcPass, string sXmlInvData,
            string sInvServiceAccount, string sInvServiceAcPass, string sFKey, string sInvPattern,
            string sInvSerial, string sAttachFile = null, int iConvert = 0)
        {
            try
            {
                sErrMsg = "";
                var resultOfService = _businessClient.AdjustInvoiceAction(
                    sEmpAccount, // Account
                    sEmpAcPass, // ACPass
                    sXmlInvData, // XMLInvData
                    sInvServiceAccount, // UserName
                    sInvServiceAcPass, // Password
                    sFKey, // FKey
                    sAttachFile, // Attach File
                    iConvert,
                    sInvPattern,
                    sInvSerial);

                if (_arrErrorService.ContainsKey("AdjustInvoiceAction") &&
                    _arrErrorService["AdjustInvoiceAction"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["AdjustInvoiceAction"]?[resultOfService];
                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        /// <summary>
        ///     Thay thế hoá đơn.
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="sEmpAccount">Tài khoản nhân viên được cấp quyền tạo hoá đơn</param>
        /// <param name="sEmpAcPass">Mật khẩu tài khoản nhân viên được cấp quyền tạo hoá đơn</param>
        /// <param name="sXmlInvData">
        ///     Chuỗi xml dữ liệu hoá đơn cần điều chỉnh
        ///     ============================================
        ///     Cấu trúc chuỗi xml:
        ///     <ReplaceInv>
        ///         <key>Chuỗi xác định hóa đơn mới</key>
        ///         <CusCode>Mã khách hàng*</CusCode>
        ///         <CusName>Tên khách hàng*</CusName>
        ///         <CusAddress>Địa chỉ khách hàng*</CusAddress>
        ///         <CusPhone>Điện thoại khách hàng</CusPhone>
        ///         <CusTaxCode>Mã số thuế KH (Bắt buộc với KH là Doanh nghiệp)</CusTaxCode>
        ///         <PaymentMethod>Phương thức thanh toán</PaymentMethod>
        ///         <KindOfService>Tháng hóa đơn</KindOfService>
        ///         <Products>
        ///             <Product>
        ///                 <ProdName>Tên sản phẩm*</ProdName>
        ///                 <ProdUnit>Đơn vị tính</ProdUnit>
        ///                 <ProdQuantity>Số lượng</ProdQuantity>
        ///                 <ProdPrice>Đơn giá</ProdPrice>
        ///                 <Amount>Tổng tiền*</Amount>
        ///             </Product>
        ///         </Products>
        ///         <Total>Tổng tiền trước thuế*</Total>
        ///         <VATRate>Thuế GTGT*</VATRate>
        ///         <VATAmount>Tiền thuế GTGT*</VATAmount>
        ///         <Amount>Tổng tiền*</Amount>
        ///         <AmountInWords>Số tiền viết bằng chữ*</AmountInWords>
        ///     </ReplaceInv>
        /// </param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <param name="sFKey">Chuỗi xác định hoá đơn cần điều chỉnh</param>
        /// <param name="sAttachFile"></param>
        /// <param name="iConvert">Mặc định là 0, 0 – Không cần convert từ TCVN3 sang Unicode. 1- Cần convert từ TCVN3 sang Unicode </param>
        /// <param name="sInvPattern"></param>
        /// <param name="sInvSerial"></param>
        /// <returns>
        ///     + OK:01GTKT0/003;TP/18E;14,...        : Thay thế hoá đơn thành công
        ///     [01GTKT0/003;TP/18E;14] = [pattern;serial;số hoá đơn mới thay cho số hoá đơn cũ]
        ///     + ERR:1     : Tài khoản đăng nhập sai hoặc không có quyền
        ///     + ERR:2     : Không tồn tại hóa đơn cần thay thế
        ///     + ERR:3     : Dữ liệu xml đầu vào không đúng quy định
        ///     + ERR:5     : Có lỗi trong quá trình thay thế hóa đơn
        ///     + ERR:6     : Dải hóa đơn cũ đã hết
        ///     + ERR:7     : Username không phù hợp - không tìm thấy company phù hợp với [Username]
        ///     + ERR:8     : Hóa đơn cần điều chỉnh đã bị thay thế. Không thể điều chỉnh được nữa.
        ///     + ERR:9     : Trạng thái hóa đơn không được điều chỉnh
        ///     + ERR:20    : Pattern và serial không phù hợp
        /// </returns>
        public string ReplaceInvoice(out string sErrMsg, string sEmpAccount, string sEmpAcPass, string sXmlInvData,
            string sInvServiceAccount, string sInvServiceAcPass, string sFKey, string sInvPattern,
            string sInvSerial, string sAttachFile = null, int iConvert = 0)
        {
            try
            {
                sErrMsg = "";
                var resultOfService = _businessClient.ReplaceInvoiceAction(
                    sEmpAccount, // Account
                    sEmpAcPass, // ACPass
                    sXmlInvData, // XMLInvData
                    sInvServiceAccount, // UserName
                    sInvServiceAcPass, // Password
                    sFKey, // FKey
                    sAttachFile, // Attach File
                    iConvert,
                    sInvPattern,
                    sInvSerial);
                if (_arrErrorService.ContainsKey("ReplaceInvoiceAction") &&
                    _arrErrorService["ReplaceInvoiceAction"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["ReplaceInvoiceAction"]?[resultOfService];

                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        /// <summary>
        ///     Lấy danh sách hoá đơn theo mã khách hàng
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="cusCode">Mã khách hàng</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <returns>
        ///     Danh sách các hoá đơn theo dạng xml
        ///     <Data>
        ///         <Item>
        ///             <index>Tháng xuất hóa đơn</index>
        ///             <invToken>Chuỗi token để xác định hóa đơn</invToken>
        ///             <name>Tên hóa đơn</name>
        ///             <publishDate>Ngày phát hành hóa đơn</publishDate>
        ///             <signStatus>Trạng thái kí khách hàng</signStatus>
        ///             <pattern>Mẫu hóa đơn</pattern>
        ///             <serial>Serial hóa đơn</serial>
        ///             <invNum>Số hóa đơn</invNum>
        ///             <payment>trạng thái thanh toán(0,1)</payment>
        ///             <Amount>Tổng tiền của hóa đơn</Amount>
        ///             <status>Trạng thái hóa đơn(1,3,4)</status>
        ///         </Item>
        ///     </Data>
        /// </returns>
        public string GetListInvoiceByCustomer(out string sErrMsg, string cusCode, string fromDate, string toDate,
            string sInvServiceAccount, string sInvServiceAcPass)
        {
            try
            {
                sErrMsg = "";
                var resultOfService =
                    _portalClient.listInvByCus(cusCode, fromDate, toDate, sInvServiceAccount, sInvServiceAcPass);
                if (_arrErrorService.ContainsKey("listInvByCus") &&
                    _arrErrorService["listInvByCus"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["listInvByCus"]?[resultOfService];
                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        /// <summary>
        ///     Huỷ hoá đơn
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="sEmpAccount">Tài khoản nhân viên được cấp quyền tạo hoá đơn</param>
        /// <param name="sEmpAcPass">Mật khẩu tài khoản nhân viên được cấp quyền tạo hoá đơn</param>
        /// <param name="sFKey">FKey của hoá đơn cần huỷ</param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <returns>
        ///     + OK:       : Huỷ hoá đơn thành công
        ///     + ERR:1     : Tài khoản đăng nhập sai hoặc không có quyền
        ///     + ERR:2     : Không tồn tại hóa đơn cần hủy
        ///     + ERR:8     : Hóa đơn đã được thay thế rồi, hủy rồi.
        ///     + ERR:9     : Trạng thái hóa đơn ko được hủy
        /// </returns>
        public string CancelInvoice(out string sErrMsg, string sEmpAccount, string sEmpAcPass, string sFKey,
            string sInvServiceAccount, string sInvServiceAcPass)
        {
            try
            {
                sErrMsg = "";
                var resultOfService = _businessClient.cancelInvNoPay(sEmpAccount, sEmpAcPass, sFKey,
                    sInvServiceAccount,
                    sInvServiceAcPass);
                if (_arrErrorService.ContainsKey("cancelInvNoPay") &&
                    _arrErrorService["cancelInvNoPay"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["cancelInvNoPay"]?[resultOfService];
                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        /// <summary>
        ///     Huỷ nhiều hoá đơn bằng FKey
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="sEmpAccount">Tài khoản nhân viên được cấp quyền tạo hoá đơn</param>
        /// <param name="sEmpAcPass">Mật khẩu tài khoản nhân viên được cấp quyền tạo hoá đơn</param>
        /// <param name="lsFKey">Danh sách FKey của hoá đơn cần huỷ, các FKey phân biệt bởi dấu "_"</param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <param name="sPattern"></param>
        /// <returns>
        ///     + OK:       : Huỷ hoá đơn thành công
        ///     + ERR:1     : Tài khoản đăng nhập sai hoặc không có quyền
        ///     + ERR:2     : Không tồn tại hóa đơn cần hủy
        ///     + ERR:8     : Hóa đơn đã được thay thế rồi, hủy rồi.
        ///     + ERR:9     : Trạng thái hóa đơn ko được hủy
        /// </returns>
        public string CancelListInvoices(out string sErrMsg, string sEmpAccount, string sEmpAcPass, string lsFKey,
            string sInvServiceAccount, string sInvServiceAcPass, string sPattern)
        {
            try
            {
                sErrMsg = "";
                var resultOfService = _publishClient.deleteInvoiceByFkey(lsFKey, sEmpAccount, sEmpAcPass,
                    sInvServiceAccount,
                    sInvServiceAcPass);
                if (_arrErrorService.ContainsKey("deleteInvoiceByFkey") &&
                    _arrErrorService["deleteInvoiceByFkey"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["deleteInvoiceByFkey"]?[resultOfService];
                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        /// <summary>
        ///     Lấy nội dung html của hoá đơn để hiển thị
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="sFKey"></param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <returns>
        ///     + string_html        : chuỗi html thể hiện nội dung của hoá đơn
        ///     + ERR:1     : Tài khoản đăng nhập sai hoặc không có quyền
        ///     + ERR:2     : Chuỗi token không chính xác
        ///     + ERR:3     : Dữ liệu xml đầu vào không đúng quy định
        ///     + ERR:4     : Công ty chưa được đăng kí mẫu hóa đơn nào
        ///     + ERR:5     : Không phát hành
        ///     + ERR:6     : Không tìm thấy hóa đơn tương ứng chuỗi đưa vào
        ///     + ERR:7     : Username không phù hợp - không tìm thấy company phù hợp với [Username]
        ///     + ERR:8     : Hóa đơn cần điều chỉnh đã bị thay thế. Không thể điều chỉnh được nữa.
        ///     + ERR:9     : Trạng thái hóa đơn không được điều chỉnh
        ///     + ERR:10    : Lô có số hóa đơn vượt quá max cho phép
        ///     + ERR:11    : Hóa đơn chưa cho deliver, ko xem được
        ///     + ERR:13    : Hóa đơn đã gạch nợ/ bỏ gạch nợ rồi
        ///     + ERR:14    : Lỗi NoFactory
        ///     + ERR:20    : Pattern và serial không phù hợp
        /// </returns>
        public string GetInvViewByFKey(out string sErrMsg, string sFKey, string sInvServiceAccount,
            string sInvServiceAcPass)
        {
            try
            {
                sErrMsg = "";
                var resultOfService = _portalClient.getInvViewFkey(sFKey, sInvServiceAccount, sInvServiceAcPass);
                if (_arrErrorService.ContainsKey("getInvViewFkey") &&
                    _arrErrorService["getInvViewFkey"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["getInvViewFkey"]?[resultOfService];
                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        /// <summary>
        ///     Lấy nội dung html của hoá đơn để hiển thị, không cần thanh toán
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="sFKey"></param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <returns>
        ///     + string_html        : chuỗi html thể hiện nội dung của hoá đơn
        ///     + ERR:1     : Tài khoản đăng nhập sai hoặc không có quyền
        ///     + ERR:2     : Chuỗi token không chính xác
        ///     + ERR:3     : Dữ liệu xml đầu vào không đúng quy định
        ///     + ERR:4     : Công ty chưa được đăng kí mẫu hóa đơn nào
        ///     + ERR:5     : Không phát hành
        ///     + ERR:6     : Không tìm thấy hóa đơn tương ứng chuỗi đưa vào
        ///     + ERR:7     : Username không phù hợp - không tìm thấy company phù hợp với [Username]
        ///     + ERR:8     : Hóa đơn cần điều chỉnh đã bị thay thế. Không thể điều chỉnh được nữa.
        ///     + ERR:9     : Trạng thái hóa đơn không được điều chỉnh
        ///     + ERR:10    : Lô có số hóa đơn vượt quá max cho phép
        ///     + ERR:11    : Hóa đơn chưa cho deliver, ko xem được
        ///     + ERR:13    : Hóa đơn đã gạch nợ/ bỏ gạch nợ rồi
        ///     + ERR:14    : Lỗi NoFactory
        ///     + ERR:20    : Pattern và serial không phù hợp
        /// </returns>
        public string GetInvViewByFKeyNoPay(out string sErrMsg, string sFKey, string sInvServiceAccount,
            string sInvServiceAcPass)
        {
            try
            {
                sErrMsg = "";
                var resultOfService = _portalClient.getInvViewFkeyNoPay(sFKey, sInvServiceAccount, sInvServiceAcPass);
                if (_arrErrorService.ContainsKey("getInvViewFkeyNoPay") &&
                    _arrErrorService["getInvViewFkeyNoPay"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["getInvViewFkeyNoPay"]?[resultOfService];
                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        /// <summary>
        ///     Chuyển đổi hoá đơn sang html view của hoá đơn để hiển thị
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="sFKey"></param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <returns>
        ///     + string_html        : chuỗi html thể hiện nội dung của hoá đơn
        ///     + ERR:1     : Tài khoản đăng nhập sai hoặc không có quyền
        ///     + ERR:2     : Chuỗi token không chính xác
        ///     + ERR:3     : Dữ liệu xml đầu vào không đúng quy định
        ///     + ERR:4     : Công ty chưa được đăng kí mẫu hóa đơn nào
        ///     + ERR:5     : Không phát hành
        ///     + ERR:6     : Không tìm thấy hóa đơn tương ứng chuỗi đưa vào
        ///     + ERR:7     : Username không phù hợp - không tìm thấy company phù hợp với [Username]
        ///     + ERR:8     : Hóa đơn cần điều chỉnh đã bị thay thế. Không thể điều chỉnh được nữa.
        ///     + ERR:9     : Trạng thái hóa đơn không được điều chỉnh
        ///     + ERR:10    : Lô có số hóa đơn vượt quá max cho phép
        ///     + ERR:11    : Hóa đơn chưa cho deliver, ko xem được
        ///     + ERR:13    : Hóa đơn đã gạch nợ/ bỏ gạch nợ rồi
        ///     + ERR:14    : Lỗi NoFactory
        ///     + ERR:20    : Pattern và serial không phù hợp
        /// </returns>
        public string ConvertStoreInvViewByFKey(out string sErrMsg, string sFKey, string sInvServiceAccount,
            string sInvServiceAcPass)
        {
            try
            {
                sErrMsg = "";
                var resultOfService = _portalClient.convertForStoreFkey(sFKey, sInvServiceAccount, sInvServiceAcPass);
                if (_arrErrorService.ContainsKey("convertForStoreFkey") &&
                    _arrErrorService["convertForStoreFkey"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["convertForStoreFkey"]?[resultOfService];
                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        /// <summary>
        ///     Tải PDF của hoá đơn by FKey
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="sFKey"></param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <returns>
        ///     + string_html        : chuỗi html thể hiện nội dung của hoá đơn
        ///     + ERR:1     : Tài khoản đăng nhập sai hoặc không có quyền
        ///     + ERR:2     : Chuỗi token không chính xác
        ///     + ERR:3     : Dữ liệu xml đầu vào không đúng quy định
        ///     + ERR:4     : Công ty chưa được đăng kí mẫu hóa đơn nào
        ///     + ERR:5     : Không phát hành
        ///     + ERR:6     : Không tìm thấy hóa đơn tương ứng chuỗi đưa vào
        ///     + ERR:7     : Username không phù hợp - không tìm thấy company phù hợp với [Username]
        ///     + ERR:8     : Hóa đơn cần điều chỉnh đã bị thay thế. Không thể điều chỉnh được nữa.
        ///     + ERR:9     : Trạng thái hóa đơn không được điều chỉnh
        ///     + ERR:10    : Lô có số hóa đơn vượt quá max cho phép
        ///     + ERR:11    : Hóa đơn chưa cho deliver, ko xem được
        ///     + ERR:13    : Hóa đơn đã gạch nợ/ bỏ gạch nợ rồi
        ///     + ERR:14    : Lỗi NoFactory
        ///     + ERR:20    : Pattern và serial không phù hợp
        /// </returns>
        public string DownloadInvPDFFkey(out string sErrMsg, string sFKey, string sInvServiceAccount,
            string sInvServiceAcPass)
        {
            try
            {
                sErrMsg = "";
                var resultOfService = _portalClient.downloadInvPDFFkey(sFKey, sInvServiceAccount, sInvServiceAcPass);
                if (_arrErrorService.ContainsKey("downloadInvPDFFkey") &&
                    _arrErrorService["downloadInvPDFFkey"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["downloadInvPDFFkey"]?[resultOfService];
                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        /// <summary>
        ///     Tải PDF của hoá đơn by FKey
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="sFKey"></param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <returns>
        ///     + string_html        : chuỗi html thể hiện nội dung của hoá đơn
        ///     + ERR:1     : Tài khoản đăng nhập sai hoặc không có quyền
        ///     + ERR:2     : Chuỗi token không chính xác
        ///     + ERR:3     : Dữ liệu xml đầu vào không đúng quy định
        ///     + ERR:4     : Công ty chưa được đăng kí mẫu hóa đơn nào
        ///     + ERR:5     : Không phát hành
        ///     + ERR:6     : Không tìm thấy hóa đơn tương ứng chuỗi đưa vào
        ///     + ERR:7     : Username không phù hợp - không tìm thấy company phù hợp với [Username]
        ///     + ERR:8     : Hóa đơn cần điều chỉnh đã bị thay thế. Không thể điều chỉnh được nữa.
        ///     + ERR:9     : Trạng thái hóa đơn không được điều chỉnh
        ///     + ERR:10    : Lô có số hóa đơn vượt quá max cho phép
        ///     + ERR:11    : Hóa đơn chưa cho deliver, ko xem được
        ///     + ERR:13    : Hóa đơn đã gạch nợ/ bỏ gạch nợ rồi
        ///     + ERR:14    : Lỗi NoFactory
        ///     + ERR:20    : Pattern và serial không phù hợp
        /// </returns>
        public string DownloadInvPdfFkeyNoPay(out string sErrMsg, string sFKey, string sInvServiceAccount,
            string sInvServiceAcPass)
        {
            try
            {
                sErrMsg = "";
                var resultOfService =
                    _portalClient.downloadInvPDFFkeyNoPay(sFKey, sInvServiceAccount, sInvServiceAcPass);
                if (_arrErrorService.ContainsKey("downloadInvPDFFkey") &&
                    _arrErrorService["downloadInvPDFFkey"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["downloadInvPDFFkey"]?[resultOfService];
                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        /// <summary>
        ///     Chuyển đổi hoá đơn để lưu trữ bằng FKey
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="sFKey"></param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <returns>
        ///     + string_html        : chuỗi html thể hiện nội dung của hoá đơn
        ///     + ERR:1     : Tài khoản đăng nhập sai hoặc không có quyền
        ///     + ERR:2     : Chuỗi token không chính xác
        ///     + ERR:3     : Dữ liệu xml đầu vào không đúng quy định
        ///     + ERR:4     : Công ty chưa được đăng kí mẫu hóa đơn nào
        ///     + ERR:5     : Không phát hành
        ///     + ERR:6     : Không tìm thấy hóa đơn tương ứng chuỗi đưa vào
        ///     + ERR:7     : Username không phù hợp - không tìm thấy company phù hợp với [Username]
        ///     + ERR:8     : Hóa đơn cần điều chỉnh đã bị thay thế. Không thể điều chỉnh được nữa.
        ///     + ERR:9     : Trạng thái hóa đơn không được điều chỉnh
        ///     + ERR:10    : Lô có số hóa đơn vượt quá max cho phép
        ///     + ERR:11    : Hóa đơn chưa cho deliver, ko xem được
        ///     + ERR:13    : Hóa đơn đã gạch nợ/ bỏ gạch nợ rồi
        ///     + ERR:14    : Lỗi NoFactory
        ///     + ERR:20    : Pattern và serial không phù hợp
        /// </returns>
        public string ConvertForStoreFkey(out string sErrMsg, string sFKey, string sInvServiceAccount,
            string sInvServiceAcPass)
        {
            try
            {
                sErrMsg = "";
                var resultOfService = _portalClient.convertForStoreFkey(sFKey, sInvServiceAccount, sInvServiceAcPass);
                if (_arrErrorService.ContainsKey("convertForStoreFkey") &&
                    _arrErrorService["convertForStoreFkey"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["convertForStoreFkey"]?[resultOfService];
                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        /// <summary>
        ///     Tải XML của hoá đơn bằng FKey
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="sFKey"></param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <returns>
        ///     + string_html        : chuỗi html thể hiện nội dung của hoá đơn
        ///     + ERR:1     : Tài khoản đăng nhập sai hoặc không có quyền
        ///     + ERR:2     : Chuỗi token không chính xác
        ///     + ERR:3     : Dữ liệu xml đầu vào không đúng quy định
        ///     + ERR:4     : Công ty chưa được đăng kí mẫu hóa đơn nào
        ///     + ERR:5     : Không phát hành
        ///     + ERR:6     : Không tìm thấy hóa đơn tương ứng chuỗi đưa vào
        ///     + ERR:7     : Username không phù hợp - không tìm thấy company phù hợp với [Username]
        ///     + ERR:8     : Hóa đơn cần điều chỉnh đã bị thay thế. Không thể điều chỉnh được nữa.
        ///     + ERR:9     : Trạng thái hóa đơn không được điều chỉnh
        ///     + ERR:10    : Lô có số hóa đơn vượt quá max cho phép
        ///     + ERR:11    : Hóa đơn chưa cho deliver, ko xem được
        ///     + ERR:13    : Hóa đơn đã gạch nợ/ bỏ gạch nợ rồi
        ///     + ERR:14    : Lỗi NoFactory
        ///     + ERR:20    : Pattern và serial không phù hợp
        /// </returns>
        public string DownloadInvXMLFkey(out string sErrMsg, string sFKey, string sInvServiceAccount,
            string sInvServiceAcPass)
        {
            try
            {
                sErrMsg = "";
                var resultOfService = _portalClient.downloadInvFkey(sFKey, sInvServiceAccount, sInvServiceAcPass);
                if (_arrErrorService.ContainsKey("downloadInvFkey") &&
                    _arrErrorService["downloadInvFkey"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["downloadInvFkey"]?[resultOfService];
                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        public string GetInvByFKeyOnDate(out string sErrMsg, string sFKey, DateTime fromDate, DateTime toDate, string sInvServiceAccount, string sInvServiceAcPass)
        {
            try
            {
                sErrMsg = "";
                var resultOfService = _portalClient.listInvByCusFkey(sFKey, fromDate.ToString("dd/MM/yyyy"),
                    toDate.ToString("dd/MM/yyyy"), sInvServiceAccount, sInvServiceAcPass);
                if (_arrErrorService.ContainsKey("listInvByCusFkey") &&
                    _arrErrorService["listInvByCusFkey"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["listInvByCusFkey"]?[resultOfService];
                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        /// <summary>
        ///     Lấy thông tin Serial thuộc Pattern
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="sInvServiceAccount"></param>
        /// <param name="sInvServiceAcPass"></param>
        /// <param name="sPattern"></param>
        /// <returns>string dạng [1-DT/21E-200000-1-200000-1714-198286-02/06/2021-2]</returns>
        public string GetSerialByPattern(out string sErrMsg, string sInvServiceAccount, string sInvServiceAcPass,
            string sPattern)
        {
            try
            {
                sErrMsg = "";
                var dataSerialByPattern =
                    _businessClient.getSerialByPattern(sInvServiceAccount, sInvServiceAcPass, sPattern);
                if (_arrErrorService.ContainsKey("getSerialByPattern") &&
                    _arrErrorService["getSerialByPattern"].ContainsKey(dataSerialByPattern))
                    sErrMsg = _arrErrorService["getSerialByPattern"]?[dataSerialByPattern];
                return dataSerialByPattern;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        #endregion

        #region Customer E-Invoice Functions

        /// <summary>
        ///     Thêm mới hoặc cập nhật thông tin danh sách khách hàng
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="sXmlCusData">
        ///     - Thông tin danh sách các khách hàng
        ///     <Customers>
        ///         <Customer>
        ///             <Name>Trung tâm Công nghệ Thông tin</Name>
        ///             <Code>DNA0100700128</Code>
        ///             <TaxCode>DNA1000700031</TaxCode>
        ///             <Address>4 Lê Lợi - P.Xương Huân - TP.Nha Trang - T.Khánh Hòa</Address>
        ///             <BankAccountName />
        ///             <BankName />
        ///             <BankNumber />
        ///             <Email>cenit @gmail.com</Email>
        ///             <Fax />
        ///             <Phone />
        ///             <ContactPerson />
        ///             <RepresentPerson />
        ///             <CusType>1</CusType>
        ///         </Customer>
        ///     </Customers>
        /// </param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <returns>
        ///     -1: Tài khoản đăng nhập sai hoặc không có quyền thêm khách hàng
        ///     -2: Không import được khách hàng vào db (Lỗi cập nhật khách hàng lẻ vào DB)
        ///     -3: Dữ liệu xml đầu vào không đúng quy định; trường CusType không hợp lệ
        ///     -5: User đã tồn tại rồi
        ///     N: Số lượng khách hàng đã import và update
        /// </returns>
        public int SaveCustomers(out string sErrMsg, string sXmlCusData, string sInvServiceAccount,
            string sInvServiceAcPass)
        {
            try
            {
                sErrMsg = string.Empty;
                var resultOfService = _publishClient.UpdateCus(sXmlCusData, sInvServiceAccount, sInvServiceAcPass, 0);
                if (_arrErrorService.ContainsKey("UpdateCus") &&
                    _arrErrorService["UpdateCus"].ContainsKey(resultOfService.ToString()))
                    sErrMsg = _arrErrorService["UpdateCus"]?[resultOfService.ToString()];
                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        /// <summary>
        ///     Lấy thông tin khách hàng bằng mã khách hàng
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="cusCode">Mã khách hàng</param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <returns>
        ///     Dữ liệu trả về dạng chuỗi xml có định dạng:
        ///     <Data>
        ///         <code>DNA0100700128</code>
        ///         <name>
        ///             <![CDATA[Trung tâm Công nghệ Thông tin]]>
        ///         </name>
        ///         <address>
        ///             <![CDATA[4 Lê Lợi - P.Xương Huân - TP.Nha Trang - T.Khánh Hòa]]>
        ///         </address>
        ///         <phone></phone>
        ///         <taxcode>DNA1000700031</taxcode>
        ///         <email>cenit @gmail.com</email>
        ///     </Data>
        /// </returns>
        public string GetCustomerByCusCode(out string sErrMsg, string cusCode, string sInvServiceAccount,
            string sInvServiceAcPass)
        {
            try
            {
                sErrMsg = "";
                var resultOfService = _portalClient.getCus(cusCode, sInvServiceAccount, sInvServiceAcPass);
                if (_arrErrorService.ContainsKey("getCus") && _arrErrorService["getCus"].ContainsKey(resultOfService))
                    sErrMsg = _arrErrorService["getCus"]?[resultOfService];
                return resultOfService;
            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
                AppProcessor.Notifider.Broadcast(
                    "Lỗi hệ thống hoá đơn điện tử", -1);
                throw;
            }
        }

        #endregion
    }
}