using System;
namespace KKday.API.WMS.Models.Search {
    public class SearchRQModel {

        public string lang { get; set; }
        public string[] prod_ids { get; set; }
        public string currency { get; set; }
        public string start { get; set; }
        public string count { get; set; }
        public string q { get; set; }
        public string[] country_keys { get; set; }
        public string[] city_keys { get; set; }
        public string[] cat_main_keys { get; set; }
        public string[] cat_keys { get; set; }
        public string price_from { get; set; }
        public string price_to { get; set; }
        public string date_from { get; set; }
        public string date_to { get; set; }
        public string[] durations { get; set; }
        public string[] stats { get; set; }
        public string locale { get; set; }
        public string sort { get; set; }
        public string source { get; set; }
        public string member_uuid { get; set; }
        public string footprint_id { get; set; }
        public string ip { get; set; }
        public string multiprice_platform { get; set; }
        public string[] facets { get; set; }
        public string companyXid { get; set; }


    }
}









