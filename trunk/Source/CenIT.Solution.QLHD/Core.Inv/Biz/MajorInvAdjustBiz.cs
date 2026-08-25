using System;
using System.Collections.Generic;
using System.Linq;
using Core.Inv.Models;
using TSFramework.App.Models;
using TSFramework.App.Processors;

namespace Core.Inv.Biz
{
    public class MajorInvAdjustBiz
    {
        private const string DATA_PROVIDER_NAME = "MCSProvider";
        private readonly string _majorInvAdjustGet = "Major_Inv_Adjust_Get";

        public List<MajorInvAdjustModel> GetInvAdjust(out int total, string userName, string managerUnions,
            string invNo = null, string pattern = null, string serials = null, string invTypes = null,
            DateTime? createdFrom = null, DateTime? createdTo = null, string creators = null, string cusName = null,
            string cusTaxCode = null, BaseSearchModel search = null)
        {
            search = search ?? new BaseSearchModel
            {
                Search = null,
                Order = "0",
                OrderDir = "ASC",
                StartIndex = 0,
                PageSize = -1
            };

            var lstAdjustInv = AppProcessor.ProcedureProvider.ExecuteTypedList<MajorInvAdjustModel>(_majorInvAdjustGet,
                DATA_PROVIDER_NAME, userName, managerUnions, invNo, pattern, serials, invTypes, cusTaxCode,
                createdFrom, createdTo, creators, cusName,
                search.Search,
                search.Order,
                search.OrderDir,
                search.StartIndex,
                search.PageSize);

            total = 0;
            if (lstAdjustInv != null && lstAdjustInv.Count > 0)
                total = int.Parse(lstAdjustInv.First()?.TotalRow.ToString() ?? "0");
            return lstAdjustInv;
        }
    }
}