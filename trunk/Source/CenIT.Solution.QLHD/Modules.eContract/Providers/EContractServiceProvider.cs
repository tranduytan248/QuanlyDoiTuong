using Cores.eContract.Consts;
using Cores.eContract.Models;
using Cores.eContract.Models.Request;
using Cores.eContract.Models.Response;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Web;
using TSFramework.Core.Providers;
using TSFramework.Core.Utils;

namespace Modules.eContract.Providers
{
    public static class EContractServiceProvider
    {
        public static JsonSerializerSettings JsonSetting = new JsonSerializerSettings
        {
            StringEscapeHandling = StringEscapeHandling.EscapeHtml,
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// 1. Xác thực, định danh Third Party
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="authModel">Request body (ReqAuthModel) </param>
        /// <param name="errMsg">Nội dung lỗi trong trường hợp request lỗi</param>
        /// <param name="statusCode"></param>
        /// <returns>ResAuthModel</returns>
        public static ResAuthModel AuthToken(string hostService, ReqAuthModel authModel, out string errMsg, out HttpStatusCode statusCode)
        {
            errMsg = null;
            var authTokenService = $"{hostService}{ConstsEContractServices.OAUTH_SERVICE}";
            var dataAuthJson = JsonConvert.SerializeObject(authModel, JsonSetting);

            var restResponse = RestServiceProvider.Post(authTokenService, new Dictionary<string, string>(),
                ConstsContentTypes.JSON, dataAuthJson);

            statusCode = restResponse.StatusCode;
            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                var resAuthTokenContent = restResponse.Content;
                if (string.IsNullOrEmpty(resAuthTokenContent)) return null;
                if (restResponse.ContentType == ConstsContentTypes.JSON_UTF8)
                {
                    var resAuthTokenModel = JsonConvert.DeserializeObject<ResAuthModel>(resAuthTokenContent);
                    return resAuthTokenModel;
                }

                errMsg = restResponse.Content;
                return null;
            }
            errMsg = restResponse.ErrorMessage ?? restResponse.StatusDescription;
            return null;
        }

        /// <summary>
        /// 2. Xác thực người dùng để xem và ký hợp đồng 
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="authModel">Request body (ReqAuthUserModel) </param>
        /// <param name="errMsg">Nội dung lỗi trong trường hợp request lỗi</param>
        /// <param name="statusCode"></param>
        /// <returns>ResAuthUserModel</returns>
        public static ResAuthUserModel AuthToken(string hostService, ReqAuthUserModel authModel, out string errMsg, out HttpStatusCode statusCode)
        {
            errMsg = null;
            var authTokenService = $"{hostService}{ConstsEContractServices.OAUTH_SERVICE}";
            var dataAuthJson = JsonConvert.SerializeObject(authModel, JsonSetting);

            var restResponse = RestServiceProvider.Post(authTokenService, new Dictionary<string, string>(),
                ConstsContentTypes.JSON, dataAuthJson);
            statusCode = restResponse.StatusCode;
            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                var resAuthTokenContent = restResponse.Content;
                if (string.IsNullOrEmpty(resAuthTokenContent)) return null;
                if (restResponse.ContentType == ConstsContentTypes.JSON_UTF8)
                {
                    var resAuthTokenModel = JsonConvert.DeserializeObject<ResAuthUserModel>(resAuthTokenContent);
                    return resAuthTokenModel;
                }
                errMsg = restResponse.Content;
                return null;
            }

            errMsg = restResponse.ErrorMessage ?? restResponse.StatusDescription;
            return null;
        }

        /// <summary>
        /// 3. Lấy Danh sách mẫu hợp đồng 
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="reqModel">Thông tin tìm kiếm + phân trang</param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<ResTemplateContractModel> GetContractTemplates(string hostService, string tokenAuth, ReqSearchModel reqModel, out string errMsg, out HttpStatusCode statusCode)
        {
            errMsg = null;
            var urlService = $"{hostService}{ConstsEContractServices.TEMPLATE_SERVICE_LIST}";
            urlService = $"{urlService}?{(reqModel != null ? reqModel.ToQueryString() : "")}";
            return RequestGetService<BaseResponseModel<ResTemplateContractModel>>(urlService, tokenAuth, out errMsg, out statusCode);
        }

