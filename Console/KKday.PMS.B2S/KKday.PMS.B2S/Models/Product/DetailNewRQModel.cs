using System;
using System.Collections.Generic;
using KKday.PMS.B2S.Models.Shared;

namespace KKday.PMS.B2S.Models.Product
{
    public class DetailNewRQModel : ScmBaseModel
    {
        public DetailNewJson json { get; set; }

    }

    public class DetailNewJson
    {
        public long supplierOid { get; set; }
        public Guid supplierUserUuid { get; set; }
        public string deviceId { get; set; }
        public string tokenKey { get; set; }

        public List<DetailList> detailList { get; set; }
    }

    public class DetailList
    {
        public string detailType { get; set; }
        public string desc { get; set; }
    }


}


