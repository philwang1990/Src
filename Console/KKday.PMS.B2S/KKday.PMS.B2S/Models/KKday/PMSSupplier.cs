using System;
namespace KKday.PMS.B2S.Models.PMSModel
{
    public partial class PMSSupplierModel
    {
        public long xid { get; set; }

        public string pms_supplier_id { get; set; }

        public string pms_supplier_name { get; set; }

        public long kkday_supplier_oid { get; set; }

        public string pms_source { get; set; }

        public DateTime create_datetime { get; set; }

        public string creator { get; set; }

        public DateTime update_datetime { get; set; }

        public string updater { get; set; }
    }
}
