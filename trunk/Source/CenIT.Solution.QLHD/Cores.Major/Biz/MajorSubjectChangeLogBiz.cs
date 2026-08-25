using System;
using System.Collections.Generic;
using System.Linq;
using Cores.Major.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Major.Biz
{
    /// <summary>
    /// Nghiệp vụ ghi và tra cứu log cập nhật Đối tượng / Lịch sử vi phạm.
    /// </summary>
    public class MajorSubjectChangeLogBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _changeLogSave = "Major_Subject_ChangeLog_Save";
        private readonly string _changeLogGet = "Major_Subject_ChangeLog_Get";

        /// <summary>
        /// Ghi một dòng log. Thứ tự tham số phải khớp đúng với
        /// p_Major_Subject_ChangeLog_Save vì provider truyền theo vị trí.
        /// </summary>
        public string Save(MajorSubjectChangeLogModel model)
        {
            if (model == null || model.SubjectId == Guid.Empty) return null;

            var result = AppProcessor.ProcedureProvider.ExecuteScalar(_changeLogSave, DATA_PROVIDER_NAME,
                model.SubjectId,
                model.ViolationId,
                model.EntityType,
                model.ActionType,
                model.ChangedFields,
                model.ChangedFieldNames,
                model.Description,
                model.ActorUserName,
                model.ActorName,
                model.ActorPosition,
                model.ActorUnit,
                model.ActorUnionId);

            return result?.ToString();
        }

        public List<MajorSubjectChangeLogModel> Get(out int total, Guid? subjectId, string entityType,
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

            var listLogs = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorSubjectChangeLogModel>(_changeLogGet,
                DATA_PROVIDER_NAME,
                subjectId,
                entityType,
                userName,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (listLogs != null && listLogs.Count > 0)
                total = int.Parse(listLogs.First()?.TotalRow.ToString() ?? "0");
            return listLogs;
        }
    }
}
