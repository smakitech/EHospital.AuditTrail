using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using EHospital.AuditTrail.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EHospital.AuditTrail.BusinessLogic.Contracts;
using EHospital.AuditTrail.BusinessLogic.Services;

namespace EHospital.AuditTrail.WebAPI
{
    /// <summary>
    /// Represent application startup settings and configuration.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// The connection string name defined in application settings.
        /// </summary>
        private const string CONNECTION_STRING_NAME = "EHospitalDB";

        ///* Swagger setting
        private const string VERSION = "v.1.0.0";
        private const string API_NAME = "EHospital.AuditTrail";
        //*/

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the services.
        /// This method gets called by the runtime.
        /// This method is used to add services to the container.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure DbContext
            string connection = this.Configuration.GetConnectionString(CONNECTION_STRING_NAME);
            services.AddDbContext<IActionLogDataProvider, AuditTrailContext>(options => options.UseSqlServer(connection));
            // Configure AutoMapper
            Mapper.Initialize(cfg => cfg.AddProfile<AuditTrailMapperProfile>());
            // Configure service
            services.AddScoped<IActionLogService, ActionLogService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            ///* Swagger Setting
            Info info = new Info
            {
                Version = VERSION,
                Title = API_NAME,
                Description = "Micro-service provides REST request for logging operations history"
                            + "about operations performing in other EHospital micro-services.",
                Contact = new Contact()
                {
                    Name = "Serhii Maksymchuk",
                    Email = "smakdealcase@gmail.com"
                }
            };
            services.AddSwaggerGen(c => { c.SwaggerDoc(VERSION, info); });
            //*/
        }

        /// <summary>
        /// This method gets called by the runtime.
        /// This method is used to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The hosting environment.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            ///* Swagger Setting
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"../swagger/{VERSION}/swagger.json", API_NAME);
            });
            //*/
        }
    }
}