using System;
namespace KKday.API.WMS.Models.DataModel.Supplier
{
    public class SupplierModel
    {
        public SupplierModel()
        {
        }
        public string sup_no { get; set; }//供應商編號 
        public string sup_name { get; set; }//供應商名稱 
        public string sup_c_name { get; set; }
        public string sup_e_name { get; set; }
        public string sup_desc { get; set; }
        public string sup_web { get; set; }
        public string sup_logo_url { get; set; }

    }
}
