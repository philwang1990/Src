using System;
using System.Collections.Generic;
using KKday.API.WMS.Models.DataModel.Product;

namespace KKday.API.WMS.Models.DataModel.Search {


    public class SearchProductModel {

        public string status { get; set; }
        public string desc { get; set; }
        public string pagination { get; set; }

        public List<ProductBaseModel> prods { get; set; }
        public List<object> facets { get; set; }
        public List<object> stats { get; set; }
    }

   

}
