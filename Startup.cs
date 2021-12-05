using Couchbase.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoAPI.Extensions;
using ToDoAPI.Services;
using ToDoAPI.Uilities.config;
using ToDoAPI.Uilities.Security.Token;

namespace ToDoAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomSwagger();
            services.AddCustomJwtToken(Configuration);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                );
            });

            //read in configuration to connect to the database
            IConfigurationSection cbConf = Configuration.GetSection("Couchbase");
            services.Configure<CouchbaseConfig>(cbConf);

            //register the configuration 
            services.AddCouchbase(cbConf);
            services.AddHttpClient();

            // configure custom services
            services.AddTransient<CouchDatabaseInintService>();
            services.AddSingleton<ITokenHandler, TokenHandler>();



            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCustomSwagger();
            }

            if (env.EnvironmentName == "Testing")
            {
                
                //setup the database once everything is setup and running integration tests need to make sure database is fully working before running,hence running Synchronously
                appLifetime.ApplicationStarted.Register(() => {
                    var db = app.ApplicationServices.GetService<CouchDatabaseInintService>();
                    db.SetupDatabase().RunSynchronously();
                });
            }
            else
            {
                //setup the database once everything is setup and running
                appLifetime.ApplicationStarted.Register(async () => {
                    var db = app.ApplicationServices.GetService<CouchDatabaseInintService>();
                    await db.SetupDatabase();
                });
            }

            //remove couchbase from memory when ASP.NET closes
            appLifetime.ApplicationStopped.Register(() => {
                app.ApplicationServices
                   .GetRequiredService<ICouchbaseLifetimeService>()
                   .Close();
            });

            // app.UseHsts();
            // app.UseHttpsRedirection();
            app.UseCors();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
