using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Middleware;
using WebStore.Infrastructure.Services;

namespace WebStore
{
    public class Startup
    {
        readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // to add services to the container:
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmployeesData, InMemoryEmployeesData>();

            services
                .AddControllersWithViews(opt =>
                {
                    // opt.Conventions.Add(new WebStoreControllerConvention());
                })
                .AddRazorRuntimeCompilation();
        }

        // to configure the HTTP request pipeline:
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            app.UseStaticFiles();

            app.UseRouting();

            //// ���� �� �������� - ���������� � �������� � ������� .UseMiddleware<T>() - ������������� ������ ������ ��������� ��������, ��������, TestMiddleware:
            //app.UseMiddleware<TestMiddleware>();

            //// ������ ������ - ���������� � �������� � ������� .Map() - ������������ ������ �� ������ ���� ������� -
            //// � ������ ��������� �������� ������������ ����, ��������, ������ �������� "/maptest" � ���� ����:
            //app.Map(
            //    "/maptest",
            //    app => app.Run(async context => { await context.Response.WriteAsync("Map Test Successful"); }));

            //// ������ ������ - ���������� � �������� � ������� .MapWhen() - ������������ ������ �� ������ ����������
            //// � ������ ��������� �������� ������������ ����, ��������, ������ �������� �������� id==5:
            //app.MapWhen(context => context.Request.Query.ContainsKey("id") && context.Request.Query["id"] == 5,
            //app => app.Run(async context => { await context.Response.WriteAsync("Branch used."); }));

            // �������������� �������� MS:
            //app.UseWelcomePage("/welcome");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
