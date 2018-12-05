using System;
using System.Collections.Generic;

namespace KKday.PMS.B2S.Models.Shared
{
    public class SupplierLoginRSModel : RSModel
    {
            public string email { get; set; }
            public string password { get; set; }
            public long supplierOid { get; set; }
            public Guid supplierUserUuid { get; set; }
            public string deviceId { get; set; }
            public string tokenKey { get; set; }


    }

}


