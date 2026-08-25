using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Core.Inv.Caches;
using Core.Inv.Enums;
using Core.Inv.Logs;
using Core.Inv.Models;
using Core.Inv.Models.Invs;
using FastMember;
using TSFramework.App.Processors;
using TSFramework.Core.Helpers;

namespace Core.Inv.Providers
{
    public class InvProvider
    {
        private readonly MajorInvCache _invCache;
        private readonly MajorInvActionLogCache _logCache;
        private readonly InvServiceProvider _serviceProvider;

        public InvProvider(string urlPortalService, string urlBusinessService, string urlPublishService)
        {
            _serviceProvider = new InvServiceProvider(urlPortalService, urlBusinessService, urlPublishService);
            _logCache = new MajorInvActionLogCache();
            _invCache = new MajorInvCache();
        }

        public bool IsOnline()
        {
            return _serviceProvider?.IsOnline() ?? false;
        }

        public bool IsCorrectUser(string sInvServiceAccount, string sInvServiceAcPass)
        {
            return _serviceProvider?.IsCorrectUser(sInvServiceAccount, sInvServiceAcPass) ?? false;
        }

        #region Publish Invoice

        /// <summary>
        ///     Thêm mới hoá đơn điện tử khách hàng
        ///     [*] Quy trình thêm mới hoá đơn
        ///     [1]. Kiểm tra khách hàng đã tồn tại hay chưa
        ///     [1]. Kiểm tra khách hàng đã tồn tại hay chưa
        ///     [1.1] Nếu chưa tồn tại thì thêm mới khách hàng và chuyển tới bước [2]
        ///     [1.2] Nếu tồn tại thì cập nhật lại thông tin khách hàng và chuyển tới bước [2]
        ///     [2]. Thêm mới và phát hành hoá đơn cho khách hàng và chuyển sang bước [3]:
        ///     - Hoá đơn phát hành có trạng thái là chưa thanh toán
        ///     - Hoá đơn phát hành có thể có 1 hoặc nhiều sản phẩm (product)
        ///     [3]. Trường hợp hoá đơn đã được thanh toán thì thực hiện xác nhận thanh toán cho hoá đơn đó.
        /// </summary>
        /// <param name="invInfo"></param>
        /// <param name="sInvAccName"></param>
        /// <param name="sInvAccPass"></param>
        /// <param name="sInvServiceAccName"></param>
        /// <param name="sInvServiceAccPass"></param>
        /// <param name="sInvPattern"></param>
        /// <param name="sInvSerial"></param>
        /// <param name="sSysAccount"></param>
        /// <param name="sReason"></param>
        /// <param name="sErrMsg"></param>
        /// <param name="cusInfo"></param>
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
        public void CreateInvoice(out string sErrMsg, InvCustomerModel cusInfo, InvInv invInfo, string sInvAccName,
            string sInvAccPass, string sInvServiceAccName, string sInvServiceAccPass, string sInvPattern,
            string sInvSerial, string sSysAccount = "", string sReason = "")
        {
            sErrMsg = string.Empty;

            var invIsSuccess = false;
            var cusIsSuccess = false;
            string resultInvImpoted;
            var sLogContents = new StringBuilder();
            var lstActionLogs = new List<MajorInvActionLogModel>();

            #region Update Customer

            var sActionCode = Enum.GetName(typeof(EnumInvActionType), EnumInvActionType.UpdateCustomerInfo);

            sLogContents.AppendLine();
            sLogContents.AppendLine("============================================================");
            sLogContents.AppendLine($"1. Cập nhật thông tin/Tạo mới khách hàng [{cusInfo.Code} - {cusInfo.Name}]");
            sLogContents.AppendLine("1.1. Tạo xml thông tin khách hàng");

            //Tạo xmlCusData thông tin khách hàng

            var eInvCustomers = new InvCustomers
            {
                ListCustomers = new List<InvCustomerModel> { cusInfo }
            };
            var sXmlCusDatas = XmlHelper.SerializeToString(eInvCustomers);

            lstActionLogs.Add(new MajorInvActionLogModel
            {
                InvAccount = sInvAccName,
                SysAccount = sSysAccount,
                ActionType = nameof(EnumInvActionType.UpdateCustomerInfo),
                Contents = sXmlCusDatas,
                Reason = sReason
            });

            sLogContents.AppendLine("1.2. Gọi service cập nhật thông tin/tạo mới khách hàng");
            var amountCusUpdated =
                _serviceProvider.SaveCustomers(out sErrMsg, sXmlCusDatas, sInvServiceAccName, sInvServiceAccPass);
            sLogContents.AppendLine($"1.3. Kết quả trả về: {amountCusUpdated}");
            sLogContents.AppendLine("----------------------------------------");

            #endregion

            #region Xử lý kết quả trả về

            #region Customer - Lỗi

            if (amountCusUpdated < 0)
            {
                resultInvImpoted = $"ERR:{amountCusUpdated}";
                switch (amountCusUpdated)
                {
                    case -1:
                        sLogContents.AppendLine(
                            "[1] - Lỗi[-1]: Tài khoản đăng nhập sai hoặc không có quyền thêm khách hàng");
                        break;
                    case -2:
                        sLogContents.AppendLine(
                            "[1] - Lỗi[-2]: Không import được khách hàng vào db (Lỗi cập nhật khách hàng lẻ vào DB)");
                        break;
                    case -3:
                        sLogContents.AppendLine(
                            "[1] - Lỗi[-3]: Dữ liệu xml đầu vào không đúng quy định; trường CusType không hợp lệ");
                        break;
                    case -5:
                        sLogContents.AppendLine("[1] - Lỗi[-5]: User đã tồn tại rồi");
                        break;
                }

                sErrMsg = "Lỗi lưu khách hàng";
                sLogContents.AppendLine("----------------------------------------");
            }

            #endregion

            #region Customer - Thành công

            else
            {
                cusIsSuccess = true;

                #region Publish Invoice

                sActionCode = Enum.GetName(typeof(EnumInvActionType), EnumInvActionType.ImportAndPublishInvoice);
                sLogContents.AppendLine(
                    $"2. Tạo hoá đơn [{invInfo.FKey}] cho khách hàng [{cusInfo.Code} - {cusInfo.Name}]");
                sLogContents.AppendLine("2.1. Tạo dữ liệu xml hoá đơn");

                #region Tạo chuỗi xml dữ liệu hoá đơn

                var eInvInvoicesModel = new InvInvoices
                {
                    ListInvs = new List<InvInv> { invInfo }
                };

                var sXmlInvInvoicesData = XmlHelper.SerializeToString(eInvInvoicesModel);

                #endregion

                sLogContents.AppendLine("2.2. Gọi service phát hành hoá đơn");

                #region Gọi service thêm mới & phát hành hoá đơn và xử lý kết quả trả về

                resultInvImpoted = _serviceProvider.ImportAndPublishInvoice(out sErrMsg, sXmlInvInvoicesData,
                    sInvAccName,
                    sInvAccPass, sInvServiceAccName, sInvServiceAccPass, sInvPattern, sInvSerial);

                sLogContents.AppendLine($"2.3. Kết quả trả về: [{resultInvImpoted}]");
                sLogContents.AppendLine("----------------------------------------");

                lstActionLogs.Add(new MajorInvActionLogModel
                {
                    InvAccount = sInvAccName,
                    SysAccount = sSysAccount,
                    ActionType = nameof(EnumInvActionType.ImportAndPublishInvoice),
                    Contents = sXmlInvInvoicesData,
                    Reason = sReason
                });

                #region Invoice - Success

                if (resultInvImpoted.Contains("OK:"))
                {
                    sLogContents.AppendLine($" - Phát hành hoá đơn [{invInfo.FKey}] thành công");

                    invIsSuccess = true;
                    var mapFKeyInvNo = new Dictionary<string, string>();
                    if (!string.IsNullOrEmpty(resultInvImpoted) && resultInvImpoted.Contains("OK:"))
                    {
                        var lstValues = GetValueFromStringBaseOnTemplate("(.*?):(.*?);(.*?)-(.*?)", resultInvImpoted);

                        if (lstValues.Count == 4)
                        {
                            var sInvFKeyWithInvNos = lstValues[3]; //3B9F6FB7C723AA0C_451,BE5FA28AC683AA0C_452

                            #region Xử lý nhiều hoá đơn điện tử

                            var sInvFKeyWithInvNo = sInvFKeyWithInvNos.Split(',');

                            sInvFKeyWithInvNo.ToList().ForEach(s =>
                            {
                                var dataFKey = GetValueFromStringBaseOnTemplate("(.*?)_(.*?)", s);
                                if (dataFKey.Count == 2) mapFKeyInvNo.Add(dataFKey[0], dataFKey[1]);
                            });

                            #endregion

                            sLogContents.AppendLine(
                                $"2.4. Cập nhật thông tin hoá đơn [{int.Parse(mapFKeyInvNo[invInfo.FKey])}-{invInfo.FKey}] vào db");

                            #region Lưu thông tin hoá đơn đã tạo vào hệ thống

                            /*
                             * Inv Status
                             */

                            var retUpdateInv = _invCache.Update(new InvStatusModel
                            {
                                InvKey = invInfo.FKey,
                                InvNo = int.Parse(mapFKeyInvNo[invInfo.FKey]).ToString().TrimStart('0'),
                                InvStatus = (int)EnumInvStatus.InvoiceHasSignature,
                                InvStatusName = EnumHelper.GetDescription(EnumInvStatus.InvoiceHasSignature),
                                PublishBy = sInvAccName,
                                PublishOn = DateTime.Now,
                                ConfirmPaidBy = sInvAccName,
                                PaidOn = DateTime.Now,
                                Reason = sReason,
                                SavedBy = sSysAccount,
                                ErrCode = null
                            });
                            if (retUpdateInv <= 0) sLogContents.AppendLine($" - Lỗi: {retUpdateInv}");

                            #endregion
                        }
                    }
                }

                #endregion

                #endregion

                #endregion
            }

            #endregion

            #region Lỗi

            if (!cusIsSuccess || !invIsSuccess)
            {
                sLogContents.AppendLine($" - Phát hành hoá đơn [{invInfo.FKey}] thất bại: {sErrMsg}");
                sLogContents.AppendLine("----------------------------------------");

                /*
                 * Inv Status
                 */

                var retUpdateInv = _invCache.Update(new InvStatusModel
                {
                    InvKey = invInfo.FKey,
                    InvNo = null,
                    InvStatus = (int)EnumInvStatus.InvoiceJustCreated,
                    InvStatusName = EnumHelper.GetDescription(EnumInvStatus.InvoiceJustCreated),
                    PublishBy = null,
                    PublishOn = null,
                    ConfirmPaidBy = null,
                    PaidOn = null,
                    Reason = sReason,
                    SavedBy = sSysAccount,
                    ErrCode = resultInvImpoted
                });

                if (retUpdateInv <= 0) AppProcessor.Logger.Message("Cập nhật trạng thái hoá đơn đã phát hành lỗi");
            }

            #endregion

            #endregion

            sLogContents.AppendLine("============================================================");
            sLogContents.AppendLine("- Lưu log thao tác hoá đơn");

            var dataActionLogs = new DataTable();

            using (var reader = ObjectReader.Create(lstActionLogs, "SysAccount", "InvAccount", "ActionType", "Contents",
                       "Reason"))
            {
                dataActionLogs.Load(reader);
            }

            var retSaveLogs = _logCache.Save(dataActionLogs);
            if (retSaveLogs <= 0) sLogContents.AppendLine($" - Lỗi: {retSaveLogs}");

            InvLogger.LogAction(sInvAccName, sActionCode, sLogContents.ToString());
        }

