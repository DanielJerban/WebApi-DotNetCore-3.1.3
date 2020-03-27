using System;
using System.Net;
using Common;
using Common.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace DanielApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // .Net Core default project startup 
            // CreateHostBuilder(args).Build().Run();

            // Updating the project startup
            // so we can log the errors that happen in main method using NLog
            
            // Set default proxy
            // Sentry.io has sanctioned iran so we need to set a proxy on the server and call it here  
            // After setting proxy on the host uncomment the line below 
            // WebRequest.DefaultWebProxy = new WebProxy("http://127.0.0.1:8118", true) { UseDefaultCredentials = true };

            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main", new AppException(ApiResultStatusCode.ServerError));
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(c => c.ClearProviders())
                .UseNLog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
