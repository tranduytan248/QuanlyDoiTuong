using Cores.eContract.Models;
using Cores.eContract.Models.Request;
using Cores.eContract.Models.Response;
using Cores.Sys.Caches.Sys;
using System.Net;
using System.Web;

namespace Modules.eContract.Providers
{
    public static class EContractProvider
    {
        const string CONFIG_KEY_ECONTRACT_HOST_API = "CONFIG_KEY_ECONTRACT_HOST_API";
        const string CONFIG_KEY_ECONTRACT_CLIENT_ID = "CONFIG_KEY_ECONTRACT_CLIENT_ID";
        const string CONFIG_KEY_ECONTRACT_CLIENT_SECRET = "CONFIG_KEY_ECONTRACT_CLIENT_SECRET";
        const string CONFIG_KEY_ECONTRACT_ACCOUNT_USERNAME = "CONFIG_KEY_ECONTRACT_ACCOUNT_USERNAME";
        const string CONFIG_KEY_ECONTRACT_ACCOUNT_PASSWORD = "CONFIG_KEY_ECONTRACT_ACCOUNT_PASSWORD";
        const string CONFIG_KEY_ECONTRACT_DOMAIN = "CONFIG_KEY_ECONTRACT_DOMAIN";

        static readonly SysConfigCache sysConfigCache = new SysConfigCache();

        //private static readonly string eContractHost = sysConfigCache.GetViaKey(CONFIG_KEY_ECONTRACT_HOST_API)?.ConfigValue;
        //private static readonly string eContractClientId = sysConfigCache.GetViaKey(CONFIG_KEY_ECONTRACT_CLIENT_ID)?.ConfigValue;
        //private static readonly string eContractSecret = sysConfigCache.GetViaKey(CONFIG_KEY_ECONTRACT_CLIENT_SECRET)?.ConfigValue;
        private static readonly string eContractUserName = sysConfigCache.GetViaKey(CONFIG_KEY_ECONTRACT_ACCOUNT_USERNAME)?.ConfigValue;
        private static readonly string eContractPassword = sysConfigCache.GetViaKey(CONFIG_KEY_ECONTRACT_ACCOUNT_PASSWORD)?.ConfigValue;
        private static readonly string eContractDomain = sysConfigCache.GetViaKey(CONFIG_KEY_ECONTRACT_DOMAIN)?.ConfigValue;

        /// <summary>
        /// Dùng để test
        /// </summary>
        private static readonly string eContractHost = "https://apigateway-econtract-poc.vnptit3.vn";
        private static readonly string eContractClientId = "4201642981.client@econtract.vnpt.vn";
        private static readonly string eContractSecret = "cN2juxPy6g0pNnXFzmOj2hYDHQ7xBnfX";

        private static string _authToken = "";

