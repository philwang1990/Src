using System;
using System.Collections.Generic;
using KKday.PMS.B2S.Models.Shared;

namespace KKday.PMS.B2S.Models.Product
{
    public class SCMProductModel : ScmBaseModel
    {
        public ScmProductJson json { get; set; }


    }

    public class ScmProductJson
    {
        public string supplierOid { get; set; }
        public string supplierUserUuid { get; set; }
        public string deviceId { get; set; }
        public string tokenKey { get; set; }
        public string productName { get; set; }
        public string timezone { get; set; }
        public string masterLang { get; set; }
        public string mainCat { get; set; }
        public string supplierNote { get; set; }
        public string tagCd { get; set; }
    }
}


