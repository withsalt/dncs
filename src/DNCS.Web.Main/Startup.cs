using System;
using System.Runtime.InteropServices;
using DNCS.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using DNCS.Data.Entity;
using DNCS.Web.Main.Services.DbInitService;
using System.Text;
using DNCS.LogInfo;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DNCS.Web.Main
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
            //初始化配置文件
            if (!InitConfig())
            {
                throw new Exception("Init config failed.");
            }

            #region 数据库
            //配置数据库实体依赖注入
            services.AddDbContext<CustumDbContext>();
            #endregion

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                services.Configure<IISServerOptions>(options => options.AllowSynchronousIO = true);
            }
            else
            {
                services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);
            }

            services.AddControllersWithViews()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddRazorRuntimeCompilation()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDbInit();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            //防止中文乱码
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private bool InitConfig()
        {
            try
            {
                ConfigManager config = Configuration.Get<ConfigManager>();
                ConfigManager.Now = config;
                if (ConfigManager.Now == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"Init config failed. {ex.Message}", ex);
                return false;
            }
        }
    }
}
