using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.Controllers {
    public class SearchController : Controller {

        [HttpPost]
        public List<ProdListModel> GetProdByDate([FromBody]SearchRQModel list_rq) {
            var prod_list = new List<ProdListModel>();
            prod_list = ProdRepository.GetProdList(list_rq);

            return prod_list;
        // GET: /<controller>/
        public IActionResult Index() {
            return View();

        }
    }
}
