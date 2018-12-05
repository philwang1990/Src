using System;
using log4net;
using KKday.PMS.B2S.AppCode;
using KKday.PMS.B2S.Models.Package;
using Newtonsoft.Json;

namespace KKday.PMS.B2S.ModuleRepository
{
    public class ModuleRepository
    {
        private readonly static ILog _log = LogManager.GetLogger(typeof(ModuleRepository));

        public void Main()
        {
            try
            {
                //initial log4net
                CommonTool.LoadLog4netConfig();

                var get = CommonTool.GetData("https://api.rezdy.com/latest/availability?apiKey=0b3d137cc1db4108a92c309fa7d7f6da&productCode=PVVRFE&startTimeLocal=2018-11-01 00:00:00&endTimeLocal=2019-12-31 00:00:00");
                //RezdyPackageModel

                RezdyPackageModel obj = JsonConvert.DeserializeObject<RezdyPackageModel>(get);

                //do something
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
            }
        }
    }
}