        #endregion

        #region Cancel Invoice

        /// <summary>
        ///     Huỷ hoá đơn
        /// </summary>
        /// <param name="sInvKey">FKey hoá đơn cần huỷ</param>
        /// <param name="sInvAccName">Tài khoản nhân viên được cấp quyền tạo hoá đơn</param>
        /// <param name="sInvAccPass">Mật khẩu tài khoản nhân viên được cấp quyền tạo hoá đơn</param>
        /// <param name="sInvServiceAccName">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAccPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <param name="sSysAccount"></param>
        /// <param name="sReason"></param>
        /// <returns>
        ///     true - Thành công
        ///     false - Thất bại
        /// </returns>
        public bool CancelInv(string sInvKey, string sInvAccName, string sInvAccPass, string sInvServiceAccName,
            string sInvServiceAccPass, string sSysAccount = "", string sReason = "")
        {
            var sLogContents = new StringBuilder();
            var sActionCode = Enum.GetName(typeof(EnumInvActionType), EnumInvActionType.CancelInvoice);
            var lstActionLogs = new List<MajorInvActionLogModel>
            {
                new MajorInvActionLogModel
                {
                    InvAccount = sInvAccName,
                    SysAccount = sSysAccount,
                    ActionType = nameof(EnumInvActionType.CancelInvoice),
                    Contents = sInvKey,
                    Reason = sReason
                }
            };

            //Lưu Log thao tác đối với hoá đơn điện tử
            sLogContents.AppendLine();
            sLogContents.AppendLine("============================================================");
            sLogContents.AppendLine($"* Bắt đầu huỷ hoá đơn với FKey [{sInvKey}]");
            sLogContents.AppendLine("1. Gọi service thực hiện huỷ hoá đơn");

            var resultServiceProcess = _serviceProvider.CancelInvoice(out var sErrMsg, sInvAccName, sInvAccPass,
                sInvKey,
                sInvServiceAccName, sInvServiceAccPass);

            #region Xử lý lỗi trả về

            if (resultServiceProcess.Contains("ERR:"))
            {
                //Xử lý lưu log lỗi xử lý khi
                sLogContents.AppendLine(
                    $"- Thực hiện huỷ hoá đơn thất bại. Lỗi = [{resultServiceProcess}-{sErrMsg}]");
                return false;
            }

            if (resultServiceProcess.Contains("OK"))
                sLogContents.AppendLine($"- Xử lý huỷ hoá đơn[FKey = {sInvKey}] thành công");

            sLogContents.AppendLine("Kết thúc xử lý hoá đơn");

            var dataActionLogs = new DataTable();

            using (var reader = ObjectReader.Create(lstActionLogs, "SysAccount", "InvAccount", "ActionType", "Contents",
                       "Reason"))
            {
                dataActionLogs.Load(reader);
            }

            var retSaveLogs = _logCache.Save(dataActionLogs);
            if (retSaveLogs <= 0) sLogContents.AppendLine($" - Lỗi: {retSaveLogs}");

            InvLogger.LogAction(sInvAccName, sActionCode, sLogContents.ToString());

            #endregion

            return true;
        }

