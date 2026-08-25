using System;
using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using Cores.Major.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Major.Biz
{
    public class MajorSubjectViolationBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _majorSubjectViolationDelete = "Major_SubjectViolation_Delete";
        private readonly string _majorSubjectViolationGet = "Major_SubjectViolation_Get";
        private readonly string _majorSubjectViolationGetById = "Major_SubjectViolation_GetById";
        private readonly string _majorSubjectViolationGetBySubjectId = "Major_SubjectViolation_GetBySubjectId";
        private readonly string _majorSubjectViolationGetBehaviors = "Major_SubjectViolation_GetBehaviors";
        private readonly string _majorSubjectViolationSave = "Major_SubjectViolation_Save";
        private readonly string _majorSubjectViolationSaveBehaviors = "Major_SubjectViolation_SaveBehaviors";

        /// <summary>
        /// Lấy danh sách lịch sử vi phạm, đã áp dụng phân quyền dữ liệu theo
        /// đơn vị khai báo và lĩnh vực được phân công của <paramref name="userName"/>.
        /// </summary>
        public List<MajorSubjectViolationModel> Get(out int total, string key, Guid? subjectId, int? fieldId,
            string userName, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "0",
                OrderDir = "DESC",
                StartIndex = 0,
                PageSize = -1
            };

            var listViolations = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorSubjectViolationModel>(_majorSubjectViolationGet,
                DATA_PROVIDER_NAME,
                key,
                subjectId,
                fieldId,
                userName,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (listViolations != null && listViolations.Count > 0)
                total = int.Parse(listViolations.First()?.TotalRow.ToString() ?? "0");
            return listViolations;
        }

        public MajorSubjectViolationModel GetById(Guid? violationId)
        {
            if (!violationId.HasValue || violationId.Value == Guid.Empty) return null;
            return AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorSubjectViolationModel>(_majorSubjectViolationGetById,
                DATA_PROVIDER_NAME, violationId.Value);
        }

        /// <summary>
        /// Lịch sử vi phạm của một đối tượng.
        /// <paramref name="userName"/> = null nghĩa là không giới hạn phạm vi (tác vụ nội bộ).
        /// </summary>
        public List<MajorSubjectViolationModel> GetBySubjectId(Guid? subjectId, string userName = null)
        {
            if (!subjectId.HasValue || subjectId.Value == Guid.Empty) return new List<MajorSubjectViolationModel>();
            return AppProcessor.ProcedureProvider.ExecuteTypedList<MajorSubjectViolationModel>(_majorSubjectViolationGetBySubjectId,
                DATA_PROVIDER_NAME, subjectId.Value, userName);
        }

        public List<CateViolationBehaviorModel> GetBehaviors(Guid? violationId)
        {
            if (!violationId.HasValue || violationId.Value == Guid.Empty) return new List<CateViolationBehaviorModel>();
            return AppProcessor.ProcedureProvider.ExecuteTypedList<CateViolationBehaviorModel>(_majorSubjectViolationGetBehaviors,
                DATA_PROVIDER_NAME, violationId.Value);
        }

        public string Save(MajorSubjectViolationModel model, string username)
        {
            var violationIdObj = AppProcessor.ProcedureProvider.ExecuteScalar(_majorSubjectViolationSave, DATA_PROVIDER_NAME,
                model.ViolationId,
                model.SubjectId,
                model.ViolationDate,
                model.TreatmentMeasures,
                model.RelatedDocuments,
                model.Images,
                model.Notes,
                model.ReporterName,
                model.ReporterUnit,
                model.ReporterPosition,
                model.ReporterPhone,
                model.ReporterUnionId,
                username
            );

            var violationIdStr = violationIdObj?.ToString();
            if (!string.IsNullOrEmpty(violationIdStr) && Guid.TryParse(violationIdStr, out var actualViolationId))
            {
                if (!string.IsNullOrEmpty(model.BehaviorIds))
                {
                    AppProcessor.ProcedureProvider.Execute(_majorSubjectViolationSaveBehaviors, DATA_PROVIDER_NAME,
                        actualViolationId,
                        model.BehaviorIds);
                }
            }

            return violationIdStr;
        }

        public bool Delete(Guid violationId, string username)
        {
            var result = AppProcessor.ProcedureProvider.ExecuteScalar(_majorSubjectViolationDelete, DATA_PROVIDER_NAME,
                violationId,
                username);
            return result != null && !string.IsNullOrEmpty(result.ToString());
        }
    }
}
