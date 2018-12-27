using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace EHospital.AuditTrail.WebAPI
{
    /// <summary>
    /// Represent logic of entry point of application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Represent logic of entry point of application.
        /// </summary>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates the web host builder.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>Web host builder.</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}