        #endregion

        #region Other Function

        /// <summary>
        ///     Lấy giá trị từ chuỗi dựa vào mẫu
        /// </summary>
        /// <param name="sTemplate"></param>
        /// <param name="sContents"></param>
        /// <returns></returns>
        public List<string> GetValueFromStringBaseOnTemplate(string sTemplate, string sContents)
        {
            var pattern = "^" + Regex.Replace(sTemplate, @"\{[0-9]+\}", "(.*?)") + "$";

            var r = new Regex(pattern);
            var m = r.Match(sContents);

            var ret = new List<string>();

            for (var i = 1; i < m.Groups.Count; i++) ret.Add(m.Groups[i].Value);

            return ret;
        }

        #endregion

        #region Download PDF

        /// <summary>
        ///     Download hoa don pdf
        /// </summary>
        /// <param name="sFKey"></param>
        /// <param name="sInvServiceAccount"></param>
        /// <param name="sInvServiceAcPass"></param>
        /// <returns></returns>
        public string DownloadPDF(string sFKey, string sInvServiceAccount, string sInvServiceAcPass)
        {
            var resultServiceProcess =
                _serviceProvider.DownloadInvPdfFkeyNoPay(out _, sFKey, sInvServiceAccount, sInvServiceAcPass);
            return resultServiceProcess;
        }

