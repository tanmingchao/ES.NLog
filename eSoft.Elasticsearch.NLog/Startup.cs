using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using NLog.Web;

namespace eSoft.Elasticsearch.NLog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddCors(options => //方式比较俗
            //{
            //    options.AddPolicy("cors",
            //        builder =>
            //        {
            //            builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials()
            //            //.WithOrigins("http://127.0.0.1:3201")
            //            ;
            //        });
            //});
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        //builder.WithOrigins("http://127.0.0.1:3201").AllowAnyHeader();
                        builder
                        .SetIsOriginAllowed(_ => { return true; })
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.ConfigureSwaggerGen(c =>
            {
                c.SwaggerDoc("使用NLoge记录日志到ES V1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "使用NLoge记录日志到ES V1", Version = "v1" });
                //c.SwaggerDoc("使用NLoge记录日志到ES V2", new Swashbuckle.AspNetCore.Swagger.Info { Title = "使用NLoge记录日志到ES V2", Version = "v2" });
                c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "eSoft.Elasticsearch.NLog.xml"));//这里的 xml名称是上面属性 中XML文档文件的名称
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("cors");
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");
            app.UseMvc();
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "使用NLoge记录日志到ES V1");
            //    //c.SwaggerEndpoint("/swagger/v2/swagger.json", "使用NLoge记录日志到ES V2");
            //    c.RoutePrefix = string.Empty;
            //});
        }
    }
}
