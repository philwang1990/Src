﻿using System;
using System.Collections.Generic;
using KKday.PMS.B2S.Models.Shared;

namespace KKday.PMS.B2S.Models.Product
{
    public class AreaRQModel : ScmBaseModel
    {
        public AreaJson json { get; set; }

    }

    public class AreaJson
    {
        public long supplierOid { get; set; }
        public Guid supplierUserUuid { get; set; }
        public string deviceId { get; set; }
        public string tokenKey { get; set; }

        public string parentAreaCd { get; set; }
    }

}

