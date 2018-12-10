using System;
using System.Collections.Generic;

namespace KKday.PMS.B2S.Models.Shared
{
    public class SupplierLoginRQModel : ScmBaseModel
    {
        public SupplierLoginJson json { get; set; }

    }

    public class SupplierLoginJson
    {
        public string email { get; set; }
        public string password { get; set; }
        public string deviceId { get; set; }
        public string code { get; set; }
    }

}


