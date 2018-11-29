using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Filters;
using KKday.Web.B2D.BE.Models.Model.Account;
using KKday.Web.B2D.BE.Models.Model.Voucher;
using KKday.Web.B2D.BE.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.Web.B2D.BE.Areas.User.Controllers
{
    [Area("User")]
    [TypeFilter(typeof(CultureFilter))]
    [Authorize(Policy = "UserOnly")]
    public class VoucherController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            var services = HttpContext.RequestServices.GetServices<IB2dAccountRepository>();
            var vouchRepos = HttpContext.RequestServices.GetService<VouchAddonRepository>();

            var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
            var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));

            B2dVoucherAddon vouch = vouchRepos.GetVoucherAddon(UserData.COMPANY_XID);

            return View(vouch);
        }

        // 選擇檔案上載
        [HttpPost]
        public async Task<IActionResult> UploadLogo([FromQuery]Int64 comp_xid, List<IFormFile> files)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var upd_user = User.FindFirst("Account").Value;
                var vouchRepos = HttpContext.RequestServices.GetService<VouchAddonRepository>();
                List<string> logo_url = new List<string>();

                var size = files.Sum(f => f.Length);

                foreach (var _file in files)
                {
                    if (_file.Length > 0)
                    {
                        var hash_code = System.Text.RegularExpressions.Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
                        // full path to file in temp location
                        var filePath = Path.Combine(Path.GetTempPath(), hash_code + "_" + _file.FileName);  // Path.GetTempFileName();

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await _file.CopyToAsync(stream);
                        }

                        // 透過 FS API 把檔案傳上 S3, 並取得 URL 

                        // 儲放網址
                        logo_url.Add(filePath);
                    }
                }

                vouchRepos.UpdateVoucheLogo(comp_xid, logo_url[0], upd_user);

                jsonData["img_url"] = logo_url;
                jsonData["status"] = "OK";
            }
            catch (Exception ex)
            {
                jsonData.Clear();
                jsonData.Add("status", "FAIL");
                jsonData.Add("msg", ex.Message);
            }

            return Json(jsonData);
        }

        // 存檔
        [HttpPost]
        public IActionResult UpdateVouchAddon([FromBody] B2dVoucherAddon vouch_addon)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var vouchRepos = HttpContext.RequestServices.GetService<VouchAddonRepository>();
                var upd_user = User.FindFirst("Account").Value;

                var aesUserData = User.FindFirst(ClaimTypes.UserData).Value;
                var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));
                vouch_addon.COMPANY_XID = UserData.COMPANY_XID;

                //更新分銷商公司資料
                vouchRepos.UpdateVouchAddon(vouch_addon, upd_user);
                jsonData["status"] = "OK";
            }
            catch (Exception ex)
            {
                jsonData.Clear();
                jsonData.Add("status", "FAIL");
                jsonData.Add("msg", ex.Message);
            }

            return Json(jsonData);
        }
    }
}
