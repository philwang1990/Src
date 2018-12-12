using System;
using System.Collections.Generic;

namespace KKday.API.WMS.Models.DataModel.Order {

    public class FileModel {
      
            public string result { get; set; }
            public string result_msg { get; set; }
            public List<File> files { get; set; }
    }

    public class LastModified {
        public int date { get; set; }
        public int day { get; set; }
        public int hours { get; set; }
        public int minutes { get; set; }
        public int month { get; set; }
        public int nanos { get; set; }
        public int seconds { get; set; }
        public long time { get; set; }
        public int timezone_offset { get; set; }
        public int year { get; set; }
    }

    public class File {
        public int order_file_id { get; set; }
        public LastModified last_modified { get; set; }
        public string file_name { get; set; }
        public string file_intro { get; set; }
        public string file_source { get; set; }
        public string kk_ticket_oid { get; set; }
        public bool has_barcode { get; set; }
        public int barcode_count { get; set; }
        public string uploader { get; set; }
        public string order_mid { get; set; }
        public string agency_no { get; set; }
        public string crt_dt { get; set; }
        public object guide_lang { get; set; }
        public string prod_name { get; set; }
        public string pkg_name { get; set; }
        public string s_date_GMT { get; set; }//begLstGoDtGMT
        public string s_date_GMTNm { get; set; }//begLstGoDtGMTNm
        public int price1_qty { get; set; }
        public int price2_qty { get; set; }
        public int price3_qty { get; set; }
        public int price4_qty { get; set; }
      
    }



   
}
