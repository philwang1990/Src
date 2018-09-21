using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.Controllers {
    public class SearchController : Controller {


        [HttpPost]
        public SearchProductModel GetProd([FromBody]SearchRQModel list_rq) {
            var prods = new SearchProductModel();
            prods = ProdRepository.GetProdList(list_rq);

            return prods;

        }
    }
}
