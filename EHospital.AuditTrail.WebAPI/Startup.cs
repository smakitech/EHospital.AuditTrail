using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using EHospital.AuditTrail.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EHospital.AuditTrail.BusinessLogic.Contracts;
using EHospital.AuditTrail.BusinessLogic.Services;

namespace EHospital.AuditTrail.WebAPI
{
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

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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