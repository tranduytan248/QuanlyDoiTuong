namespace Cores.eContract.Consts
{
    /// <summary>
    /// Danh sách các api của eContract
    /// </summary>
    public class ConstsEContractServices
    {
        /// <summary>
        /// 01.Xác thực, định danh third party 
        /// 02.Đăng nhập với tài khoản eContract để sinh token xem và ký HĐ
        /// </summary>
        public const string OAUTH_SERVICE = "/auth-service/oauth/token";
        /// <summary>
        /// 03.Danh sá ch mẫu hợp đồng
        /// </summary>
        public const string TEMPLATE_SERVICE_LIST = "/template-service/api/templates/app/list";
        /// <summary>
        /// 04.Thông tin chi tiết hợp đồng mẫu để  tạo hợp đồng
        /// </summary>
        public const string TEMPLATE_SERVICE_DETAIL_TEMPLATE = "/template-service/api/templates/v1/{templateId}/all-config";
        /// <summary>
        /// 05.Đổ dữ liệu vào mẫu hợp đồng 
        /// </summary>
        public const string TEMPLATE_SERVICE_RENDER = "/template-service/api/templates/{templateId}/render";
        /// <summary>
        /// 06.Danh sách tọa độ các biến vị trí trong file PDF: tìm danh sách các chữ @{1}, @{2},….
        /// </summary>
        public const string ESIGNATURE_SERVICE_LIST_POSITION = "/esignature-service/api/list-position";
        /// <summary>
        /// 07.Danh sách luồng hợp đồng
        /// </summary>
        public const string ESIGNATURE_SERVICE_CONTRACT_FLOW = "/esolution-service/contract/flow/list";
        /// <summary>
        /// 08.Chi tiết luồng hợp đồng
        /// </summary>
        public const string ESIGNATURE_SERVICE_CONTRACT_FLOW_DETAIL = "/esolution-service/contract/flow/detail/{contractFlowTemplateId}";
        /// <summary>
        /// 09.Tạo hợp đồng từ file pdf hợp đồng 
        /// </summary>
        public const string ESIGNATURE_SERVICE_CONTRACT_CREATE = "/esolution-service/contracts/create-draft-from-file-raw";
        /// <summary>
        /// 10.Gửi hợp đồng cho nội bộ hoặc đối tác theo luồng HĐ đã cấu hình. 
        /// </summary>
        public const string ESIGNATURE_SERVICE_CONTRACT_SUBMIT = "/esolution-service/contracts/{contractId}/submit-contract";
        /// <summary>
        /// 11.Upload và cập nhật trạng hợp đồng sau các bước duyệt/ký nháy/ký duyệt 
        /// </summary>
        public const string ESIGNATURE_SERVICE_CONTRACT_SIGN = "/esolution-service/contracts/{contractId}/digital-sign";
        /// <summary>
        /// 12.Danh sách hợp đồng gửi
        /// </summary>
        public const string ESIGNATURE_SERVICE_CONTRACT_LIST_SENT = "/esolution-service/user-informations/{userId}/v1/contract-list-by-owner";
        /// <summary>
        /// 13.Danh sách hợp đồng nhận 
        /// </summary>
        public const string ESIGNATURE_SERVICE_CONTRACT_LIST_RECEIVE = "/esolution-service/user-informations/{userId}/v1/contract-list-by-assign";
        /// <summary>
        /// 14.Xem thông tin chi tiết hợp đồng 
        /// </summary>
        public const string ESIGNATURE_SERVICE_CONTRACT_DETAIL = "/esolution-service/contracts/{contractId}";
        /// <summary>
        /// 15.Tải hợp đồng  
        /// </summary>
        public const string ESIGNATURE_SERVICE_CONTRACT_DOWNLOAD = "/esignature-service/dsign/esolution/download";
        /// <summary>
        /// 16.Huỷ hợp đồng
        /// </summary>
        public const string ESIGNATURE_SERVICE_CONTRACT_CANCEL = "/esolution-service/contracts/{contractId}/cancel-draft";
        /// <summary>
        /// 17.Xoá hợp đồng
        /// </summary>
        public const string ESIGNATURE_SERVICE_CONTRACT_DELETE = "/esolution-service/contracts/{contractId}/delete-draft";
        /// <summary>
        /// 18.Lấy danh sách bộ phận phân của tổ chức theo cây đơn vị
        /// </summary>
        public const string ESIGNATURE_SERVICE_DEPARTMENT_SEARCH = "/esolution-service/parties/departments";
        /// <summary>
        /// 19.Lấy danh sách bộ phận phân của tổ chức theo cây đơn vị
        /// </summary>
        public const string ESIGNATURE_SERVICE_DEPARTMENT_EMPLOYEE_SEARCH = "/esolution-service/parties/{partyId}/list-employee?sta-tus=&keySearch=&page=1&maxSize=10&sort=&propertiesSort=";
        /// <summary>
        /// 20.Lấy danh sách loại hợp đồng của tổ chức
        /// </summary>
        public const string ESOLUTION_SERVICE_CONTRACT_TYPES = "/esolution-service/contract-types";
        /// <summary>
        /// 21.Tạo hợp đồng (kèm thông tin định danh)
        ///  Tạo hợp đồng từ file pdf hợp đồng 
        /// </summary>
        public const string ESOLUTION_SERVICE_CREATE_CONTRACT_WITH_IDENTIFICATION =
            "/esolution-service/contracts/create-draft-from-file-and-identification";
        /// <summary>
        /// 22.Tạo hợp đồng (ký 1 lần)
        /// Tạo hợp đồng từ file pdf hợp đồng, đối tác Cá Nhân vào thực hiện ký 1 lần  
        /// </summary>
        public const string ESOLUTION_SERVICE_CREATE_CONTRACT_SIGN_ONE_TIME = "/esolution-service/contracts/create-draft-sign-one-time";
    }
}