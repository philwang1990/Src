using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OcelotGateway {
    public class Program {
        public static void Main(string[] args) {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                 //.UseKestrel(options => {
                 //    options.Listen(IPAddress.Loopback, 5000);  // http:localhost:5000
                 //    //options.Listen(IPAddress.Any, 80);         // http:*:80

                 //    //設定https啟動的port --> https://localhost:80
                 //     //options.Listen(IPAddress.Loopback, 5001, listenOptions => {
                 //     //   //取讀https要用的憑證  放在根目錄底下
                 //     //    listenOptions.UseHttps("certificate.pfx", "2wsx3edc");
                 //     //});
                 //})

                .UseStartup<Startup>();
    }
}
