using DNCS.Data.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNCS.Web.Main.Services.DbInitService
{
    public static class DatabaseInitExtension
    {
        public static IApplicationBuilder UseDbInit(this IApplicationBuilder app)
        {
            try
            {
                var logger = app.ApplicationServices.GetService<ILogger<Startup>>();
                if (logger != null)
                {
                    logger.LogInformation("Seek database.");
                }

                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<CustumDbContext>();
                    if (!CustumDbDataInitializer.Initialize(context))
                    {
                        throw new Exception("An error occurred while seeding the database.");
                    }
                    return app;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