        /// <summary>
        /// 4. Thông tin chi tiết hợp đồng mẫu để tạo hợp đồng 
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="templateId">Id hợp đồng mẫu </param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<ResDetailTemplateContractModel> GetDetailTemplateContract(string hostService, string tokenAuth, string templateId, out string errMsg, out HttpStatusCode statusCode)
        {
            var urlService = $"{hostService}{ConstsEContractServices.TEMPLATE_SERVICE_DETAIL_TEMPLATE}";
            urlService = urlService.Format(new Dictionary<string, object> { { "{templateId}", templateId } });
            return RequestGetService<BaseResponseModel<ResDetailTemplateContractModel>>(urlService, tokenAuth, out errMsg, out statusCode);
        }

        /// <summary>
        /// 5. Render hợp đồng từ mẫu hợp đồng
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="templateId">Id hợp đồng mẫu </param>
        /// <param name="dataFields">Dữ liệu hợp đồng dạng key=>value </param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns>byte[]</returns>
        public static byte[] RenderContract(string hostService, string tokenAuth, string templateId, string dataFields, out string errMsg, out HttpStatusCode statusCode)
        {
            errMsg = null;
            var urlService = $"{hostService}{ConstsEContractServices.TEMPLATE_SERVICE_RENDER}";
            urlService = urlService.Format(new Dictionary<string, object> { { "{templateId}", templateId } });

            var restResponse = RestServiceProvider.Post(urlService, new Dictionary<string, string>
                {
                    { "Authorization", tokenAuth },
                },
                ConstsContentTypes.JSON, dataFields);
            statusCode = restResponse.StatusCode;
            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                if (restResponse.RawBytes == null || restResponse.RawBytes.Length <= 0) return null;
                if (restResponse.ContentType == ConstsContentTypes.FILE_PDF)
                {
                    return restResponse.RawBytes;
                }
            }
            return null;
        }

        /// <summary>
        /// 6. Lấy danh sách tọa độ các biến vị trí trong file PDF: tìm danh sách các chữ @{1}, 
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="attachFile">File pdf tài liệu cần lấy thông tin </param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<ResListPositionSignatureModel> GetListPosition(string hostService, string tokenAuth, HttpPostedFileBase attachFile, out string errMsg, out HttpStatusCode statusCode)
        {
            errMsg = null;
            var urlService = $"{hostService}{ConstsEContractServices.ESIGNATURE_SERVICE_LIST_POSITION}";
            var restResponse = RestServiceProvider.PostFile(urlService, new Dictionary<string, string>
            {
                { "Authorization", tokenAuth },
                { "Content-Type", ConstsContentTypes.FORM_DATA }
            }, ConstsContentTypes.FORM_DATA, null, new Dictionary<string, HttpPostedFileBase> { { "attachFile", attachFile } });

            statusCode = restResponse.StatusCode;
            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                var resAuthTokenContent = restResponse.Content;
                if (string.IsNullOrEmpty(resAuthTokenContent)) return null;
                if (restResponse.ContentType == ConstsContentTypes.JSON) //ConstsContentTypes.JSON_UTF8
                {
                    var resAuthTokenModel = JsonConvert.DeserializeObject<BaseResponseModel<ResListPositionSignatureModel>>(resAuthTokenContent);
                    return resAuthTokenModel;
                }
                errMsg = restResponse.Content;
                return null;
            }
            var resErrContentModel = JsonConvert.DeserializeObject<BaseResponseModel<ResListPositionSignatureModel>>(restResponse.Content);
            return resErrContentModel;
        }

        /// <summary>
        /// 7. Danh sách luồng hợp đồng
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="reqModel">Params request</param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static BaseResponseModel<ResListFlowContractModel> GetListFlowContract(string hostService, string tokenAuth,
            ReqListFlowContractModel reqModel, out string errMsg, out HttpStatusCode statusCode)
        {
            var urlService = $"{hostService}{ConstsEContractServices.ESIGNATURE_SERVICE_CONTRACT_FLOW}";
            urlService = $"{urlService}?{(reqModel != null ? reqModel.ToQueryString() : "")}";
            return RequestGetService<BaseResponseModel<ResListFlowContractModel>>(urlService, tokenAuth, out errMsg, out statusCode);
        }

        /// <summary>
        /// 8. Chi tiết luồng hợp đồng
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="contractFlowTemplateId">Id luồng hợp đồng lấy từ API GetListFlowContract </param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<FlowContractModel> GetDetailFlowContact(string hostService, string tokenAuth,
            string contractFlowTemplateId, out string errMsg, out HttpStatusCode statusCode)
        {
            var urlService = $"{hostService}{ConstsEContractServices.ESIGNATURE_SERVICE_CONTRACT_FLOW_DETAIL}";
            urlService = urlService.Format(new Dictionary<string, object> { { "{contractFlowTemplateId}", contractFlowTemplateId } });
            return RequestGetService<BaseResponseModel<FlowContractModel>>(urlService, tokenAuth, out errMsg, out statusCode);
        }

        /// <summary>
        /// 9. Tạo hợp đồng từ file pdf hợp đồng
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="contractModel"></param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> CreateContract(string hostService, string tokenAuth,
            ReqContractModel contractModel, out string errMsg, out HttpStatusCode statusCode)
        {
            errMsg = null;
            var urlService = $"{hostService}{ConstsEContractServices.ESIGNATURE_SERVICE_CONTRACT_CREATE}";
            var dataReqJson = JsonConvert.SerializeObject(contractModel, JsonSetting);

            var restResponse = RestServiceProvider.PostFile(urlService, new Dictionary<string, string>
            {
                { "Authorization", tokenAuth },
                { "Content-Type", ConstsContentTypes.FORM_DATA }
            }, ConstsContentTypes.FORM_DATA, dataReqJson, new Dictionary<string, HttpPostedFileBase>
            {
                { "attachFile", contractModel.AttachFile }
            });
            statusCode = restResponse.StatusCode;
            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                var resContent = restResponse.Content;
                if (string.IsNullOrEmpty(resContent)) return null;
                if (restResponse.ContentType == ConstsContentTypes.JSON_UTF8)
                {
                    var resContentModel = JsonConvert.DeserializeObject<BaseResponseModel<object>>(resContent);
                    return resContentModel;
                }
                errMsg = restResponse.Content;
                return null;
            }

            var resErrContentModel = JsonConvert.DeserializeObject<BaseResponseModel<object>>(restResponse.Content);
            return resErrContentModel;
        }

        /// <summary>
        /// 10. Gửi hợp đồng cho nội bộ hoặc đối tác theo luồng HĐ đã cấu hình. 
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="contractId">Id hơp đồng</param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> SubmitContract(string hostService, string tokenAuth,
            string contractId, out string errMsg, out HttpStatusCode statusCode)
        {
            var urlService = $"{hostService}{ConstsEContractServices.ESIGNATURE_SERVICE_CONTRACT_SUBMIT}";
            urlService = urlService.Format(new Dictionary<string, object> { { "{contractId}", contractId } });
            return RequestPostService<BaseResponseModel<object>>(urlService, tokenAuth, null, out errMsg, out statusCode);
        }

        /// <summary>
        /// 11. Upload và cập nhật trạng hợp đồng sau các bước duyệt/ký nháy/ký duyệt 
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="contractId">Id hơp đồng</param>
        /// <param name="reqModel">File pdf và thông tin hợp đồng đã ký </param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static BaseResponseModel<object> SignDigitalContract(string hostService, string tokenAuth, string contractId,
            ReqDigitalSignModel reqModel, out string errMsg, out HttpStatusCode statusCode)
        {
            var urlService = $"{hostService}{ConstsEContractServices.ESIGNATURE_SERVICE_CONTRACT_SIGN}";
            urlService = urlService.Format(new Dictionary<string, object> { { "{contractId}", contractId } });
            var dataReqJson = JsonConvert.SerializeObject(reqModel, JsonSetting);

            return RequestPostService<BaseResponseModel<object>>(urlService, tokenAuth, dataReqJson, out errMsg, out statusCode);
        }

        /// <summary>
        /// 12. Danh sách hợp đồng gởi 
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="userId">Id của người dùng </param>
        /// <param name="reqModel">ReqContractSentModel</param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> GetListSentContracts(string hostService, string tokenAuth, string userId,
            ReqContractSentModel reqModel, out string errMsg, out HttpStatusCode statusCode)
        {
            var urlService = $"{hostService}{ConstsEContractServices.ESIGNATURE_SERVICE_CONTRACT_LIST_SENT}";
            urlService = urlService.Format(new Dictionary<string, object> { { "{userId}", userId } });
            urlService = $"{urlService}?{(reqModel != null ? reqModel.ToQueryString() : "")}";
            return RequestGetService<BaseResponseModel<object>>(urlService, tokenAuth, out errMsg, out statusCode);
        }

        /// <summary>
        /// 13. Danh sách hợp đồng nhận  
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="userId">Id của người dùng </param>
        /// <param name="reqModel"></param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> GetListReceiveContracts(string hostService, string tokenAuth, string userId, ReqContractSentModel reqModel, out string errMsg, out HttpStatusCode statusCode)
        {
            var urlService = $"{hostService}{ConstsEContractServices.ESIGNATURE_SERVICE_CONTRACT_LIST_RECEIVE}";
            urlService = urlService.Format(new Dictionary<string, object> { { "{userId}", userId } });
            urlService = $"{urlService}?{(reqModel != null ? reqModel.ToQueryString() : "")}";
            return RequestGetService<BaseResponseModel<object>>(urlService, tokenAuth, out errMsg, out statusCode);
        }

        /// <summary>
        /// 14. Xem thông tin chi tiết hợp đồng
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="contractId">Id hơp đồng</param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static BaseResponseModel<object> GetDetailContract(string hostService, string tokenAuth, string contractId, out string errMsg, out HttpStatusCode statusCode)
        {
            var urlService = $"{hostService}{ConstsEContractServices.ESIGNATURE_SERVICE_CONTRACT_DETAIL}";
            urlService = urlService.Format(new Dictionary<string, object> { { "{contractId}", contractId } });
            return RequestGetService<BaseResponseModel<object>>(urlService, tokenAuth, out errMsg, out statusCode);
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
        /// <param name="statusCode"></param>
        /// <returns>byte[]</returns>
        public static byte[] DownloadContract(string hostService, string tokenAuth, ReqContractDownloadModel reqModel, out string errMsg, out HttpStatusCode statusCode)
        {
            var urlService = $"{hostService}{ConstsEContractServices.ESIGNATURE_SERVICE_CONTRACT_DOWNLOAD}";
            urlService = $"{urlService}?{(reqModel != null ? reqModel.ToQueryString() : "")}";
            return RequestDownloadFileService(urlService, tokenAuth, out errMsg, out statusCode);
        }

        /// <summary>
        /// 16. Huỷ hợp đồng
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="contractId">ID hợp đồng trên hệ  thống eContract</param>
        /// <param name="reqModel">
        ///  ReqCancelContractModel gồm:
        ///     + cancelReason: Lý do từ chối
        /// </param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> CancelContract(string hostService, string tokenAuth, string contractId, ReqCancelContractModel reqModel, out string errMsg, out HttpStatusCode statusCode)
        {
            var urlService = $"{hostService}{ConstsEContractServices.ESIGNATURE_SERVICE_CONTRACT_CANCEL}";
            urlService = urlService.Format(new Dictionary<string, object> { { "{contractId}", contractId } });
            var dataReqJson = JsonConvert.SerializeObject(reqModel, JsonSetting);

            return RequestPostService<BaseResponseModel<object>>(urlService, tokenAuth, dataReqJson, out errMsg, out statusCode);
        }

        /// <summary>
        /// 17. Xóa hợp đồng điện tử
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="contractId">ID hợp đồng trên hệ  thống eContract</param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> DeleteContract(string hostService, string tokenAuth, string contractId, out string errMsg, out HttpStatusCode statusCode)
        {
            var urlService = $"{hostService}{ConstsEContractServices.ESIGNATURE_SERVICE_CONTRACT_DELETE}";
            urlService = urlService.Format(new Dictionary<string, object> { { "{contractId}", contractId } });
            return RequestPostService<BaseResponseModel<object>>(urlService, tokenAuth, null, out errMsg, out statusCode);
        }

        /// <summary>
        /// 18. Lấy danh sách bộ phận phân của tổ chức theo cây đơn vị
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="reqModel">ReqSearchDepartmentModel</param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> GetListDepartments(string hostService, string tokenAuth, ReqSearchDepartmentModel reqModel, out string errMsg, out HttpStatusCode statusCode)
        {
            var urlService = $"{hostService}{ConstsEContractServices.ESIGNATURE_SERVICE_DEPARTMENT_SEARCH}";
            urlService = $"{urlService}?{(reqModel != null ? reqModel.ToQueryString() : "")}";
            return RequestPostService<BaseResponseModel<object>>(urlService, tokenAuth, null, out errMsg, out statusCode);
        }

        /// <summary>
        /// 19. Lấy danh sách Danh sách nhân sự thuộc bộ phận
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="partyId">ID bộ phận </param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> GetListEmployeesViaDepartment(string hostService, string tokenAuth,
            string partyId, out string errMsg, out HttpStatusCode statusCode)
        {
            var urlService = $"{hostService}{ConstsEContractServices.ESIGNATURE_SERVICE_DEPARTMENT_EMPLOYEE_SEARCH}";
            urlService = urlService.Format(new Dictionary<string, object> { { "{partyId}", partyId } });
            return RequestGetService<BaseResponseModel<object>>(urlService, tokenAuth, out errMsg, out statusCode);
        }

        /// <summary>
        /// 20. Danh sách loại hợp đồng của tổ chức
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="reqModel">ReqSearchContractTypeModel</param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> GetListContractTypes(string hostService, string tokenAuth,
            ReqSearchContractTypeModel reqModel, out string errMsg, out HttpStatusCode statusCode)
        {
            var urlService = $"{hostService}{ConstsEContractServices.ESOLUTION_SERVICE_CONTRACT_TYPES}";
            urlService = $"{urlService}?{(reqModel != null ? reqModel.ToQueryString() : "")}";
            return RequestPostService<BaseResponseModel<object>>(urlService, tokenAuth, null, out errMsg, out statusCode);
        }

        /// <summary>
        /// 21. Tạo hợp đồng (kèm thông tin định danh)
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="reqModel">ReqContractWithIdentificationModel</param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> CreateContractWithIdentification(string hostService, string tokenAuth,
            ReqContractWithIdentificationModel reqModel, out string errMsg, out HttpStatusCode statusCode)
        {
            errMsg = null;

            var urlService = $"{hostService}{ConstsEContractServices.ESOLUTION_SERVICE_CREATE_CONTRACT_WITH_IDENTIFICATION}";
            var dataReqJson = JsonConvert.SerializeObject(reqModel, JsonSetting);
            var restResponse = RestServiceProvider.PostFile(urlService, new Dictionary<string, string>
            {
                { "Authorization", tokenAuth },
                { "Content-Type", ConstsContentTypes.FORM_URLENCODED }
            }, ConstsContentTypes.FORM_DATA, dataReqJson, reqModel.ToDictionary<HttpPostedFileBase>());
            var resContent = restResponse.Content;
            statusCode = restResponse.StatusCode;
            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                if (string.IsNullOrEmpty(resContent)) return null;
                if (restResponse.ContentType == ConstsContentTypes.JSON_UTF8)
                {
                    var resContentModel = JsonConvert.DeserializeObject<BaseResponseModel<object>>(resContent);
                    return resContentModel;
                }
                errMsg = restResponse.Content;
                return null;
            }
            var resErrContentModel = JsonConvert.DeserializeObject<BaseResponseModel<object>>(resContent);
            errMsg = resErrContentModel?.Error[0] ?? restResponse.ErrorMessage ?? restResponse.StatusDescription;
            return null;
        }

        /// <summary>
        /// 22. Tạo hợp đồng (ký 1 lần)
        /// Tạo hợp đồng từ file pdf hợp đồng, đối tác Cá Nhân vào thực hiện ký 1 lần 
        /// </summary>
        /// <param name="hostService">Host Service</param>
        /// <param name="tokenAuth">Token lấy từ hàm AuthToken</param>
        /// <param name="contractModel"></param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns>BaseResponseModel</returns>
        public static BaseResponseModel<object> CreateContractSignOneTime(string hostService, string tokenAuth,
            ReqContractModel contractModel, out string errMsg, out HttpStatusCode statusCode)
        {
            errMsg = null;
            var urlService = $"{hostService}{ConstsEContractServices.ESOLUTION_SERVICE_CREATE_CONTRACT_SIGN_ONE_TIME}";
            var dataReqJson = JsonConvert.SerializeObject(contractModel, JsonSetting);

            var restResponse = RestServiceProvider.PostFile(urlService, new Dictionary<string, string>
            {
                { "Authorization", tokenAuth },
                { "Content-Type", ConstsContentTypes.FORM_URLENCODED }
            }, ConstsContentTypes.FORM_DATA, dataReqJson, new Dictionary<string, HttpPostedFileBase>
            {
                {"file",contractModel.AttachFile}
            });

            var resContent = restResponse.Content;
            statusCode = restResponse.StatusCode;
            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                if (string.IsNullOrEmpty(resContent)) return null;
                if (restResponse.ContentType == ConstsContentTypes.JSON_UTF8)
                {
                    var resContentModel = JsonConvert.DeserializeObject<BaseResponseModel<object>>(resContent);
                    return resContentModel;
                }
                errMsg = restResponse.Content;
                return null;
            }
            var resErrContentModel = JsonConvert.DeserializeObject<BaseResponseModel<object>>(resContent);
            errMsg = resErrContentModel?.Error[0] ?? restResponse.ErrorMessage ?? restResponse.StatusDescription;
            return null;
        }

        /// <summary>
        /// Request dùng chung dạng GET
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlService"></param>
        /// <param name="tokenAuth"></param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        private static T RequestGetService<T>(string urlService, string tokenAuth, out string errMsg, out HttpStatusCode statusCode)
        {
            errMsg = null;
            var restResponse = RestServiceProvider.Get(urlService, new Dictionary<string, string>
            {
                { "Authorization", tokenAuth }
            });

            var resContent = restResponse.Content;
            statusCode = restResponse.StatusCode;

            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                if (string.IsNullOrEmpty(resContent)) return default(T);
                if (restResponse.ContentType == ConstsContentTypes.JSON_UTF8 || restResponse.ContentType == ConstsContentTypes.JSON)
                {
                    var resContentModel = JsonConvert.DeserializeObject<T>(resContent);
                    return resContentModel;
                }
                errMsg = restResponse.Content;
                return default(T);
            }
            var resErrContentModel = JsonConvert.DeserializeObject<BaseResponseModel<object>>(resContent);
            errMsg = resErrContentModel?.Error[0] ?? restResponse.ErrorMessage ?? restResponse.StatusDescription;
            return default(T);
        }

        /// <summary>
        /// Request tải file
        /// </summary>
        /// <param name="urlService"></param>
        /// <param name="tokenAuth"></param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        private static byte[] RequestDownloadFileService(string urlService, string tokenAuth, out string errMsg, out HttpStatusCode statusCode)
        {
            errMsg = null;
            var restResponse = RestServiceProvider.Get(urlService, new Dictionary<string, string>
            {
                    { "Authorization", tokenAuth },
                    { "Content-Type", ConstsContentTypes.JSON }
            });

            var resContent = restResponse.Content;
            statusCode = restResponse.StatusCode;

            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                if (string.IsNullOrEmpty(resContent)) return null;
                if (restResponse.ContentType != ConstsContentTypes.JSON_UTF8 && restResponse.ContentType != ConstsContentTypes.JSON)
                {
                    return restResponse.RawBytes;
                }
                errMsg = restResponse.Content;
                return null;
            }
            var resErrContentModel = JsonConvert.DeserializeObject<BaseResponseModel<object>>(resContent);
            errMsg = resErrContentModel?.Error[0] ?? restResponse.ErrorMessage ?? restResponse.StatusDescription;
            return null;
        }

        /// <summary>
        /// Request dùng chung dạng POST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlService"></param>
        /// <param name="tokenAuth"></param>
        /// <param name="bodyData"></param>
        /// <param name="errMsg"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        private static T RequestPostService<T>(string urlService, string tokenAuth, string bodyData, out string errMsg, out HttpStatusCode statusCode)
        {
            errMsg = null;
            var restResponse = RestServiceProvider.Post(urlService, new Dictionary<string, string>
            {
                { "Authorization", tokenAuth },
                //{ "Content-Type", ConstsContentTypes.JSON }
            }, ConstsContentTypes.JSON, bodyData);

            var resContent = restResponse.Content;
            statusCode = restResponse.StatusCode;
            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                if (string.IsNullOrEmpty(resContent)) return default(T);
                if (restResponse.ContentType == ConstsContentTypes.JSON_UTF8)
                {
                    var resContentModel = JsonConvert.DeserializeObject<T>(resContent);
                    return resContentModel;
                }
                errMsg = restResponse.Content;
                return default(T);
            }
            var resErrContentModel = JsonConvert.DeserializeObject<BaseResponseModel<object>>(resContent);
            errMsg = resErrContentModel?.Error[0] ?? restResponse.ErrorMessage ?? restResponse.StatusDescription;
            return default(T);
        }
    }
}