using System;
using System.Collections.Generic;

namespace KKday.API.WMS.Models.DataModel.Common
{
    public class GuideLanguageModel
    {

        public string result { get; set; }
        public string result_msg { get; set; }
        public List<langList> lang_list { get; set; }
    }

    public class langList
    {
        public string langShortCd { get; set; }
        public string langName { get; set; }
        public string langCd { get; set; }
    }
}
