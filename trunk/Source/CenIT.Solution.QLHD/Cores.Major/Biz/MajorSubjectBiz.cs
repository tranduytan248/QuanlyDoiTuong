using System;
using System.Collections.Generic;
using System.Linq;
using Cores.Major.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Major.Biz
{
    public class MajorSubjectBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _majorSubjectDelete = "Major_Subject_Delete";
        private readonly string _majorSubjectGet = "Major_Subject_Get";
        private readonly string _majorSubjectGetById = "Major_Subject_GetById";
        private readonly string _majorSubjectGetAll = "Major_Subject_GetAll";
        private readonly string _majorSubjectSave = "Major_Subject_Save";

        /// <summary>
        /// Lấy danh sách đối tượng theo các tiêu chí tra cứu, đã áp dụng phân quyền dữ liệu.
        /// Tham số <paramref name="userName"/> được proc dùng để giới hạn phạm vi đơn vị và lĩnh vực.
        /// </summary>
        public List<MajorSubjectModel> Get(out int total, string identityCardNumber, string fullName,
            string behaviorIds, string gender, string userName, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "0",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var listSubjects = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorSubjectModel>(_majorSubjectGet,
                DATA_PROVIDER_NAME,
                identityCardNumber,
                fullName,
                behaviorIds,
                gender,
                userName,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (listSubjects != null && listSubjects.Count > 0)
                total = int.Parse(listSubjects.First()?.TotalRow.ToString() ?? "0");
            return listSubjects;
        }

        public List<MajorSubjectModel> GetAll()
        {
            return AppProcessor.ProcedureProvider.ExecuteTypedList<MajorSubjectModel>(_majorSubjectGetAll, DATA_PROVIDER_NAME);
        }

        /// <summary>
        /// Tra cứu đối tượng theo số CCCD/CMND (khớp chính xác).
        /// Dùng lại stored procedure Major_Subject_Get với tham số key để không phải bổ sung proc mới.
        /// </summary>
        public MajorSubjectModel GetByIdentityCardNumber(string identityCardNumber)
        {
            if (string.IsNullOrWhiteSpace(identityCardNumber)) return null;

            var cardNumber = identityCardNumber.Trim();
            var search = new BaseSearchModel
            {
                Search = null,
                Order = "0",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            // userName = null => proc bỏ qua giới hạn phạm vi, vì đây là tra cứu nội bộ
            // phục vụ việc chống trùng số CCCD khi thêm mới.
            var listSubjects = Get(out int _, cardNumber, null, null, null, null, search);
            if (listSubjects == null || listSubjects.Count == 0) return null;

            return listSubjects.FirstOrDefault(item =>
                !string.IsNullOrEmpty(item.IdentityCardNumber) &&
                item.IdentityCardNumber.Trim().Equals(cardNumber, StringComparison.OrdinalIgnoreCase));
        }

        public MajorSubjectModel GetById(Guid? subjectId)
        {
            if (!subjectId.HasValue || subjectId.Value == Guid.Empty) return null;
            return AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorSubjectModel>(_majorSubjectGetById,
                DATA_PROVIDER_NAME, subjectId.Value);
        }

        public string Save(MajorSubjectModel model, string username)
        {
            var result = AppProcessor.ProcedureProvider.ExecuteScalar(_majorSubjectSave, DATA_PROVIDER_NAME,
                model.SubjectId,
                model.IdentityCardNumber != null ? model.IdentityCardNumber.Trim() : string.Empty,
                model.FullName != null ? model.FullName.Trim() : string.Empty,
                model.OtherName,
                model.DateOfBirth,
                model.Gender,
                model.Ethnicity,
                model.Religion,
                model.Nationality,
                model.PlaceOfOrigin,
                model.IdentityCardFrontUrl,
                model.IdentityCardBackUrl,
                model.AvatarUrl,
                model.BirthRegistrationPlace,
                model.CurrentResidence,
                model.PhoneNumber != null ? model.PhoneNumber.Trim() : string.Empty,
                model.ReporterName != null ? model.ReporterName.Trim() : string.Empty,
                model.ReporterUnit != null ? model.ReporterUnit.Trim() : string.Empty,
                model.ReporterPhone != null ? model.ReporterPhone.Trim() : string.Empty,
                model.ReporterPosition != null ? model.ReporterPosition.Trim() : string.Empty,
                model.ReporterUnionId,
                username
            );

            return result?.ToString();
        }

        public bool Delete(Guid subjectId, string username)
        {
            var result = AppProcessor.ProcedureProvider.ExecuteScalar(_majorSubjectDelete, DATA_PROVIDER_NAME,
                subjectId,
                username);
            return result != null && !string.IsNullOrEmpty(result.ToString());
        }
    }
}
