using Microsoft.AspNetCore;
using NLog;
using NLog.Config;
using Personnel.Api;
using Personnel.Infra.Data.Mapping.Logging;

namespace Personnel.Api.Program
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    CreateWebHostBuilder(args).Build().Run();
        //}

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>();


        public static void Main(string[] args)
        {
            LoggingConfiguration loggingConfiguration = new();
            loggingConfiguration.DefaultCultureInfo = new System.Globalization.CultureInfo("IR-fa");

            var logger = LogManager.Setup()
                                 .LoadConfiguration(loggingConfiguration)
                                 .GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            };
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //builder.Logging.AddDbLogger(options =>
                    //{
                    //    builder.Configuration.GetSection("Logging").GetSection("Database").Bind(options);
                    //});

                    webBuilder.UseStartup<Startup>();
                });
        //.ConfigureLogging(logging =>
        //{
        //    logging.ClearProviders();
        //    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        //});

    }


}
