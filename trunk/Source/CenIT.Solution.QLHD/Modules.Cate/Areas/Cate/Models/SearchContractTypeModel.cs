using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Modules.Cate.Areas.Cate.Models
{
    public class SearchContractTypeModel
    {
        public string TuKhoa { get; set; }

        public List<ListItem> ListTypeContracts { get; set; } = new List<ListItem>();
    }
}