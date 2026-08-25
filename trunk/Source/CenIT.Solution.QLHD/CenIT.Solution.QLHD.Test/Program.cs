using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;
using Cores.eContract.Consts;
using Cores.eContract.Models;
using Cores.eContract.Models.Request;
using Cores.Sys.Caches.Sys;
using Modules.eContract.Providers;

namespace CenIT.Solution.QLHD.Test
{
    internal class Program
    {
        //const string CONFIG_KEY_ECONTRACT_HOST_API = "CONFIG_KEY_ECONTRACT_HOST_API";
        //const string CONFIG_KEY_ECONTRACT_CLIENT_ID = "CONFIG_KEY_ECONTRACT_CLIENT_ID";
        //const string CONFIG_KEY_ECONTRACT_CLIENT_SECRET = "CONFIG_KEY_ECONTRACT_CLIENT_SECRET";
        //const string CONFIG_KEY_ECONTRACT_ACCOUNT_USERNAME = "CONFIG_KEY_ECONTRACT_USERNAME";
        //const string CONFIG_KEY_ECONTRACT_ACCOUNT_PASSWORD = "CONFIG_KEY_ECONTRACT_ACCOUNT_PASSWORD";

        static void Main(string[] args)
        {
            #region Configs

            //SysConfigCache sysConfigCache = new SysConfigCache();

            //var eContractHost = sysConfigCache.GetViaKey(CONFIG_KEY_ECONTRACT_HOST_API)?.ConfigValue;
            //var eContractClientId = sysConfigCache.GetViaKey(CONFIG_KEY_ECONTRACT_CLIENT_ID)?.ConfigValue;
            //var eContractSecret = sysConfigCache.GetViaKey(CONFIG_KEY_ECONTRACT_CLIENT_SECRET)?.ConfigValue;
            //var eContractAccountUserName = sysConfigCache.GetViaKey(CONFIG_KEY_ECONTRACT_ACCOUNT_USERNAME)?.ConfigValue;
            //var eContractAccountPassword = sysConfigCache.GetViaKey(CONFIG_KEY_ECONTRACT_ACCOUNT_PASSWORD)?.ConfigValue;

            var eContractHost = "https://apigateway-econtract-poc.vnptit3.vn";
            var eContractClientId = "4201642981.client@econtract.vnpt.vn";
            var eContractSecret = "cN2juxPy6g0pNnXFzmOj2hYDHQ7xBnfX";
            var eContractAccountUserName = "4201642981_poc";
            var eContractAccountPassword = "BmEhtf9C";

            string errMsg;
            string userId = "b43de65d-35c3-4821-b988-5519433eb5fa";

            #endregion

            #region 1. Test Auth Token

            //var authTokenModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthModel
            //{
            //    ClientId = eContractClientId,
            //    ClientSecret = eContractSecret,
            //    GrantType = ConstsAuthGrantTypes.CLIENT_CREDENTIALS,
            //}, out errMsg);

            #endregion

            #region 2. Test Auth User Token

            //var userAuthTokenModel = EContractServiceProvider.AuthToken(eContractHost, new ReqAuthUserModel
            //{
            //    ClientId = "clientapp",
            //    ClientSecret = "password",
            //    GrantType = ConstsAuthGrantTypes.PASSWORD,
            //    UserName = eContractAccountUserName,
            //    Password = eContractAccountPassword,
            //    Domain = "econtract-poc.vnptit3.vn"
            //}, out errMsg);

            //var accessToken = "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX25hbWUiOiI0MjAxNjQyOTgxX3BvYyIsInBhY2thZ2VFeHBpcmVkIjpudWxsLCJyb2xlcyI6WyJFU08yMDAxIiwiRVNPNDAwNCIsIkVTTzIwMDIiLCJFU080MDAzIiwiRVNPMDAwMSIsIkVTTzIwMDMiLCJFU08wMDAyIiwiRVNPMjAwNCIsIkVTTzQwMDUiLCJFU08wMDAzIiwiRVNPMjAwNSIsIklBTTAyMDIiLCJJQU0wMjAxIiwiRVNPMzAwMSIsIkVTTzIwMTIiLCJFU08zMDAzIiwiRUNUMjMwMSIsIkVTTzMwMDIiLCJFU08xMDAyIiwiRVNPMzAwNSIsIkVTTzMwMDQiLCJEU0lHTiIsIkVDVDIzMDAiLCJFU0lHTiIsIkNNVDAwMDEiLCJFU080MDAyIiwiRVNPNDAwMSJdLCJhdXRob3JpdGllcyI6WyJFU08yMDAxIiwiRVNPNDAwNCIsIkVTTzIwMDIiLCJFU080MDAzIiwiRVNPMDAwMSIsIkVTTzIwMDMiLCJFU08wMDAyIiwiRVNPMjAwNCIsIkVTTzQwMDUiLCJFU08wMDAzIiwiRVNPMjAwNSIsIklBTTAyMDIiLCJJQU0wMjAxIiwiRVNPMzAwMSIsIkVTTzIwMTIiLCJFU08zMDAzIiwiRUNUMjMwMSIsIkVTTzMwMDIiLCJFU08xMDAyIiwiRVNPMzAwNSIsIkVTTzMwMDQiLCJEU0lHTiIsIkVDVDIzMDAiLCJFU0lHTiIsIkNNVDAwMDEiLCJFU080MDAyIiwiRVNPNDAwMSJdLCJvcmdJZCI6ImI0M2RlNjVkLTM1YzMtNDgyMS1iOTg4LTU1MTk0MzNlYjVmYSIsImNsaWVudF9pZCI6ImNsaWVudGFwcCIsImlkVXNlciI6ImI0M2RlNjVkLTM1YzMtNDgyMS1iOTg4LTU1MTk0MzNlYjVmYSIsImF1ZCI6WyJ0ZW1wbGF0ZSIsImVzb2x1dGlvbiIsImlhbSIsImRzaWduIiwiZXNpZ24iLCJyZXN0c2VydmljZSJdLCJzY29wZSI6W10sImRvbWFpbiI6ImVjb250cmFjdC1wb2Mudm5wdGl0My52biIsInBhY2thZ2VOYW1lIjpudWxsLCJleHAiOjE3MDk1NTQ1OTAsInBhcnR5SWQiOiJiNDNkZTY1ZC0zNWMzLTQ4MjEtYjk4OC01NTE5NDMzZWI1ZmEiLCJqdGkiOiJiNWJhMzE0MS1iMTA4LTQwY2ItOGMyZi03NjEyOTQ5MTEyY2QiLCJrZXkiOiItLS0tLUJFR0lOIFBSSVZBVEUgS0VZLS0tLS1cbk1JSUNlQUlCQURBTkJna3Foa2lHOXcwQkFRRUZBQVNDQW1Jd2dnSmVBZ0VBQW9HQkFOZTFiakNucEE3ZEY3bnNcbnhLTEZOQTREMUN5QytsRjF3aUhPUGJMNTZMUmNhTWVseTVNNmZDaWNuQm1tZDZ0Q0cxcFV1bis2VTdxa0U5OWNcblRmVjNGNFZRbVNVVytzZjNDNGtwUlZMelIzNmNJNlpNUWtldkZuNVVqUnJCRitKWkd3Z2diTWQybnlwQnFkV0pcblhmWE95NU03VjErN1hHazE1TEpsdVI0eGtXTDFBZ01CQUFFQ2dZRUFqb3RLWWNTbVdWd3BUYWYwMlV0UDl1TDRcbjF1Rkc0WVhiMzlNV1dxdGk1NjBaWUxWakxjdThPR29saloyLy91QVVpMERxYlhXSDF4S09QMUFVQkYzS29BdXhcbnVraWVnWG82ZmVUbEJMcEZaSXpkYVR2N1JDUUMxOEo1MDRpZ3ozV0lmN1UwVGpwK2JDNmFZMEtnTGlxK2NPYUZcbkIyQ1JKWEUvWWJ1cXo5ODNmeEVDUVFEMER0aU1URVVQWjI0Z214U2trRUFpTndDeWFQeE9jS3ZMbElwSCsveFpcbnE5M0hVRVNTc2JPbVlKT2V2MzVIK2dnMDJBMGZyRHJkUXhyN3RVeVdyTUlqQWtFQTRrTjR4RktSbTRGeElKK1hcbi91RU4vZE56OW5ZekFSVVNVZ0wwd2tsMkJ0dmdQR0lUdUhjSkRoemRsaVBqZm9RRHViQTFYNzcraTkwNnFYbEpcbnJ4YmNCd0pCQU5adkROa0lVazFjaFNoazJkaFZYZVF5QzR5Mkxha0YyZ3YvTVJoYVVMakJCeFdPY3hHb3pMM01cbmNTZXovTkprM3p6KzcxajZ6S1dIeG5lT0xnTGRPNTBDUURrM3NNbU15OCtVVzBSUnQ0RTM3bTdhMHo5blFweXFcbmRIaVMwTXgxQVVqWXY0cmxqbkVlZ1FhMW0vK0UwZG5EanFHZGd0SFVuZEJHd0xmc3VRcHk4RzhDUVFDL0paV2dcbmZ3Q2t2dVMzKzRGU3VaemtYS1FPR3paRStHQ21maThsNXZSaUNESUdjb2IrT0lHMHJxMno0MUFiM0l6NFdHeGZcblZPZlQxZy96OVF2eU5NZ1Bcbi0tLS0tRU5EIFBSSVZBVEUgS0VZLS0tLS1cbiIsImNyZWF0ZURhdGUiOjE3MDk1NDczOTA5ODF9.Ahzi_a_HsEjuDkkMeiyuYJS5XlXMq2JLeGOHAYtvShaVs64qyliM_2ztJ20wlK8K3TuuoZgatmHGn-HFKyyQDFw38dj8GnYCEZTRvzqQjC7DliHjv3MSNELQ1hNKkqAUk5rOhdzXr5Rh6wYiP-88zNCLi7ptRgY5p2vL62by2VyjsDhySvG-dQ6ObCaxbUileWZYoNDVliUslrRz6YL6xHC7Q-8Tzkvdp7OcM_60V9-VPoMCO9wjRtWJje1uKF61N7VALx1zZXxatknVnIASEK6DAQF2yY_VIQG17qufpMAbFLiVPgpMH0f6NnsqKB88DAMNUc0boghCWHhmzkmDtA";

            var accessToken = "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX25hbWUiOiI0MjAxNjQyOTgxX3BvYyIsInBhY2thZ2VFeHBpcmVkIjpudWxsLCJyb2xlcyI6WyJFU08yMDAxIiwiRVNPNDAwNCIsIkVTTzIwMDIiLCJFU080MDAzIiwiRVNPMDAwMSIsIkVTTzIwMDMiLCJFU08wMDAyIiwiRVNPMjAwNCIsIkVTTzQwMDUiLCJFU08wMDAzIiwiRVNPMjAwNSIsIklBTTAyMDIiLCJJQU0wMjAxIiwiRVNPMzAwMSIsIkVTTzIwMTIiLCJFU08zMDAzIiwiRUNUMjMwMSIsIkVTTzMwMDIiLCJFU08xMDAyIiwiRVNPMzAwNSIsIkVTTzMwMDQiLCJEU0lHTiIsIkVDVDIzMDAiLCJFU0lHTiIsIkNNVDAwMDEiLCJFU080MDAyIiwiRVNPNDAwMSJdLCJhdXRob3JpdGllcyI6WyJFU08yMDAxIiwiRVNPNDAwNCIsIkVTTzIwMDIiLCJFU080MDAzIiwiRVNPMDAwMSIsIkVTTzIwMDMiLCJFU08wMDAyIiwiRVNPMjAwNCIsIkVTTzQwMDUiLCJFU08wMDAzIiwiRVNPMjAwNSIsIklBTTAyMDIiLCJJQU0wMjAxIiwiRVNPMzAwMSIsIkVTTzIwMTIiLCJFU08zMDAzIiwiRUNUMjMwMSIsIkVTTzMwMDIiLCJFU08xMDAyIiwiRVNPMzAwNSIsIkVTTzMwMDQiLCJEU0lHTiIsIkVDVDIzMDAiLCJFU0lHTiIsIkNNVDAwMDEiLCJFU080MDAyIiwiRVNPNDAwMSJdLCJvcmdJZCI6ImI0M2RlNjVkLTM1YzMtNDgyMS1iOTg4LTU1MTk0MzNlYjVmYSIsImNsaWVudF9pZCI6ImNsaWVudGFwcCIsImlkVXNlciI6ImI0M2RlNjVkLTM1YzMtNDgyMS1iOTg4LTU1MTk0MzNlYjVmYSIsImF1ZCI6WyJ0ZW1wbGF0ZSIsImVzb2x1dGlvbiIsImlhbSIsImRzaWduIiwiZXNpZ24iLCJyZXN0c2VydmljZSJdLCJzY29wZSI6W10sImRvbWFpbiI6ImVjb250cmFjdC1wb2Mudm5wdGl0My52biIsInBhY2thZ2VOYW1lIjpudWxsLCJleHAiOjE3MDk3MDEwMTgsInBhcnR5SWQiOiJiNDNkZTY1ZC0zNWMzLTQ4MjEtYjk4OC01NTE5NDMzZWI1ZmEiLCJqdGkiOiJmMTYyODBlZC0zOTVhLTRhZGItOGZmOS1mOWY1Y2YwZjFkNjciLCJrZXkiOiItLS0tLUJFR0lOIFBSSVZBVEUgS0VZLS0tLS1cbk1JSUNlQUlCQURBTkJna3Foa2lHOXcwQkFRRUZBQVNDQW1Jd2dnSmVBZ0VBQW9HQkFOZTFiakNucEE3ZEY3bnNcbnhLTEZOQTREMUN5QytsRjF3aUhPUGJMNTZMUmNhTWVseTVNNmZDaWNuQm1tZDZ0Q0cxcFV1bis2VTdxa0U5OWNcblRmVjNGNFZRbVNVVytzZjNDNGtwUlZMelIzNmNJNlpNUWtldkZuNVVqUnJCRitKWkd3Z2diTWQybnlwQnFkV0pcblhmWE95NU03VjErN1hHazE1TEpsdVI0eGtXTDFBZ01CQUFFQ2dZRUFqb3RLWWNTbVdWd3BUYWYwMlV0UDl1TDRcbjF1Rkc0WVhiMzlNV1dxdGk1NjBaWUxWakxjdThPR29saloyLy91QVVpMERxYlhXSDF4S09QMUFVQkYzS29BdXhcbnVraWVnWG82ZmVUbEJMcEZaSXpkYVR2N1JDUUMxOEo1MDRpZ3ozV0lmN1UwVGpwK2JDNmFZMEtnTGlxK2NPYUZcbkIyQ1JKWEUvWWJ1cXo5ODNmeEVDUVFEMER0aU1URVVQWjI0Z214U2trRUFpTndDeWFQeE9jS3ZMbElwSCsveFpcbnE5M0hVRVNTc2JPbVlKT2V2MzVIK2dnMDJBMGZyRHJkUXhyN3RVeVdyTUlqQWtFQTRrTjR4RktSbTRGeElKK1hcbi91RU4vZE56OW5ZekFSVVNVZ0wwd2tsMkJ0dmdQR0lUdUhjSkRoemRsaVBqZm9RRHViQTFYNzcraTkwNnFYbEpcbnJ4YmNCd0pCQU5adkROa0lVazFjaFNoazJkaFZYZVF5QzR5Mkxha0YyZ3YvTVJoYVVMakJCeFdPY3hHb3pMM01cbmNTZXovTkprM3p6KzcxajZ6S1dIeG5lT0xnTGRPNTBDUURrM3NNbU15OCtVVzBSUnQ0RTM3bTdhMHo5blFweXFcbmRIaVMwTXgxQVVqWXY0cmxqbkVlZ1FhMW0vK0UwZG5EanFHZGd0SFVuZEJHd0xmc3VRcHk4RzhDUVFDL0paV2dcbmZ3Q2t2dVMzKzRGU3VaemtYS1FPR3paRStHQ21maThsNXZSaUNESUdjb2IrT0lHMHJxMno0MUFiM0l6NFdHeGZcblZPZlQxZy96OVF2eU5NZ1Bcbi0tLS0tRU5EIFBSSVZBVEUgS0VZLS0tLS1cbiIsImNyZWF0ZURhdGUiOjE3MDk2OTM4MTgwOTZ9.c0zvCzKs9w09ayQAR2v7mD_TVwwtZmDgkdt5heJc6rqLyIFZRMILynHsntIpbwnRGZDmDR3NVest_VFc9_Sx8RwX1AOhK6S4UVP5PU5XBbtacL29sTEjrA8R2qdcLcuBokiqtcrkUAyQ-JPI3JezeeMtU55AkzIr5epDupYydM-EKBdCPBolwRVYoCa5rgN6iSlS7ClcV1lGEqnC8k75sdRzqS90RDpboD2BBscETgFIhQ1oRw-qIGD6HFMfQU3iIDt9VQwke_DbyAKtZ51Wztrsj_qgNzU6hLW-PoMNhUcTzCLOITKumpLl6vLJ3uQfflmlOgm5YuuQ2FkJQNeVoA";

            #endregion

            #region 3. Lấy danh sách mẫu hợp đồng

            //var reqTemplateContract =
            //    EContractServiceProvider.GetContractTemplates(eContractHost, accessToken, new ReqSearchModel(),
            //        out errMsg);

            #endregion

            #region 4. Chi tiết hợp đồng mẫu

            //var resDetailTemplateContract =
            //    EContractServiceProvider.GetDetailTemplateContract(eContractHost, accessToken, "65e14c84fae838f98c19178d",
            //        out errMsg);

            #endregion

            #region 5. Render hợp đồng từ  mẫu hợp đồng

            //var dataFields =
            //    "{\n  \"${maTinh}\": \"15\",\n  \"${ngayKy}\": \"10\",\n  \"${thangKy}\": \"11\",\n  \"${namKy}\": \"2021\",\n  \"${sttHD}\": \"000000001\",\n  \"${tenToChucA}\": \"THIEN\",\n  \"${maDoiTacA}\": \"ABC\",\n  \"${nganhNgheA}\": \"CNTT\",\n  \"${dkkdA}\": \"ABC/186\",\n  \"${mstA}\": \"123456789-123\",\n  \"${ngayDkkdA}\": \"29/05/2021\",\n  \"${noiCapDkkdA}\": \"Đa\u0300  Nẵng\",\n  \"${diaChiA}\": \"186 hoang diê\u0323 u, ha\u0309 i châu đa\u0300  nẵng\",\n  \"!{xuatHoaDon}\": [\n    2\n  ],\n  \"${mstB}\": \"987654321\",\n  \"${diaChiB}\": \"186 hoang diê\u0323 u, ha\u0309 i châu đa\u0300  nẵng\",\n  \"${daiDienB}\": \"Đinh Tấn Thiê\u0323 n\",\n  \"${chucVuB}\": \"Giám đô\u0301 c\",\n  \"${soGiayUyQuyenB}\": \"ABC/0948858109\",\n  \"${ngayCapUyQuyenB}\": \"29/05/2020\",\n  \"#ds\": [\n    {\n      \"#ds{chuTaiKhoan}\": \"Đinh Tấn Thiê\u0323 n\",\n      \"#ds{diaChi}\": \"186 hoang diê\u0323 u, ha\u0309 i châu đa\u0300  nẵng\",\n      \"#ds{emailLienHe}\": \"thiendt@vnpt.vn\",\n      \"#ds{loaiHinhKD}\": \"Cá nhân\",\n      \"#ds{nganHangTaiKhoan}\": \"SEAbank\",\n      \"#ds{sdtDangKyUngDung}\": \"0948858109\",\n      \"#ds{sdtLienHe}\": \"0948858109\",\n      \"#ds{stt}\": \"1\",\n      \"#ds{tenDCN}\": \"Đinh Tấn Thiê\u0323 n\",\n      \"#ds{tenLienHe}\": \"Đinh Tấn Thiê\u0323 n\",\n      \"#ds{tkHuongThu}\": \"0948858109\"\n    },\n    {\n      \"#ds{chuTaiKhoan}\": \"Đinh Tấn Thiê\u0323 n\",\n      \"#ds{diaChi}\": \"186 hoang diê\u0323 u, ha\u0309i  châu đa\u0300  nẵng\",\n      \"#ds{emailLienHe}\": \"thiendt@vnpt.vn\",\n      \"#ds{loaiHinhKD}\": \"Cá nhân\",\n      \"#ds{nganHangTaiKhoan}\": \"SEAbank\",\n      \"#ds{sdtDangKyUngDung}\": \"0948858109\",\n      \"#ds{sdtLienHe}\": \"0948858109\",\n      \"#ds{stt}\": \"2\",\n      \"#ds{tenDCN}\": \"Đinh Tấn Thiê\u0323 n\",\n      \"#ds{tenLienHe}\": \"Đinh Tấn Thiê\u0323 n\",\n      \"#ds{tkHuongThu}\": \"0948858109\"\n    }\n  ]\n}";

            //var resRenderTemplateContract =
            //    EContractServiceProvider.RenderContract(eContractHost, accessToken, "65e14c84fae838f98c19178d", dataFields,
            //        out errMsg, out var statusCode);

            #endregion

            #region 6. Lấy tọa độ danh sách các biến vị trí chữ ký 

            //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Datas\1.pdf");
            //byte[] dataFile = File.ReadAllBytes(path);

            //var resDetailTemplateContract =
            //    EContractServiceProvider.GetListPosition(eContractHost, accessToken, new MemoryPostedFile(dataFile, "1.pdf", ConstsContentTypes.FILE_PDF),
            //        out errMsg);

            #endregion

            #region 7. Danh sách luồng hợp đồng 

            //var resListFlowContract =
            //    EContractServiceProvider.GetListFlowContract(eContractHost, accessToken, new ReqListFlowContractModel
            //        {
            //            Disable = "0",
            //            Discuss = "1",
            //            MaxSize = 100,
            //            Page = 1,
            //            Sort = "ASC"
            //        },
            //        out errMsg);

            #endregion

            #region 8. Chi tiết luồng hợp đồng

            //var resDetailFlowContact =
            //    EContractServiceProvider.GetDetailFlowContact(eContractHost, accessToken, "3996a480-b5e7-46bf-b72a-a0f336247488",
            //        out errMsg);

            #endregion

            #region 9. Tạo hợp đồng (không kèm thông tin định danh):

            //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Datas\1.pdf");
            //byte[] dataFile = File.ReadAllBytes(path);

            //var resCreateContract =
            //    EContractServiceProvider.CreateContract(eContractHost, accessToken, new ReqContractModel
            //    {
            //        AttachFile = new MemoryPostedFile(dataFile, "1.pdf", ConstsContentTypes.FILE_PDF),
            //        Customer = "{\"cmnd\":\"CCCD\",\"email\":\"0966147612@gmail.com\",\"mst\":\"0945450012\",\"sdt\":\"0911578055\",\"signFrame\":[{\"x\":116,\"y\":678,\"w\":200,\"h\":80,\"page\":5}],\"loaiGtId\":\"0\",\"ten\":\"Đào Bích Phương\",\"tenToChuc\":\"CenIT\",\"userType\":\"BUSINESS\"}",

            //        Contract = "{\"contractValue\":\"0\",\"creationNote\":\"\",\"flowTemplateId\":\"2b009094-696a-4efa-813f-cd8f69ba54ed\",\"orgTemplateId\":\"62185e34036e5e377d3b35dd\",\"productId\":\"620e1486acad1bac4582d8c4\",\"sequence\":1,\"signFlow\":[{\"signType\":\"DRAFT\",\"departmentId\":\"3f558a33-9df0-4f33-96a0-b5159fc7c274\",\"userId\":\"fdd81165-81e1-4dc7-841f-fb76740ccdf4\",\"sequence\":2,\"limitDate\":1,\"signForm\":[\"EKYC\",\"USB_TOKEN\",\"SIGN_SERVER\",\"SMART_CA\",\"OTP\"],\"signFrame\":[{\"x\":116,\"y\":678,\"w\":200,\"h\":80,\"page\":5}]},{\"signType\":\"APPROVAL\",\"departmentId\":\"b0eb8870-a4e7-44ca-9468-d49dd870fe90\",\"userId\":\"ec631102-98cc-4fd3-9f00-b5c5971acb92\",\"sequence\":1,\"limitDate\":3,\"signForm\":[\"EKYC\",\"USB_TOKEN\",\"SIGN_SERVER\",\"SMART_CA\",\"OTP\"],\"signFrame\":[{\"x\":116,\"y\":678,\"w\":200,\"h\":80,\"page\":5}]}],\"signForm\":[\"OTP\",\"EKYC\",\"EKYC_EMAIL\",\"OTP_EMAIL\",\"NO_AUTHEN\"],\"templateId\":\"622ef246eae29e88f40ebb93\",\"title\":\"HĐ đăng kí Điểm kinh doanh Mobile Money của Test PNC 0945450093 - Tên Hiển thị\",\"validDate\":\"2022-03-23\",\"verificationType\":\"NONE\",\"signFlowType\":\"REQUIRE_FLOW\",\"internalDiscussType\":\"NO_REQUIRE\"}",
            //        Fields = ""
            //    }, out errMsg);

            #endregion

            #region 10. Gửi hợp đồng

            //var resDetailFlowContact =
            //    EContractServiceProvider.SubmitContract(eContractHost, accessToken, "da3d2a81-3bf3-4595-9a11-a21be7ddb6ae",
            //        out errMsg);

            #endregion

            #region 11. Upload và cập nhật trạng thá i file hợp đồng sau khi ký số/ ký điện tử

            //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Datas\1.pdf");
            //byte[] dataFile = File.ReadAllBytes(path);

            //var resDetailFlowContact =
            //    EContractServiceProvider.SignDigitalContract(eContractHost, accessToken, "da3d2a81-3bf3-4595-9a11-a21be7ddb6ae", new ReqDigitalSignModel
            //    {
            //        AttachFile = new MemoryPostedFile(dataFile, "1.pdf", ConstsContentTypes.FILE_PDF),
            //        SignInfo = new SignInfoModel
            //        {
            //            SignForm = "USB_TOKEN"
            //        }
            //    }, out errMsg);

            #endregion

            #region 12. Danh sách hợp đồng gửi

            //var resDetailFlowContact =
            //    EContractServiceProvider.GetListSentContracts(eContractHost, accessToken, userId, new ReqContractSentModel
            //        {
            //            Page = 1, MaxSize = 100, Sort = "ASC", PropertiesSort = "-modified"
            //    }, out errMsg);

            #endregion

            #region 13. Danh sách hợp đồng nhận

            //var resListReceiveContracts =
            //    EContractServiceProvider.GetListReceiveContracts(eContractHost, accessToken, userId, new ReqContractSentModel
            //    {
            //        Page = 1,
            //        MaxSize = 100,
            //        Sort = "ASC",
            //        PropertiesSort = "-modified"
            //    }, out errMsg);

            #endregion

            #region 14. Chi tiết hợp đồng

            //var resListReceiveContracts =
            //    EContractServiceProvider.GetDetailContract(eContractHost, accessToken, "730bb394-8576-4110-899e-823abb027c43", out errMsg);

            #endregion

            #region 15. Tải hợp đồng điện tử

            var resListReceiveContracts =
                EContractServiceProvider.DownloadContract(eContractHost, accessToken, new ReqContractDownloadModel
                {
                    DocumentType = "DRAFT",
                    DocumentHash = "b47a7dce33a718013677122c96c795945e84a1fd781a6051664236d9e361ef0d",
                    ContractId = "f817d12c-07e0-4567-878e-ddab0b29bd36"
                }, out errMsg, out var responseCode );

            #endregion

            #region 18. Danh sá ch bộ phận 

            //var resListReceiveContracts =
            //    EContractServiceProvider.GetListDepartments(eContractHost, accessToken, new ReqSearchDepartmentModel
            //    {
            //        MaxSize = 100, Page = 1, PropertiesSort = "-modified", Status = 1
            //    }, out errMsg);

            #endregion

            #region 20. Danh sá ch loại hợp đồng   

            //var resListReceiveContracts =
            //    EContractServiceProvider.GetListContractTypes(eContractHost, accessToken, new ReqSearchContractTypeModel
            //    {
            //        MaxSize = 100,
            //        Page = 1,
            //        PropertiesSort = "-modified",
            //        Status = "N"
            //    }, out errMsg);

            #endregion
        }
    }

    public class MemoryPostedFile : HttpPostedFileBase
    {
        private readonly byte[] _fileBytes;

        public MemoryPostedFile(byte[] fileBytes, string fileName = null, string contentType = null)
        {
            _fileBytes = fileBytes;
            FileName = fileName;
            InputStream = new MemoryStream(fileBytes);
            ContentType = contentType;
        }

        public override int ContentLength => _fileBytes.Length;

        public override string FileName { get; }

        public override Stream InputStream { get; }

        public override string ContentType { get; }
    }
}
