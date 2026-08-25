using CenIT.Lib.SMSBrandName.Helpers;
using CenIT.Lib.SMSBrandName.Models;
using Cores.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json;
using TSFramework.Plugable.Interfaces;

namespace Cores.Base.Providers
{
    public class SMSProvider
    {
        /// <summary>
        /// Gửi SMS Thông báo xác nhận thực hiện hợp đồng
        /// </summary>
        /// <param name="phone">số điện thoại nhận</param>
        /// <param name="contractNoInfo">mã hợp đồng</param>
        /// <param name="url">link tra cứu</param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static ResponseSendSMSList Send_SMS_Contract_Confirm(string phone, string contractNoInfo, string url, IBasePrincipal user = null)
        {
            RequestSendSMSListModel sendSMSModel = new RequestSendSMSListModel();
            DataRequestSendSMSListModel rqstData = new DataRequestSendSMSListModel
            {
                AGENTID = ConfigurationManager.AppSettings["SMS_BRANDNAME_AGENTID"],
                APIPASS = ConfigurationManager.AppSettings["SMS_BRANDNAME_APIPASS"],
                APIUSER = ConfigurationManager.AppSettings["SMS_BRANDNAME_APIUSER"],
                CONTRACTID = ConfigurationManager.AppSettings["SMS_BRANDNAME_CONTRACTID"],
                CONTRACTTYPEID = ConfigurationManager.AppSettings["SMS_BRANDNAME_CONTRACTTYPEID"],
                DATACODING = "0",
                ISTELCOSUB = "0",
                LABELID = ConfigurationManager.AppSettings["SMS_BRANDNAME_LABELID"],
                MOBILELIST = UtilSMSBrandNameHelper.ConvertPhone(phone),
                PACKAGEID = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                REQID = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                SALEORDERID = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                SCHEDULETIME = "",
                USERNAME = ConfigurationManager.AppSettings["SMS_BRANDNAME_USERNAME"],
                TEMPLATEID = ConfigurationManager.AppSettings["SMS_BRANDNAME_TEMPLATEID_OTP_CONFIRM"]
            };

            List<DataRequestParamsSendSMSListModel> reqParams = new List<DataRequestParamsSendSMSListModel>();

            // Tạo một đối tượng tham số mới và thiết lập nội dung
            DataRequestParamsSendSMSListModel param1 = new DataRequestParamsSendSMSListModel()
            {
                NUM = "1", // Số thứ tự của tham số
                CONTENT = VietnameseStringHelper.RemoveVietnameseAccents(contractNoInfo)

            };
            reqParams.Add(param1);

            // Tạo một đối tượng tham số mới và thiết lập nội dung
            DataRequestParamsSendSMSListModel param2 = new DataRequestParamsSendSMSListModel()
            {
                NUM = "2", // Số thứ tự của tham số
                CONTENT = url
            };
            reqParams.Add(param2);
            rqstData.PARAMS = reqParams;

            sendSMSModel.RQST = rqstData;

            APIHelper apiHelper = new APIHelper();
            ResponseSendSMSList response = apiHelper.CallAPI<ResponseSendSMSList>(JsonConvert.SerializeObject(sendSMSModel), user);
            return response;
        }

