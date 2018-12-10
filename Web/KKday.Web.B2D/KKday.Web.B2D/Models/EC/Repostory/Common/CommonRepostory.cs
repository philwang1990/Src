using System;
using System.Collections.Generic;
using KKday.Web.B2D.EC.AppCode;
using Newtonsoft.Json;

namespace KKday.Web.B2D.EC.Models.Repostory.Common
{
    public static class CommonRepostory
    {
        public static Dictionary<string, string> getuiKey(IRedisHelper rds,string lang)
        {
            Dictionary<string, string> uikey = getKlingon(rds,"frontend", lang);
            Dictionary<string, string> uikey2 = getKlingon(rds,"system", lang);

            foreach (var key in uikey2)
            {
                if (!uikey.ContainsKey(key.Key)) uikey.Add(key.Key, key.Value);
            }
            return uikey;
        }

        //挖字專用
        public static Dictionary<string, string> getKlingon(IRedisHelper rds,string webType, string lang)
        {
            try
            {
                string klingon = "";

                if (webType == "frontend")
                {
                    klingon = rds.getRedis($"common:uiLangList:{webType}:{lang}");
                }
                else
                {
                    klingon = rds.getRedis($"common:uiLangList:{webType}:{lang}");
                }

                if (klingon == null)
                {
                    //重新reflash klingon
                    //再取一次
                    //mod_commmon  lang_ui refreshUiLang2Redis 
                }
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(klingon);

                return values;
            }
            catch (Exception ex)
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                error["error"] = ex.ToString();
                return error;
            }
        }
    }
}
