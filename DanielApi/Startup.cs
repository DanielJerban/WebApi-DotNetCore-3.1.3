using Data.Contracts;
using Data.Repositories;
using ElmahCore.Mvc;
using ElmahCore.Sql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebFramework.MiddleWares;

namespace DanielApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Data.ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
            });

            services.AddControllers();

            // Dependency injection for repositories 
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();

            // Swagger services
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1.0", new OpenApiInfo()
                {
                    Version = "v1.0",
                    Title = "Daniel Api OpenInfo"
                });
            });

            // Elmah Error logger service
            services.AddElmah<SqlErrorLog>(c =>
            {
                c.Path = "/Elmah";
                c.ConnectionString = Configuration.GetConnectionString("Elmah");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Custom exception middleware 
            app.UseCustomExceptionHandler();

            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseExceptionHandler();
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Swagger middleware
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Doc-v1");
            });

            // Elmah middleware 
            app.UseElmah();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
