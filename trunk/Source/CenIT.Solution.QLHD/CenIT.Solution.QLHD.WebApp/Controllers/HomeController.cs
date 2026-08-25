using System;
using System.Web.Mvc;
using CenIT.Solution.QLHD.WebApp.Models;
using Cores.Base.Helpers;
using Cores.Cate.Caches;
using Cores.Major.Caches;
using Cores.Major.Models;
using Cores.Major.Providers;

namespace CenIT.Solution.QLHD.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly FEContractCache _feContractCache = new FEContractCache();
        private readonly MajorContractCache _contractCache = new MajorContractCache();
        private readonly CateDocCache _docCache = new CateDocCache();

        //trang chu - Chuyen huong ve site quan tri (Account/Login)
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Account");
        }

        //Xem thong tin hop dong
        public ActionResult Search(SearchFEContractModel searchModel)
        {
            //searchModel.ContractNo = "19/2024/HĐ-DVDV";
            //searchModel.Phone = "065656565";

            var dataContract = _feContractCache.DetailContract(searchModel.ContractNo, searchModel.Phone);
            var dataTasks = _contractCache.GetTask(dataContract?.ContractId);
            var dataPayments = _feContractCache.PaymentGetByContractId(dataContract?.ContractId);
            var data = new DetailContract
            {
                ContractId = dataContract?.ContractId,
                ListContractTasks = dataTasks,
                ListPayment = dataPayments,
                ContractModel = dataContract,
                ListRefFiles = _docCache.GetByObjectId($"{dataContract?.ContractId}")
            };

            return PartialView("_Detail", data);
        }

        //    Mã hóa Guid? contractId
        //    string enContractId = CenIT.Solution.QLHD.WebApp.Providers.SecurityHelper.EncryptId(contractId.Value);
        public ActionResult QRContract(string enContractId)
        {
            // Giải mã contractId
            Guid? decryptedId = Providers.SecurityHelper.DecryptId(enContractId);

            var dataTasks = _contractCache.GetTask(decryptedId);
            var dataContract = _feContractCache.DetailByQRCode(decryptedId);
            var dataPayments = _feContractCache.PaymentGetByContractId(decryptedId);
            var data = new DetailContract
            {
                ListContractTasks = dataTasks,
                ListPayment = dataPayments,
                ContractModel = dataContract
            };

            return PartialView("_DetailQRCode", data);
        }

        //Hiển thị hợp đồng
        public ActionResult ShowContract(string id)
        {
            // Giải mã contractId
            Guid contractId = SecurityHelper.DecryptId(id);

            var contractModel = _contractCache.GetById(contractId);
            if (contractModel == null)
                return Json(new
                {
                    status = true,
                    message = "Không tìm thấy dữ liệu hợp đồng"
                });

            contractModel.RenderContractId = id;
            contractModel.FileType = ".pdf";

            // Gọi phương thức RenderContract và lấy dữ liệu hợp đồng
            byte[] contractData = RenderContract(contractId); // Thay thế null bằng templateId nếu cần

            // Chuyển đổi dữ liệu từ byte[] sang chuỗi UTF-8
            string base64String = Convert.ToBase64String(contractData);

            return PartialView("_ShowContract", base64String);
        }

        //render hop dong
        public byte[] RenderContract(Guid id)
        {
            var host = Request.Url.Host;

            var urlViewContract = host + "/Home/QRContract?enContractId=";
            urlViewContract += SecurityHelper.EncryptId(id);

            FERenderContractModel rendercontract = _feContractCache.GetDataRenderContract(id);
            var jsondata = rendercontract.JsonContractInfo;

            byte[] fileBytes = RenderContractProvider.RenderModelToPdfAndSave(rendercontract.TemplatePath, jsondata, rendercontract.IndexTabel, rendercontract.IndexRowInTable, urlViewContract);

            return fileBytes;
        }
    }
}