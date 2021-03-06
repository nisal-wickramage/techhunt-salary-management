using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Techhunt.SalaryManagement.Api.ModelBinder;
using Techhunt.SalaryManagement.Application;
using Techhunt.SalaryManagement.Infrastructure.Csv;
using Techhunt.SalaryManagement.Infrastructure.Persistance;

namespace Techhunt.SalaryManagement.Api
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
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyMethod();
                    policy.AllowAnyHeader();
                });
            });

            services.AddControllers(
                options => options.ModelBinderProviders.Insert(0, new EmployeeSortOptionsBinderProvider()));

            services.AddTransient<EmployeeService>();
            services.AddTransient<ICsvMapper, CsvMapper>();
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddDbContext<SalaryManagementDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "SalaryManagement"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