        #endregion

        #region Adjust Invoice

        /// <summary>
        ///     Điều chỉnh hoá đơn.
        ///     ==========================================
        ///     Có 3 loại điều chỉnh hoá đơn:
        ///     + 2 - Điều chỉnh tăng
        ///     + 3 - Điều chỉnh giảm
        ///     + 4 - Hóa đơn điều chỉnh thông tin
        /// </summary>
        /// <param name="eInvAdjust"></param>
        /// <param name="type"></param>
        /// <param name="sInvAccPass"></param>
        /// <param name="sInvServiceAccount">Tài khoản dùng để gọi service</param>
        /// <param name="sInvServiceAcPass">Mật khẩu tài khoản dùng để gọi service</param>
        /// <param name="sFKey">Chuỗi xác định hoá đơn cần điều chỉnh</param>
        /// <param name="sInvPattern"></param>
        /// <param name="sInvSerial"></param>
        /// <param name="sSysAccount"></param>
        /// <param name="sReason"></param>
        /// <param name="sInvAccName"></param>
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
        public string AdjustInvoice(InvAdjustInv eInvAdjust, int type, string sInvAccName, string sInvAccPass,
            string sInvServiceAccount, string sInvServiceAcPass, string sFKey, string sInvPattern,
            string sInvSerial, string sSysAccount = "", string sReason = "")
        {
            var sLogContents = new StringBuilder();
            var sActionCode = Enum.GetName(typeof(EnumInvActionType), EnumInvActionType.AdjustInvoice);
            var lstActionLogs = new List<MajorInvActionLogModel>();

            sLogContents.AppendLine("1. Tạo xml thông tin hiệu chỉnh");
            //Tạo sXmlInvData
            var sXmlInvData = XmlHelper.SerializeToString(eInvAdjust);

            lstActionLogs.Add(new MajorInvActionLogModel
            {
                InvAccount = sInvAccName,
                SysAccount = sSysAccount,
                ActionType = nameof(EnumInvActionType.UpdateCustomerInfo),
                Contents = sXmlInvData,
                Reason = sReason
            });

            sLogContents.AppendLine("2. Bắt đầu hiệu chỉnh hóa đơn");

            var result = _serviceProvider.AdjustInvoice(out _, sInvAccName, sInvAccPass, sXmlInvData,
                sInvServiceAccount, sInvServiceAcPass, sFKey, sInvPattern, sInvSerial);

            //Xử lý lưu log lỗi xử lý khi
            sLogContents.AppendLine(result.Contains("ERR:")
                ? $"- Thực hiện hiệu chỉnh hoá đơn thất bại. Lỗi = [{result}]"
                : $"- Xử lý hiệu chỉnh hoá đơn[FKey = {sFKey}] thành công");

            sLogContents.AppendLine("3. Kết thúc xử lý hoá đơn");
            sLogContents.AppendLine("============================================================");
            sLogContents.AppendLine("- Lưu log thao tác hoá đơn");

            var dataActionLogs = new DataTable();

            using (var reader = ObjectReader.Create(lstActionLogs, "SysAccount", "InvAccount", "ActionType", "Contents",
                       "Reason"))
            {
                dataActionLogs.Load(reader);
            }

            var retSaveLogs = _logCache.Save(dataActionLogs);
            if (retSaveLogs <= 0) sLogContents.AppendLine($" - Lỗi: {retSaveLogs}");

            InvLogger.LogAction(sInvAccName, sActionCode, sLogContents.ToString());

            return result;
        }

