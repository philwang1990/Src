using System;
using System.Collections.Generic;
using KKday.PMS.B2S.Models.Shared;

namespace KKday.PMS.B2S.Models.Product
{
    public class ExchangeUpdateRQModel : ScmBaseModel
    {
        public ExchangeUpdateJson json { get; set; }

    }

    public class ExchangeUpdateJson
    {
        public long supplierOid { get; set; }
        public Guid supplierUserUuid { get; set; }
        public string deviceId { get; set; }
        public string tokenKey { get; set; }

        public string moduleType { get; set; }
        public ExchangeUpdateModuleSetting moduleSetting { get; set; }
    }


    public class ExchangeUpdateModuleSetting
    {
        public bool isRequired { get; set; }
        public ExchangeUpdateSetting setting { get; set; }
    }

    public class ExchangeUpdateSetting
    {
        public string exchangeType { get; set; }
        public ExchangeUpdateDataItems dataItems { get; set; }
    }

    public class ExchangeUpdateDataItems
    {
        public List<object> locations { get; set; }
    }


}


