using System;
using KKday.PMS.B2S.Models.Shared.Enum;
namespace KKday.PMS.B2S.Models.PMSModel
{
    public partial class ProductMapLogModel
    {
        public long xid { get; set; }

        public long kkday_prod_oid { get; set; }

        public Step step { get; set; }

        public DateTime create_datetime { get; set; }
    }
}
