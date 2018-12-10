using System;
using System.Collections.Generic;
using KKday.PMS.B2S.Models.Shared;

namespace KKday.PMS.B2S.Models.Product
{
    public class SetCostMethodRQModel : ScmBaseModel
    {
        public SetCostMethodJson json { get; set; }

    }

    public class SetCostMethodJson
    {
        public long supplierOid { get; set; }
        public Guid supplierUserUuid { get; set; }
        public string deviceId { get; set; }
        public string tokenKey { get; set; }

        public string costCalcMethod { get; set; }
        public string prodCurrCd { get; set; }
    }

}


