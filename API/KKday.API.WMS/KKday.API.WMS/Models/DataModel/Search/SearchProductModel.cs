﻿using System;
using System.Collections.Generic;
using KKday.API.WMS.Models.DataModel.Product;

namespace KKday.API.WMS.Models.DataModel.Search {


    public class SearchProductModel {

        public Metadata metadata { get; set; }
        public Stats stats { get; set; }
        public Facets facets { get; set; }

        public List<ProductBaseModel> prods { get; set; }

    }

    public class Metadata {
        public string result { get; set; }
        public string result_msg { get; set; }
        public int total_count { get; set; }
        public int start { get; set; }
        public int count { get; set; }

    }

    public class Stats {
        public Price price { get; set; }
    }

    public class Price {
        public int min { get; set; }
        public int max { get; set; }
        public int count { get; set; }
        public string currency { get; set; }
    }

    public class Facets {
        public List<CatMain> cat_main { get; set; }
        public List<TotalTime> total_time { get; set; }
        public List<GuideLang> guide_lang { get; set; }
        public List<Cat> cat { get; set; }
        public List<SaleDt> sale_dt { get; set; }
    }

    public class CatMain {
        public string id { get; set; }
        public string name { get; set; }
        public string sort { get; set; }
        public int count { get; set; }
    }

    public class TotalTime {
        public int time { get; set; }
        public int count { get; set; }
    }

    public class GuideLang {
        public string id { get; set; }
        public string name { get; set; }
        public int count { get; set; }
    }

    public class Cat {
        public string id { get; set; }
        public string name { get; set; }
        public string sort { get; set; }
        public int count { get; set; }
    }

    public class SaleDt {
        public int id { get; set; }
        public int count { get; set; }
    }




}
