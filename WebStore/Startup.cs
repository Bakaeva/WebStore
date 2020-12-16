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

            //// Один из способов - добавление в конвейер с помощью .UseMiddleware<T>() - использование своего класса обработки запросов, например, TestMiddleware:
            //app.UseMiddleware<TestMiddleware>();

            //// Другой способ - добавление в конвейер с помощью .Map() - разветвление потока на основе пути запроса -
            //// в случае обработки запросов определённого вида, например, запрос содержит "/maptest" в своём пути:
            //app.Map(
            //    "/maptest",
            //    app => app.Run(async context => { await context.Response.WriteAsync("Map Test Successful"); }));

            //// Третий способ - добавление в конвейер с помощью .MapWhen() - разветвление потока на основе предикатов
            //// в случае обработки запросов определённого вида, например, запрос содержит параметр id==5:
            //app.MapWhen(context => context.Request.Query.ContainsKey("id") && context.Request.Query["id"] == 5,
            //app => app.Run(async context => { await context.Response.WriteAsync("Branch used."); }));

            // Приветственная страница MS:
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
