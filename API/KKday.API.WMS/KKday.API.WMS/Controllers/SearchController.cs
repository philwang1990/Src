using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKday.API.WMS.Models.DataModel.Search;
using KKday.API.WMS.Models.Repository;
using KKday.API.WMS.Models.Repository.Discount;
using KKday.API.WMS.Models.Search;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.Controllers {

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SearchController : Controller {

        /// <summary>
        /// Gets the prod.
        /// </summary>
        /// <returns>The prod.</returns>
        /// <param name="list_rq">List rq.</param>
        [HttpPost]
        public SearchProductModel GetProd([FromBody]SearchRQModel list_rq) {

            SearchProductModel prods = SearchRepository.GetProdList(list_rq);

            return prods;

        }
    }
}