        #endregion

        #region Sync Invoice

        public int SyncInvoice(out string sErrMsg, string invKey, DateTime dateCreated, string sInvServiceAccName,
            string sInvServiceAccPass, string sReason, string syncBy)
        {
            var sDataInvs = _serviceProvider.GetInvByFKeyOnDate(out sErrMsg, invKey, dateCreated, dateCreated,
                sInvServiceAccName, sInvServiceAccPass);

            if (!string.IsNullOrEmpty(sErrMsg) || sDataInvs.Contains("ERR:")) return -1;

            DataInvoices lstDataInvs = null;
            if (!string.IsNullOrEmpty(sDataInvs))
                lstDataInvs = XmlHelper.DeserializeXmlToClass<DataInvoices>(sDataInvs);

            if (lstDataInvs == null || lstDataInvs.InvoiceItems.Count <= 0) return 0;
            var invInfo = _invCache.GetByKey(invKey);

            foreach (var dataInv in lstDataInvs.InvoiceItems)
            {
                var eInvModel = new InvStatusModel
                {
                    InvKey = invInfo.InvKey,
                    InvNo = dataInv.InvNum,
                    InvStatus = int.Parse(dataInv.Status),
                    InvStatusName = EnumHelper.GetDescription((EnumInvStatus)int.Parse(dataInv.Status)),
                    PublishOn = DateTime.ParseExact(dataInv.PublishDate, "M/d/yyyy h:mm:ss tt",
                        CultureInfo.InvariantCulture),
                    Reason = sReason,
                    SavedBy = syncBy,
                    ErrCode = null
                };

                var invId = _invCache.Sync(eInvModel);
                if (invId < 0) return -1;
            }

            return 1;
        }

