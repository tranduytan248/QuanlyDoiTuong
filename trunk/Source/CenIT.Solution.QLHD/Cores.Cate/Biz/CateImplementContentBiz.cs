using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateImplementContentBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _cateImplementContentDelete = "Cate_ImplementContent_Delete";
        private readonly string _cateImplementContentGet = "Cate_ImplementContent_Get";
        private readonly string _cateImplementContentGetByID = "Cate_ImplementContent_GetByID";

        private readonly string _cateImplementContentSave = "Cate_ImplementContent_Save";

        /// Lấy toàn bộ thông tin nội dung thực hiện
        /// <returns></returns>
        private List<CateImplementContentModel> LoadList(out int total, string tuKhoa, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var data = AppProcessor.ProcedureProvider.ExecuteTypedList<CateImplementContentModel>(
                _cateImplementContentGet,
                DATA_PROVIDER_NAME,
                tuKhoa,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (data != null && data.Count > 0)
                total = int.Parse(data.First()?.TotalRow.ToString() ?? "0");
            return data;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách nội dung thực hiên theo bộ lọc
        /// </summary>
        /// <returns></returns>
        public List<CateImplementContentModel> GetList(out int total, string tuKhoa, BaseSearchModel search = null)
        {
            var data = LoadList(out total, tuKhoa, search);
            return data;
        }

        /// <summary>
        ///     Lấy chi tiết theo ID
        /// </summary>
        /// <returns></returns>
        private CateImplementContentModel LoadDetail(int implementContentID)
        {
            var data =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<CateImplementContentModel>(
                    _cateImplementContentGetByID, DATA_PROVIDER_NAME,
                    implementContentID);
            return data;
        }

        /// <summary>
        ///     Lấy chi tiết theo ID
        /// </summary>
        /// <returns></returns>
        public CateImplementContentModel GetById(int implementContentID)
        {
            var data = LoadDetail(implementContentID);
            return data;
        }


        /// <summary>
        ///     Lưu thông tin
        /// </summary>
        /// <returns></returns>
        public int Save(CateImplementContentModel model, string savedBy)
        {
            var implementContent = AppProcessor.ProcedureProvider.Execute(_cateImplementContentSave,
                DATA_PROVIDER_NAME,
                model.ImplementContentId,
                model.WorkContent,
                model.WorkPurpose,
                model.FileId,
                savedBy
            );
            return implementContent.GetValueOrDefault(0);
        }

        /// <summary>
        ///     Xóa nội dung thực hiện
        /// </summary>
        /// <returns></returns>
        public int Delete(CateImplementContentModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateImplementContentDelete, DATA_PROVIDER_NAME,
                model.ImplementContentId);
            return result.GetValueOrDefault(0);
        }
    }
}