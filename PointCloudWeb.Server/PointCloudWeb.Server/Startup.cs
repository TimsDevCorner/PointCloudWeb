using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PointCloudWeb.Server.Services;

namespace PointCloudWeb.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            if (Directory.Exists(Globals.TempPath))
                Directory.Delete(Globals.TempPath, true);
            if (Directory.Exists(Globals.PotreeDataPath))
                Directory.Delete(Globals.PotreeDataPath, true);
            Directory.CreateDirectory(Globals.TempPath);
            Directory.CreateDirectory(Globals.PotreeDataPath);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

            //app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<PointCloudService>();
            services.AddTransient<ScanConverterService>();
            services.AddTransient<ScanDataService>();
            services.AddTransient<IPointCloudRegistrationService, PointCloudRegistrationServiceTeaerPp>();
            services.AddControllers();
        }
    }
}