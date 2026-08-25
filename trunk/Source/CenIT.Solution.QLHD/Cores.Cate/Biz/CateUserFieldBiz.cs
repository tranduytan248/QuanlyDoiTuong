using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    /// <summary>
    /// Nghiệp vụ phân quyền lĩnh vực cho người dùng.
    /// </summary>
    public class CateUserFieldBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _cateUserFieldGet = "Sys_UserField_Get";
        private readonly string _cateUserFieldGetByUser = "Sys_UserField_GetByUser";
        private readonly string _cateUserFieldSave = "Sys_UserField_Save";

        public List<CateUserFieldModel> Get(out int total, string key, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "0",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var listUserFields = AppProcessor.ProcedureProvider.ExecuteTypedList<CateUserFieldModel>(_cateUserFieldGet,
                DATA_PROVIDER_NAME,
                key,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (listUserFields != null && listUserFields.Count > 0)
                total = int.Parse(listUserFields.First()?.TotalRow.ToString() ?? "0");
            return listUserFields;
        }

        /// <summary>
        /// Danh sách lĩnh vực mà một người dùng được phân công quản lý.
        /// </summary>
        public List<CateFieldModel> GetByUser(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return new List<CateFieldModel>();
            return AppProcessor.ProcedureProvider.ExecuteTypedList<CateFieldModel>(_cateUserFieldGetByUser,
                DATA_PROVIDER_NAME, userName.Trim());
        }

        /// <summary>
        /// Lưu danh sách lĩnh vực cho một người dùng.
        /// Proc sẽ xoá toàn bộ phân quyền cũ rồi ghi lại theo danh sách mới.
        /// </summary>
        public bool Save(string userName, string fieldIds, string createdBy)
        {
            if (string.IsNullOrWhiteSpace(userName)) return false;

            var result = AppProcessor.ProcedureProvider.ExecuteScalar(_cateUserFieldSave, DATA_PROVIDER_NAME,
                userName.Trim(),
                fieldIds,
                createdBy);

            return result != null && bool.TryParse(result.ToString(), out var isSuccess) && isSuccess;
        }
    }
}
