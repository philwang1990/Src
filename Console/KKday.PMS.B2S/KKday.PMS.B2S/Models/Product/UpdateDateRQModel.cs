using System;
using System.Collections.Generic;
using KKday.PMS.B2S.Models.Shared;

namespace KKday.PMS.B2S.Models.Product
{
    public class UpdateDateRQModel : ScmBaseModel
    {
        public UpdateDateJson json { get; set; }

    }

    public class UpdateDateJson
    {
        public long supplierOid { get; set; }
        public Guid supplierUserUuid { get; set; }
        public string deviceId { get; set; }
        public string tokenKey { get; set; }

        public int cutOfDay { get; set; }
        public string begSaleDt { get; set; }
        public string endSaleDt { get; set; }
        public string cutoffdayProcessTimezone { get; set; }
        public object cutoffdayProcessDeadline { get; set; }
        public string cutoffdayProcessWeek { get; set; }
    }

}


