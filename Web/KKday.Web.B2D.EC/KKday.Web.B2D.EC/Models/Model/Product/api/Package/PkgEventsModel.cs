using System;
using System.Collections.Generic;

namespace KKday.Web.B2D.EC.Models.Model.Product
{
    public class PkgEventsModel
    {

        public string result { get; set; }
        public string result_msg { get; set; }
        public int pkg_no { get; set; }
        public string is_hl { get; set; }

        public List<Event> events { get; set; }

    }

    public class Event
    {
        public string day { get; set; }
        public string event_times { get; set; }
    }
}
