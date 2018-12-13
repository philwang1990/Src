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
        public long supplierOid { get; set; }
        public Guid supplierUserUuid { get; set; }
        public string deviceId { get; set; }
        public string tokenKey { get; set; }
        public string productName { get; set; }
        public string masterLang { get; set; }
        public string mainCat { get; set; }
        public string timezone { get; set; }
        public List<string> guideLang { get; set; }
        public string supplierNote { get; set; }
        public string keyWord { get; set; }
        public string orderEmail { get; set; }
        public List<string> tagCd { get; set; }
        public string introduction { get; set; }
        public string productDesc { get; set; }
        public string tourDays { get; set; }
        public string tourHours { get; set; }
        public string tourMinutes { get; set; }
        public string isSche { get; set; }
        public List<ScheMeal> scheMeal { get; set; }
        public string policyNo { get; set; }
        public string confirmHour { get; set; }


    }

    public class ScheMeal
    {
        public int day { get; set; }
        public Meal meal { get; set; }
    }

    public class Meal
    {
        public string breakfast { get; set; }
        public string lunch { get; set; }
        public string dinner { get; set; }
    }
}


