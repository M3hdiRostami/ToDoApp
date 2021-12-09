using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ToDoAPI.Tests.Helpers
{
    public class CustomWebApplicationFactory<TStartup>: WebApplicationFactory<Startup>
    {

        protected IConfiguration Configuration => GetService<IConfiguration>();
        protected IWebHostEnvironment WebHostEnvironment => GetService<IWebHostEnvironment>();

        protected override IHostBuilder CreateHostBuilder()
        {
            var hbuilder = base.CreateHostBuilder();

            return hbuilder;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            base.ConfigureWebHost(builder);
        }
        public T GetService<T>()
        {
            using (var serviceScope = this.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                try
                {
                    var scopedService = services.GetRequiredService<T>();
                    return scopedService;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
            };
        }
    }
}
