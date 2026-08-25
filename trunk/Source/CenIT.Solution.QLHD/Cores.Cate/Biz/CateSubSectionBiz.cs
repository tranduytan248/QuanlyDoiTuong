using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateSubSectionBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _subSectionDelete = "Cate_SubSection_Delete";
        private readonly string _subSectionGet = "Cate_SubSection_Get";
        private readonly string _subSectionGetByID = "Cate_SubSection_GetByID";
        private readonly string _subSectionSave = "Cate_SubSection_Save";

        /// <summary>
        ///     Lấy toàn bộ danh sách theo giá trị lọc
        /// </summary>
        /// <returns>Danh sách Cate_SubSection</returns>
        public List<CateSubSectionModel> LoadList(out int total, int? mainSection, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };
            var data = AppProcessor.ProcedureProvider.ExecuteTypedList<CateSubSectionModel>(_subSectionGet,
                DATA_PROVIDER_NAME,
                mainSection,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);
            total = 0;
            if (data != null && data.Count > 0)
                total = int.Parse(data.First()?.TotalRecord.ToString() ?? "0");
            return data;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách điều kiện thành phần lập đơn giá
        /// </summary>
        /// <returns>Danh sách điều kiện thành phần lập đơn giá</returns>
        public List<CateSubSectionModel> GetAll(int? mainSection)
        {
            var list = LoadList(out _, mainSection, null);
            return list;
        }

        /// <summary>
        ///     Lấy thông tin điều kiện thành phần lập đơn giá theo ID
        /// </summary>
        /// <returns>thông tin điều kiện thành phần lập đơn giá</returns>
        public CateSubSectionModel LoadDetail(int? id)
        {
            var data = AppProcessor.ProcedureProvider.ExecuteScalarObject<CateSubSectionModel>(_subSectionGetByID,
                DATA_PROVIDER_NAME, id);
            return data;
        }

        /// <summary>
        ///     Xóa điều kiện thành phần lập đơn giá theo ID
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Delete(int id)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_subSectionDelete, DATA_PROVIDER_NAME, id);
            return result.GetValueOrDefault(0);
        }

        /// <summary>
        ///     Cập nhật danh sách Cate_SubSection theo dữ liệu đầu vào
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Save(CateSubSectionModel model, string savedBy)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_subSectionSave, DATA_PROVIDER_NAME,
                model.SubSectionId,
                model.Cate_MainSectionId,
                model.SubSectionName,
                model.Unit,
                model.Price
                , savedBy);
            return result.GetValueOrDefault(0);
        }
    }
}