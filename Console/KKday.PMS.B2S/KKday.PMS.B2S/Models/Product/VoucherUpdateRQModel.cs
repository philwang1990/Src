using System;
using System.Collections.Generic;
using KKday.PMS.B2S.Models.Shared;

namespace KKday.PMS.B2S.Models.Product
{
    public class VoucherUpdateRQModel : ScmBaseModel
    {
        public VoucherUpdateJson json { get; set; }

    }

    public class VoucherUpdateJson
    {
        public long supplierOid { get; set; }
        public Guid supplierUserUuid { get; set; }
        public string deviceId { get; set; }
        public string tokenKey { get; set; }

        public string moduleType { get; set; }
        public ModuleSetting moduleSetting { get; set; }
    }

    public class ModuleSetting
    {
        public bool isRequired { get; set; }
        public Setting setting { get; set; }
    }

    public class Setting
    {
        public DataItems dataItems { get; set; }
        public string voucherType { get; set; }
    }

    public class DataItems
    {
        public ValidOptions validOptions { get; set; }
    }

    public class ValidOptions
    {
        public object isRequired { get; set; }
        public object validType { get; set; }
    }

}


