using System;
using System.Collections.Generic;
using System.Linq;
using Cores.Major.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Cores.Major.Biz
{
    public class MajorCustomerBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _majorCustomersDelete = "Major_Customers_Delete";
        private readonly string _majorCustomersGet = "Major_Customers_Get";
        private readonly string _majorCustomersGetByID = "Major_Customers_GetByID";
        private readonly string _majorCustomersSave = "Major_Customers_Save";

        /// <summary>
        ///     Lấy toàn bộ danh sách theo giá trị lọc
        /// </summary>
        /// <returns>Danh sách Major_Customers</returns>
        public List<MajorCustomerModel> LoadList(string keyword, string cusType, out int total, BaseSearchModel search)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "1",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };
            var data = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorCustomerModel>(_majorCustomersGet,
                DATA_PROVIDER_NAME, cusType, keyword, search.Search, search.Order, search.OrderDir, search.StartIndex,
                search.PageSize);
            total = 0;
            if (data != null && data.Count > 0)
                total = int.Parse(data.First()?.TotalRow.ToString() ?? "0");
            return data;
        }

        /// <summary>
        ///     Lấy toàn bộ danh sách Major_Customers
        /// </summary>
        /// <returns>Danh sách Major_Customers</returns>
        public List<MajorCustomerModel> GetAll()
        {
            var list = LoadList(null, null, out _, null);
            return list;
        }

        /// <summary>
        ///     Lấy danh sách Major_Customers theo ID
        /// </summary>
        /// <returns>Danh sách Major_Customers</returns>
        public MajorCustomerModel LoadDetail(Guid? id)
        {
            var data = AppProcessor.ProcedureProvider.ExecuteScalarObject<MajorCustomerModel>(_majorCustomersGetByID,
                DATA_PROVIDER_NAME, id);
            return data;
        }

        /// <summary>
        ///     Xóa danh sách Major_Customers theo ID
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Delete(Guid? id, string userName)
        {
            var result =
                AppProcessor.ProcedureProvider.Execute(_majorCustomersDelete, DATA_PROVIDER_NAME, id, userName);
            return result.GetValueOrDefault(0);
        }

        /// <summary>
        ///     Cập nhật danh sách Major_Customers theo dữ liệu đầu vào
        /// </summary>
        /// <returns> Kết quả thực hiện</returns>
        public int Save(MajorCustomerModel model)
        {
            var result = AppProcessor.ProcedureProvider.Execute(_majorCustomersSave, DATA_PROVIDER_NAME
                , model.CusId
                , model.TypeCus
                , model.TypeCusName
                , model.CusName
                , model.TaxCode
                , model.Gender
                , model.TypeIdentifier
                , model.TypeIdentifierName
                , model.IdentifierNo
                , model.Phone
                , model.Email
                , model.ProvinceId
                , model.WardId
                , model.StreetName
                , model.AddressNo
                , model.RefCus
                , model.Address
                , model.RepresenterName
                , model.RepresenterGender
                , model.RepresenterTitle
                , model.UpdatedBy);

            return result.GetValueOrDefault(0);
        }
    }
}