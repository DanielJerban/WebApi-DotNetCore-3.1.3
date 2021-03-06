using Common;
using Data.Contracts;
using Data.Repositories;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Services.Services;
using WebFramework.Configuration;
using WebFramework.MiddleWares;

namespace DanielApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly SiteSettings _siteSetting;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));

            services.AddDbContext<Data.ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
            });

            services.AddControllers();

            // Dependency injection for repositories 
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJwtService, JwtService>();

            // Swagger services
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1.0", new OpenApiInfo()
                {
                    Version = "v1.0",
                    Title = "Daniel Api OpenInfo"
                });
            });

            // Jwt Authentication 
            services.AddJwtAuthentication(_siteSetting.JwtSettings);

            // Elmah Error logger service
            services.AddElmahConfiguration(Configuration, _siteSetting.ElmahSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Middle wares goes here 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Custom exception middleware 
            app.UseCustomExceptionHandler();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseExceptionHandler();
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Doc-v1");
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseElmah();
        }
    }
}