        #endregion

        #region Get Info

        /// <summary>
        ///     Lấy nội dung hoá đơn để hiển thị, không cần thanh toán
        /// </summary>
        /// <param name="sFKey"></param>
        /// <param name="sInvServiceAccount"></param>
        /// <param name="sInvServiceAcPass"></param>
        /// <returns></returns>
        public string GetInvViewNoPay(string sFKey, string sInvServiceAccount, string sInvServiceAcPass)
        {
            var resultServiceProcess =
                _serviceProvider.GetInvViewByFKeyNoPay(out _, sFKey, sInvServiceAccount, sInvServiceAcPass);
            return resultServiceProcess;
        }

        /// <summary>
        ///     Lấy thông tin serial của partter
        /// </summary>
        /// <param name="sErrMsg"></param>
        /// <param name="sInvServiceAccount"></param>
        /// <param name="sInvServiceAcPass"></param>
        /// <param name="sPattern"></param>
        /// s
        /// <returns>string dạng: [1-DT/21E-1000-1-1000-37-963-21/05/2021-2;2-DT/22E-1000000-1-1000000-0-1000000-10/01/2022-1]</returns>
        public string GetSerialByPattern(out string sErrMsg, string sInvServiceAccount, string sInvServiceAcPass,
            string sPattern)
        {
            var resultServiceProcess =
                _serviceProvider.GetSerialByPattern(out sErrMsg, sInvServiceAccount, sInvServiceAcPass, sPattern);
            return resultServiceProcess;
        }

        #endregion

        #region Update Customer

        //public void UpdateCustomer(out string sErrMsg, InvCustomerModel cusInfo, string sInvAccName, string sInvAccPass, string sInvServiceAccName, string sInvServiceAccPass, string sSysAccount = "", string sReason = "")
        //{
        //    var sLogContents = new StringBuilder();
        //    var lstActionLogs = new List<MajorInvActionLogModel>();

        //    var sActionCode = Enum.GetName(typeof(EnumInvActionType), EnumInvActionType.UpdateCustomerInfo);

        //    sLogContents.AppendLine();
        //    sLogContents.AppendLine("============================================================");
        //    sLogContents.AppendLine($"1. Cập nhật thông tin/Tạo mới khách hàng [{cusInfo.Code} - {cusInfo.Name}]");
        //    sLogContents.AppendLine("1.1. Tạo xml thông tin khách hàng");

        //    //Tạo xmlCusData thông tin khách hàng

        //    var eInvCustomers = new InvCustomers
        //    {
        //        ListCustomers = new List<InvCustomerModel> { cusInfo }
        //    };
        //    var sXmlCusDatas = XmlHelper.SerializeToString(eInvCustomers);

        //    lstActionLogs.Add(new MajorInvActionLogModel
        //    {
        //        InvAccount = sInvAccName,
        //        SysAccount = sSysAccount,
        //        ActionType = EnumInvActionType.UpdateCustomerInfo.ToString(),
        //        Contents = sXmlCusDatas,
        //        Reason = sReason
        //    });

        //    sLogContents.AppendLine("1.2. Gọi service cập nhật thông tin/tạo mới khách hàng");
        //    var amountCusUpdated = _serviceProvider.SaveCustomers(out sErrMsg, sXmlCusDatas, sInvServiceAccName, sInvServiceAccPass);
        //    sLogContents.AppendLine($"1.3. Kết quả trả về: {amountCusUpdated}");
        //    sLogContents.AppendLine("----------------------------------------");
        //}

        #endregion
    }
}