        /// <summary>
        /// 3. Lấy Danh sách mẫu hợp đồng 
        /// </summary>
        /// <param name="reqModel">Thông tin tìm kiếm + phân trang</param>
        /// <param name="errMsg"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<ResTemplateContractModel> GetContractTemplates(ReqSearchModel reqModel, out string errMsg)
        {
            int countAuth = 0;

        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel { ClientId = eContractClientId, ClientSecret = eContractSecret }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }

                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.GetContractTemplates(eContractHost, _authToken, reqModel,
                out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;

            return resService;
        }
        /// <summary>
        /// 4. Thông tin chi tiết hợp đồng mẫu để tạo hợp đồng 
        /// </summary>
        /// <param name="templateId">Id hợp đồng mẫu </param>
        /// <param name="errMsg"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<ResDetailTemplateContractModel> GetDetailTemplateContract(string templateId, out string errMsg)
        {
            int countAuth = 0;

        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel { ClientId = eContractClientId, ClientSecret = eContractSecret }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.GetDetailTemplateContract(eContractHost, _authToken, templateId,
                out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }
        /// <summary>
        /// 5. Render hợp đồng từ mẫu hợp đồng
        /// </summary>
        /// <param name="templateId">Id hợp đồng mẫu </param>
        /// <param name="dataFields">Dữ liệu hợp đồng dạng key=>value </param>
        /// <param name="errMsg"></param>
        /// <returns>byte[]</returns>
        public static byte[] RenderContract(string templateId, string dataFields, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel { ClientId = eContractClientId, ClientSecret = eContractSecret }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.RenderContract(eContractHost, _authToken, templateId, dataFields,
                out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }
        /// <summary>
        /// 6. Lấy danh sách tọa độ các biến vị trí trong file PDF: tìm danh sách các chữ @{1}, 
        /// </summary>
        /// <param name="attachFile">File pdf tài liệu cần lấy thông tin </param>
        /// <param name="errMsg"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<ResListPositionSignatureModel> GetListPosition(HttpPostedFileBase attachFile, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthUserModel { UserName = eContractUserName, Password = eContractPassword, Domain = eContractDomain }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.GetListPosition(eContractHost, _authToken, attachFile, out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }
        /// <summary>
        /// 7. Danh sách luồng hợp đồng
        /// </summary>
        /// <param name="reqModel">Params request</param>
        /// <param name="errMsg"></param>
        /// <returns>ResListFlowContractModel</returns>
        public static BaseResponseModel<ResListFlowContractModel> GetListFlowContract(ReqListFlowContractModel reqModel, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthUserModel { UserName = eContractUserName, Password = eContractPassword, Domain = eContractDomain }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.GetListFlowContract(eContractHost, _authToken, reqModel, out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }
        /// <summary>
        /// 8. Chi tiết luồng hợp đồng
        /// </summary>
        /// <param name="contractFlowTemplateId">Id luồng hợp đồng lấy từ API GetListFlowContract </param>
        /// <param name="errMsg"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<FlowContractModel> GetDetailFlowContact(string contractFlowTemplateId, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthUserModel { UserName = eContractUserName, Password = eContractPassword, Domain = eContractDomain }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.GetDetailFlowContact(eContractHost, _authToken, contractFlowTemplateId, out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }
        /// <summary>
        /// 9. Tạo hợp đồng từ file pdf hợp đồng
        /// </summary>
        /// <param name="contractModel"></param>
        /// <param name="errMsg"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> CreateContract(ReqContractModel contractModel, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel { ClientId = eContractClientId, ClientSecret = eContractSecret }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.CreateContract(eContractHost, _authToken, contractModel, out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }
        /// <summary>
        /// 10. Gửi hợp đồng cho nội bộ hoặc đối tác theo luồng HĐ đã cấu hình. 
        /// </summary>
        /// <param name="contractId">Id hơp đồng</param>
        /// <param name="errMsg"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> SubmitContract(string contractId, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel { ClientId = eContractClientId, ClientSecret = eContractSecret }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.SubmitContract(eContractHost, _authToken, contractId, out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }
        /// <summary>
        /// 11. Upload và cập nhật trạng hợp đồng sau các bước duyệt/ký nháy/ký duyệt 
        /// </summary>
        /// <param name="contractId">Id hơp đồng</param>
        /// <param name="reqModel">File pdf và thông tin hợp đồng đã ký </param>
        /// <param name="errMsg"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> SignDigitalContract(string contractId, ReqDigitalSignModel reqModel, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel { ClientId = eContractClientId, ClientSecret = eContractSecret }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.SignDigitalContract(eContractHost, _authToken, contractId, reqModel, out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }
        /// <summary>
        /// 12. Danh sách hợp đồng gởi 
        /// </summary>
        /// <param name="userId">Id của người dùng </param>
        /// <param name="reqModel">ReqContractSentModel</param>
        /// <param name="errMsg"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> GetListSentContracts(string userId, ReqContractSentModel reqModel, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel { ClientId = eContractClientId, ClientSecret = eContractSecret }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.GetListSentContracts(eContractHost, _authToken, userId, reqModel, out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }
        /// <summary>
        /// 13. Danh sách hợp đồng nhận  
        /// </summary>
        /// <param name="userId">Id của người dùng </param>
        /// <param name="reqModel"></param>
        /// <param name="errMsg"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> GetListReceiveContracts(string userId, ReqContractSentModel reqModel, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel { ClientId = eContractClientId, ClientSecret = eContractSecret }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.GetListReceiveContracts(eContractHost, _authToken, userId, reqModel, out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }
        /// <summary>
        /// 14. Xem thông tin chi tiết hợp đồng
        /// </summary>
        /// <param name="contractId">Id hơp đồng</param>
        /// <param name="errMsg"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> GetDetailContract(string contractId, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel { ClientId = eContractClientId, ClientSecret = eContractSecret }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.GetDetailContract(eContractHost, _authToken, contractId, out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }
        /// <summary>
        /// 15. Tải hợp đồng điện tử
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="reqModel">
        ///  ReqContractDownloadModel gồm:
        ///     + contractId: ID hợp đồng trên hệ  thống eContract  
        ///     + documentType: loại hợp đồng (DRAFT/CONTRACT)
        ///     + documentHash: Hash file hợp đồng. Lấy từ API chi tiết hợp đồng 
        /// </param>
        /// <param name="errMsg"></param>
        /// <returns>byte[]</returns>
        public static byte[] DownloadContract(string hostService, string tokenAuth, ReqContractDownloadModel reqModel, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel { ClientId = eContractClientId, ClientSecret = eContractSecret }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.DownloadContract(eContractHost, _authToken, reqModel, out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }
        /// <summary>
        /// 16. Huỷ hợp đồng
        /// </summary>
        /// <param name="contractId">ID hợp đồng trên hệ  thống eContract</param>
        /// <param name="reqModel">
        ///  ReqCancelContractModel gồm:
        ///     + cancelReason: Lý do từ chối
        /// </param>
        /// <param name="errMsg"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> CancelContract(string contractId, ReqCancelContractModel reqModel, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel { ClientId = eContractClientId, ClientSecret = eContractSecret }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.CancelContract(eContractHost, _authToken, contractId, reqModel, out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }
        /// <summary>
        /// 17. Xóa hợp đồng điện tử
        /// </summary>
        /// <param name="contractId">ID hợp đồng trên hệ  thống eContract</param>
        /// <param name="errMsg"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> DeleteContract(string contractId, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel { ClientId = eContractClientId, ClientSecret = eContractSecret }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.DeleteContract(eContractHost, _authToken, contractId, out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }
        /// <summary>
        /// 18. Lấy danh sách bộ phận phân của tổ chức theo cây đơn vị
        /// </summary>
        /// <param name="reqModel">ReqSearchDepartmentModel</param>
        /// <param name="errMsg"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> GetListDepartments(ReqSearchDepartmentModel reqModel, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel { ClientId = eContractClientId, ClientSecret = eContractSecret }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.GetListDepartments(eContractHost, _authToken, reqModel, out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }
        /// <summary>
        /// 19. Lấy danh sách Danh sách nhân sự thuộc bộ phận
        /// </summary>
        /// <param name="partyId">ID bộ phận </param>
        /// <param name="errMsg"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> GetListEmployeesViaDepartment(string partyId, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel { ClientId = eContractClientId, ClientSecret = eContractSecret }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.GetListEmployeesViaDepartment(eContractHost, _authToken, partyId, out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }
        /// <summary>
        /// 20. Danh sách loại hợp đồng của tổ chức
        /// </summary>
        /// <param name="reqModel">ReqSearchContractTypeModel</param>
        /// <param name="errMsg"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> GetListContractTypes(ReqSearchContractTypeModel reqModel, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel { ClientId = eContractClientId, ClientSecret = eContractSecret }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.GetListContractTypes(eContractHost, _authToken, reqModel, out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }

        /// <summary>
        /// 21. Tạo hợp đồng (kèm thông tin định danh)
        /// </summary>
        /// <param name="reqModel">ReqContractWithIdentificationModel</param>
        /// <param name="errMsg"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> CreateContractWithIdentification(ReqContractWithIdentificationModel reqModel, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel { ClientId = eContractClientId, ClientSecret = eContractSecret }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.CreateContractWithIdentification(eContractHost, _authToken, reqModel, out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }

        /// <summary>
        /// 22. Tạo hợp đồng (ký 1 lần)
        /// Tạo hợp đồng từ file pdf hợp đồng, đối tác Cá Nhân vào thực hiện ký 1 lần 
        /// </summary>
        /// <param name="reqModel"></param>
        /// <param name="errMsg"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> CreateContractSignOneTime(ReqContractModel reqModel, out string errMsg)
        {
            int countAuth = 0;
        Auth:

            #region Auth

            HttpStatusCode statusCode;

            if (string.IsNullOrEmpty(_authToken))
            {
                var resAuthModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel { ClientId = eContractClientId, ClientSecret = eContractSecret }, out errMsg, out statusCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    _authToken = $"Bearer {resAuthModel.AccessToken}";
                }
                countAuth += 1;
            }

            #endregion

            var resService = EContractServiceProvider.CreateContractSignOneTime(eContractHost, _authToken, reqModel, out errMsg, out statusCode);
            if (statusCode == HttpStatusCode.OK)
            {
                return resService;
            }

            if (countAuth <= 3 && statusCode == HttpStatusCode.Unauthorized)
                goto Auth;


            return resService;
        }

    }
}