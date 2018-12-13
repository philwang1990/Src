using System;
namespace KKday.PMS.B2S.Models.PMSModel
{
    public partial class PMSSupplierProductModel
    {
        public long pms_suppliers_xid { get; set; }

        public string pms_supplier_id { get; set; }

        public string prod_code { get; set; }

        public string prod_name { get; set; }

        public long kkday_prod_oid { get; set; }

        public DateTime prod_create_datetime { get; set; }

        public DateTime prod_update_datetime { get; set; }

        public string is_availability { get; set; }

        public string is_finish { get; set; }

        public string product_datamodel { get; set; }

        public string package_datamodel { get; set; }

        public string bookingfields_datamodel { get; set; }

        public string creator { get; set; }

        public DateTime create_datetime { get; set; }

        public DateTime update_datetime { get; set; }
    }
}
