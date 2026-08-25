using Cores.Cate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Cate.Biz
{
    public class CateCustomerBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";

        private readonly string _cateCustomerGet = "Cate_Customer_Get";
        private readonly string _cateCustomerSave = "Cate_Customer_Save";
        private readonly string _cateCustomerGetById = "Cate_Customer_GetById";
        private readonly string _cateCustomerDelete = "Cate_Customer_Delete";


        /// <summary>
        /// Get thông tin khách hàng
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="fullName"></param>
        /// <param name="total"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<CateCustomerModel> Get(string userType, string fullName,  out int total, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "0",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var listCustomers = AppProcessor.ProcedureProvider.ExecuteTypedList<CateCustomerModel>(_cateCustomerGet,
                DATA_PROVIDER_NAME,
                fullName,
                userType,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (listCustomers != null && listCustomers.Count > 0)
                total = int.Parse(listCustomers.First()?.TotalRow.ToString() ?? "0");
            return listCustomers;
        }

        /// <summary>
        /// Get tất cả thông tin khách hàng
        /// </summary>
        /// <returns></returns>
        public List<CateCustomerModel> GetAll(string fullName = null, string userType = "ALL")
        {
            var listCustomers = Get(fullName, userType, out _, null);
            return listCustomers;
        }

        /// <summary>
        /// Lưu thông tin khách hàng 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int? Save(CateCustomerModel model, string username)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_cateCustomerSave, DATA_PROVIDER_NAME,
                model.CustomerId,
                model.FullName,
                model.Gender,
                model.UserType,
                model.TaxCode,
                model.PhoneNumber,
                model.Email,
                model.Zalo,
                model.CitizenIdentification,
                model.PositionName,
                model.UnionName,
                model.ProvinceId,
                model.DistrictId,
                model.WardId,
                model.StreetId,
                model.BusinessCode,
                model.PlaceGetCitizenIdentification,
                model.PlaceGetBusinessCode,
                model.DateGetBusinessCode,
                model.PageType,
                model.Alley,
                model.ApartmentNumber,
                username);

            return result;
        }

        /// <summary>
        /// Get thông tin khách hàng bằng id
        /// </summary>
        /// <param name="cateId"></param>
        /// <returns></returns>
        public CateCustomerModel GetById(Guid? cateId)
        {
            var lstCategories =
                AppProcessor.ProcedureProvider.ExecuteScalarObject<CateCustomerModel>(_cateCustomerGetById,
                    DATA_PROVIDER_NAME, cateId);

            return lstCategories;
        }

        /// <summary>
        /// Xóa thông tin khách hàng
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool Delete(CateCustomerModel model, string username)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_cateCustomerDelete, DATA_PROVIDER_NAME, model.CustomerId, username);
            return result == 1;
        }
    }
}
