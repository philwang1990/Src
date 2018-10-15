using System;
namespace KKday.API.WMS.Models.DataModel.Package {
        
        public class PkgRQModel {

            public string apiKey { get; set; }
            public string userOid { get; set; }
            public string ver { get; set; }
            public string locale { get; set; }
            public string currency { get; set; }
            public string ipaddress { get; set; }
            public Json json { get; set; }
        }

        public class Json {
            public string pkgStatus { get; set; }
            public string pkgOid { get; set; }
            public string multipricePlatform { get; set; }
        }

    }

