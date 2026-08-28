using System;
using System.Collections.Generic;
using System.Linq;
using Cores.Major.Models;

namespace Cores.Major.Helpers
{
    /// <summary>
    /// So sánh dữ liệu cũ và mới để xác định những trường thực sự bị thay đổi.
    /// Nếu không có trường nào thay đổi, tầng controller sẽ KHÔNG ghi xuống CSDL.
    /// </summary>
    public static class SubjectChangeDetector
    {
        /// <summary>
        /// Chuẩn hoá chuỗi trước khi so sánh: null, chuỗi rỗng và chuỗi toàn khoảng
        /// trắng đều được coi là như nhau.
        /// Nếu thiếu bước này, một ô nhập để trống (gửi lên "") sẽ luôn bị coi là
        /// khác với giá trị NULL trong CSDL, khiến việc "không có thay đổi" không
        /// bao giờ xảy ra.
        /// </summary>
        private static string Norm(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static bool IsSame(string oldValue, string newValue)
        {
            return string.Equals(Norm(oldValue), Norm(newValue), StringComparison.Ordinal);
        }

        private static void AddIfChanged(ICollection<SubjectFieldChangeModel> changes, string field,
            string label, string oldValue, string newValue)
        {
            if (IsSame(oldValue, newValue)) return;

            changes.Add(new SubjectFieldChangeModel
            {
                Field = field,
                Label = label,
                OldValue = Norm(oldValue) ?? string.Empty,
                NewValue = Norm(newValue) ?? string.Empty
            });
        }

        /// <summary>
        /// So sánh thông tin định danh của Đối tượng.
        ///
        /// Các trường CỐ Ý không đưa vào so sánh:
        ///  - BirthRegistrationPlace: không có trên giao diện và không được proc
        ///    tra cứu trả về, nếu so sánh sẽ luôn báo "có thay đổi" một cách sai lệch.
        ///  - Reporter*: luôn được xác định lại từ tài khoản đăng nhập, không phải
        ///    dữ liệu người dùng nhập.
        ///  - Các cột hệ thống: IsDeleted, Created*, Updated*, ViolationCount...
        /// </summary>
        public static List<SubjectFieldChangeModel> Diff(MajorSubjectModel oldModel, MajorSubjectModel newModel)
        {
            var changes = new List<SubjectFieldChangeModel>();
            if (oldModel == null || newModel == null) return changes;

            AddIfChanged(changes, "IdentityCardNumber", "Số CCCD / Căn cước", oldModel.IdentityCardNumber, newModel.IdentityCardNumber);
            AddIfChanged(changes, "FullName", "Họ và tên khai sinh", oldModel.FullName, newModel.FullName);
            AddIfChanged(changes, "OtherName", "Tên gọi khác", oldModel.OtherName, newModel.OtherName);

            // Ngày sinh chỉ so sánh phần ngày, bỏ qua giờ phút
            var oldDob = oldModel.DateOfBirth?.Date.ToString("dd/MM/yyyy");
            var newDob = newModel.DateOfBirth?.Date.ToString("dd/MM/yyyy");
            AddIfChanged(changes, "DateOfBirth", "Ngày, tháng, năm sinh", oldDob, newDob);

            AddIfChanged(changes, "Gender", "Giới tính", oldModel.Gender, newModel.Gender);
            AddIfChanged(changes, "Ethnicity", "Dân tộc", oldModel.Ethnicity, newModel.Ethnicity);
            AddIfChanged(changes, "Religion", "Tôn giáo", oldModel.Religion, newModel.Religion);
            AddIfChanged(changes, "Nationality", "Quốc tịch", oldModel.Nationality, newModel.Nationality);
            AddIfChanged(changes, "PhoneNumber", "Số điện thoại", oldModel.PhoneNumber, newModel.PhoneNumber);
            AddIfChanged(changes, "PlaceOfOrigin", "Quê quán", oldModel.PlaceOfOrigin, newModel.PlaceOfOrigin);
            AddIfChanged(changes, "CurrentResidence", "Nơi ở hiện tại", oldModel.CurrentResidence, newModel.CurrentResidence);
            AddIfChanged(changes, "AvatarUrl", "Ảnh chân dung", oldModel.AvatarUrl, newModel.AvatarUrl);
            AddIfChanged(changes, "IdentityCardFrontUrl", "Ảnh CCCD mặt trước", oldModel.IdentityCardFrontUrl, newModel.IdentityCardFrontUrl);
            AddIfChanged(changes, "IdentityCardBackUrl", "Ảnh CCCD mặt sau", oldModel.IdentityCardBackUrl, newModel.IdentityCardBackUrl);
            AddIfChanged(changes, "SubjectTypeIds", "Loại đối tượng", oldModel.SubjectTypeIds, newModel.SubjectTypeIds);

            return changes;
        }

        /// <summary>
        /// So sánh thông tin một lần vi phạm.
        /// Danh sách hành vi được so sánh theo TẬP HỢP (đã sắp xếp), để việc tích
        /// chọn theo thứ tự khác nhau không bị hiểu nhầm là có thay đổi.
        /// </summary>
        public static List<SubjectFieldChangeModel> DiffViolation(MajorSubjectViolationModel oldModel,
            MajorSubjectViolationModel newModel)
        {
            var changes = new List<SubjectFieldChangeModel>();
            if (oldModel == null || newModel == null) return changes;

            var oldDate = oldModel.ViolationDate.ToString("dd/MM/yyyy HH:mm");
            var newDate = newModel.ViolationDate.ToString("dd/MM/yyyy HH:mm");
            AddIfChanged(changes, "ViolationDate", "Thời gian vi phạm", oldDate, newDate);

            AddIfChanged(changes, "TreatmentMeasures", "Biện pháp xử lý", oldModel.TreatmentMeasures, newModel.TreatmentMeasures);
            AddIfChanged(changes, "RelatedDocuments", "Văn bản liên quan", oldModel.RelatedDocuments, newModel.RelatedDocuments);
            AddIfChanged(changes, "Images", "Hình ảnh vi phạm", oldModel.Images, newModel.Images);
            AddIfChanged(changes, "Notes", "Ghi chú", oldModel.Notes, newModel.Notes);

            var oldBehaviors = NormalizeIdList(oldModel.BehaviorIds);
            var newBehaviors = NormalizeIdList(newModel.BehaviorIds);
            AddIfChanged(changes, "BehaviorIds", "Hành vi vi phạm", oldBehaviors, newBehaviors);

            return changes;
        }

        /// <summary>
        /// Đưa chuỗi id phân tách bởi dấu phẩy về dạng chuẩn: loại bỏ trùng lặp,
        /// sắp xếp tăng dần, bỏ giá trị không hợp lệ.
        /// </summary>
        private static string NormalizeIdList(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids)) return null;

            var parsed = ids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(item => item.Trim())
                .Where(item => int.TryParse(item, out _))
                .Select(int.Parse)
                .Distinct()
                .OrderBy(item => item)
                .ToList();

            return parsed.Count == 0 ? null : string.Join(",", parsed);
        }
    }
}
