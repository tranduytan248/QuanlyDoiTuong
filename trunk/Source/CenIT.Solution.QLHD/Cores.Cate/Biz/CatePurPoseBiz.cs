using System.Collections.Generic;
using System.Linq;
using Cores.Cate.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CatePurPoseBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _catePurPoseDelete = "Cate_PurPose_Delete";
        private readonly string _catePurPoseGet = "Cate_PurPose_Get";
        private readonly string _catePurPoseGetByID = "Cate_PurPose_GetByID";

        private readonly string _catePurPoseSave = "Cate_PurPose_Save";

        /// Lấy toàn bộ thông tin loại hợp đồng
        /// <returns></returns>
        private List<CatePurPoseModel> LoadList(out int total, string searchValue, string contractTypeIds,
            BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var lstPurposes = AppProcessor.ProcedureProvider.ExecuteTypedList<CatePurPoseModel>(_catePurPoseGet,
                DATA_PROVIDER_NAME,
                searchValue,
                contractTypeIds,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (lstPurposes != null && lstPurposes.Count > 0)
                total = int.Parse(lstPurposes.First()?.TotalRow.ToString() ?? "0");
            return lstPurposes;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách nội dung thực hiên theo bộ lọc
        /// </summary>
        /// <returns></returns>
        public List<CatePurPoseModel> GetList(out int total, string searchValue, string contractTypeIds,
            BaseSearchModel search = null)
        {
            var lstPurposes = LoadList(out total, searchValue, contractTypeIds, search);
            return lstPurposes;
        }

        /// <summary>
        ///     Lấy chi tiết theo ID
        /// </summary>
        /// <returns></returns>
        private CatePurPoseModel LoadDetail(int purposeId)
        {
            var data =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<CatePurPoseModel>(_catePurPoseGetByID,
                    DATA_PROVIDER_NAME,
                    purposeId);
            return data;
        }

        /// <summary>
        ///     Lấy chi tiết theo ID
        /// </summary>
        /// <returns></returns>
        public CatePurPoseModel GetById(int purposeId)
        {
            var data = LoadDetail(purposeId);
            return data;
        }

        /// <summary>
        ///     Lấy tất cả loại hợp đồng
        /// </summary>
        /// <returns></returns>
        public List<CatePurPoseModel> GetAll(string searchValue = null, string contractTypeIds = null)
        {
            var lstPurposes = LoadList(out _, contractTypeIds: contractTypeIds, searchValue: null, search: null);
            return lstPurposes;
        }


        /// <summary>
        ///     Lưu thông tin
        /// </summary>
        /// <returns></returns>
        public int Save(CatePurPoseModel model, string savedBy)
        {
            var purpose = AppProcessor.ProcedureProvider.Execute(_catePurPoseSave,
                DATA_PROVIDER_NAME,
                model.PurPoseId,
                model.ContractTypeId,
                model.PurPoseName,
                savedBy
            );
            return purpose.GetValueOrDefault(0);
        }

        /// <summary>
        ///     Xóa loại hợp đồng
        /// </summary>
        /// <returns></returns>
        public int Delete(CatePurPoseModel model)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_catePurPoseDelete, DATA_PROVIDER_NAME, model.PurPoseId);
            return result.GetValueOrDefault(0);
        }
    }
}