        /// <summary>
        /// Thông báo hợp đồng bị từ chối 
        /// </summary>
        /// <param name="phone">số điện thoại nhận</param>
        /// <param name="contractNoInfo">mã hợp đồng</param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static ResponseSendSMSList Send_SMS_Contract_Refuse(string phone, string contractNoInfo, IBasePrincipal user = null)
        {
            RequestSendSMSListModel sendSMSModel = new RequestSendSMSListModel();
            DataRequestSendSMSListModel rqstData = new DataRequestSendSMSListModel
            {
                AGENTID = ConfigurationManager.AppSettings["SMS_BRANDNAME_AGENTID"],
                APIPASS = ConfigurationManager.AppSettings["SMS_BRANDNAME_APIPASS"],
                APIUSER = ConfigurationManager.AppSettings["SMS_BRANDNAME_APIUSER"],
                CONTRACTID = ConfigurationManager.AppSettings["SMS_BRANDNAME_CONTRACTID"],
                CONTRACTTYPEID = ConfigurationManager.AppSettings["SMS_BRANDNAME_CONTRACTTYPEID"],
                DATACODING = "0",
                ISTELCOSUB = "0",
                LABELID = ConfigurationManager.AppSettings["SMS_BRANDNAME_LABELID"],
                MOBILELIST = UtilSMSBrandNameHelper.ConvertPhone(phone),
                PACKAGEID = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                REQID = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                SALEORDERID = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                SCHEDULETIME = "",
                USERNAME = ConfigurationManager.AppSettings["SMS_BRANDNAME_USERNAME"],
                TEMPLATEID = ConfigurationManager.AppSettings["SMS_BRANDNAME_TEMPLATEID_OTP_REFUSE"]
            };

            List<DataRequestParamsSendSMSListModel> reqParams = new List<DataRequestParamsSendSMSListModel>();

            // Tạo một đối tượng tham số mới và thiết lập nội dung
            DataRequestParamsSendSMSListModel param1 = new DataRequestParamsSendSMSListModel()
            {
                NUM = "1", // Số thứ tự của tham số
                CONTENT = VietnameseStringHelper.RemoveVietnameseAccents(contractNoInfo)

            };
            reqParams.Add(param1);
            rqstData.PARAMS = reqParams;

            sendSMSModel.RQST = rqstData;

            APIHelper apiHelper = new APIHelper();
            ResponseSendSMSList response = apiHelper.CallAPI<ResponseSendSMSList>(JsonConvert.SerializeObject(sendSMSModel), user);
            return response;
        }

        /// <summary>
        /// Thông báo Thông báo hợp đồng có kết quả 
        /// </summary>
        /// <param name="phone">số điện thoại nhận</param>
        /// <param name="contractNoInfo">mã hợp đồng</param>
        /// <param name="User"></param>
        /// <returns></returns>
        public static ResponseSendSMSList Send_SMS_Contract_Resolved(string phone, string contractNoInfo, IBasePrincipal User = null)
        {
            RequestSendSMSListModel sendSMSModel = new RequestSendSMSListModel();
            DataRequestSendSMSListModel rqstData = new DataRequestSendSMSListModel
            {
                AGENTID = ConfigurationManager.AppSettings["SMS_BRANDNAME_AGENTID"],
                APIPASS = ConfigurationManager.AppSettings["SMS_BRANDNAME_APIPASS"],
                APIUSER = ConfigurationManager.AppSettings["SMS_BRANDNAME_APIUSER"],
                CONTRACTID = ConfigurationManager.AppSettings["SMS_BRANDNAME_CONTRACTID"],
                CONTRACTTYPEID = ConfigurationManager.AppSettings["SMS_BRANDNAME_CONTRACTTYPEID"],
                DATACODING = "0",
                ISTELCOSUB = "0",
                LABELID = ConfigurationManager.AppSettings["SMS_BRANDNAME_LABELID"],
                MOBILELIST = UtilSMSBrandNameHelper.ConvertPhone(phone),
                PACKAGEID = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                REQID = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                SALEORDERID = DateTime.Now.ToString("ddMMyyyyHHmmss"),
                SCHEDULETIME = "",
                USERNAME = ConfigurationManager.AppSettings["SMS_BRANDNAME_USERNAME"],
                TEMPLATEID = ConfigurationManager.AppSettings["SMS_BRANDNAME_TEMPLATEID_OTP_RESOLVED"]
            };

            List<DataRequestParamsSendSMSListModel> reqParams = new List<DataRequestParamsSendSMSListModel>();

            // Tạo một đối tượng tham số mới và thiết lập nội dung
            DataRequestParamsSendSMSListModel param1 = new DataRequestParamsSendSMSListModel()
            {
                NUM = "1", // Số thứ tự của tham số
                CONTENT = VietnameseStringHelper.RemoveVietnameseAccents(contractNoInfo)

            };
            reqParams.Add(param1);
            rqstData.PARAMS = reqParams;

            sendSMSModel.RQST = rqstData;

            APIHelper apiHelper = new APIHelper();
            ResponseSendSMSList response = apiHelper.CallAPI<ResponseSendSMSList>(JsonConvert.SerializeObject(sendSMSModel), User);
            return response;
        }

    }
}
