namespace CenIT.Lib.SMSBrandName.Helpers
{
    public static class EnumHelper
    {
        public static string GetErrorSendSMS(string err)
        {
            string errDesc = "No Error Desc";
            switch (err)
            {
                case "-1":
                    errDesc = "Exception - Request chứa 5 ký tự đặc biệt của XML, hoặc dữ liệu dài quá, hoặc lỗi nội bộ";
                    break;
                case "0":
                    errDesc = "Success";
                    break;
                case "1":
                    errDesc = "Username, password, IP, status các API không hợp lệ: Liên hệ media(Hoàn DD) để kiểm tra username, password của API, đồng thời kiểm tra IP phía server nhận được nếu cần";
                    break;
                case "2":
                    errDesc = "Thời gian đặt lịch sai định dạng Đúng dd-MM-yyyy HH:mi Ví dụ 31 - 08 - 2018 15:00";
                    break;
                case "8":
                    errDesc = "Sai thời gian quy định đối với tin nhắn QC. Không được phép gửi quảng cáo ngoài các khung giờ: -800 1130 - 1300 1830- 2000 2100";
                    break;
                case "3":
                    errDesc = "ID method không hợp lệ";
                    break;
                case "7":
                    errDesc = "Template không hợp lệ hoặc không tồn tại với nhãn và đại lý Kiểm tra ID template trên portal Các tham số agent, contract, label, template, nếu không trùng nhau sẽ báo lỗi này.Nếu trùng mà status = 0 thì mới ra lỗi khác tương ứng";
                    break;
                case "9":
                    errDesc = "Contract_type_id không hợp lệ CSKH = 1 hoặc QC = 2";
                    break;
                case "10":
                    errDesc = "User_name không hợp lệ (user đăng nhập của Agent trên portal không đúng)";
                    break;
                case "11":
                    errDesc = "Độ dài tin nhắn không hợp lệ Độ dài tin nhắn hiện tại khai báo them độ dài của agent";
                    break;
                case "12":
                    errDesc = "Thời gian không hợp lệ với chính sách của VinaphoneTài liệu API dịch vụ SMS Marketing ";
                    break;
                case "13":
                    errDesc = "Hợp đồng không đúng Các tham số agent, contract, label, template, trùng nhau nhưng status hợp đồng = 0";
                    break;
                case "14":
                    errDesc = "Label không hợp lệ Tham số label_id ko hợp lệ hoặc Nhãn chưa được actived";
                    break;
                case "15":
                    errDesc = "Agent không hợp lệ Tham số agent_id k hợp lệ hoặc Agent chưa được actived";
                    break;
                case "16":
                    errDesc = "Quá tốc độ gửi tin cho phép Dự phòng";
                    break;
                case "17":
                    errDesc = "Định dạng ký tự không hợp lệ Truyền sai tham số dataencoding hoặc nôi dung truyền vào sai chuẩn encoding";
                    break;
                case "20":
                    errDesc = "Hết gói tin của hợp đồng Các đại lý tự mở thêm hạn mức nếu còn";
                    break;
                case "21":
                    errDesc = "Hết gói tin của khách hàng Các đại lý tự mở thêm hạn mức nếu còn";
                    break;
                case "22":
                    errDesc = "Hết gói tin của đại lý Liên hệ VNP Admin để được cấp gói";
                    break;
                case "23":
                    errDesc = "Gửi nhiều mạng trong một lệnh gửi tin hoặc số điện thoại không hợp lệ Mỗi request chỉ được gửi 1 mạng và số điện thoại phải hợp lệ";
                    break;
                case "24":
                    errDesc = "Thời gian đặt lịch sớm hơn thời gian hiện tại của hệ thống Cho phép gửi trước 1 ngày";
                    break;
                case "25":
                    errDesc = "sai mạng, mạng đúng [telco chuyển], lable không hợp lệ Thuê bao đã được chuyển sang mạng khác và nhãn chưa được khai ở mạng này. Quy định về telco: 1- Vinaphone 2- Mobifone 3- Viettel 4- Gtel 5- Vietnamobile";
                    break;
                case "26":
                    errDesc = "thue bao da nhan 3 request gui tin nhan QC trong ngay Thuê bao đã nhận 3 tin nhắn Quảng Cáo từ SMSMKT";
                    break;
                case "27":
                    errDesc = "giá trị truyền vào tham biến [vị trí tham biến] không đúng. VD: Template có param nhưng truyền nội dung vào param kTài liệu API dịch vụ SMS Marketinghợp lệ với khai báo (Chưa áp dụng)";
                    break;
                case "28":
                    errDesc = "số lượng ký tự tham biến [vị trí tham biến] vượt quá hạn mức khai báo Truyền nội dung param vượt quá số kí tự đã khai báo khi tạo template (Chưa áp dụng)";
                    break;
                case "29":
                    errDesc = "Brandname hết hiệu lực Gửi tin trong trường hợp brandname (label) bị hết hạn.";
                    break;
                case "30":
                    errDesc = "msg has illegal keyword Trong tin nhắn gửi tin có chứa từ khóa vi phạm";
                    break;
                case "31":
                    errDesc = "trang thai adser ko hop le Trạng thái của k/h không hợp lệ ví dụ k hợp động";
                    break;
                case "33":
                    errDesc = "Gửi trùng request id Chỉ áp dụng api bank";
                    break;
                default:
                    break;
            }

            return errDesc;
        }
    }
}