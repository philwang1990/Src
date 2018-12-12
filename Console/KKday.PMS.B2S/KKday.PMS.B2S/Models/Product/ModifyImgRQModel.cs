using System;
using System.Collections.Generic;
using KKday.PMS.B2S.Models.Shared;

namespace KKday.PMS.B2S.Models.Product
{
    public class ModifyImgRQModel : ScmBaseModel
    {
        public ModifyImgJson json { get; set; }

    }

    public class ModifyImgJson
    {
        public long supplierOid { get; set; }
        public Guid supplierUserUuid { get; set; }
        public string deviceId { get; set; }
        public string tokenKey { get; set; }

        public List<ImgList> imgList { get; set; }
    }

    public class ImgList
    {
        public string authName { get; set; }
        public string defaultImg { get; set; }
        public string imgDesc { get; set; }
        public int imgOid { get; set; }
        public int imgSeq { get; set; }
        public string imgUrl { get; set; }
        public string isCcAuth { get; set; }
        public string isCommerce { get; set; }
        public string shareType { get; set; }
        public string usageTag { get; set; }
        public string kkdayImgUrl { get; set; }
    }


}


