using System;
using System.Collections.Generic;
using KKday.PMS.B2S.Models.Shared;

namespace KKday.PMS.B2S.Models.Product
{
    public class CountryModifyRQModel : ScmBaseModel
    {
        public CountryModifyJson json { get; set; }

    }

    public class CountryModifyJson
    {
        public long supplierOid { get; set; }
        public Guid supplierUserUuid { get; set; }
        public string deviceId { get; set; }
        public string tokenKey { get; set; }

        public string opType { get; set; }
        public string cityCd { get; set; }
    }

}


