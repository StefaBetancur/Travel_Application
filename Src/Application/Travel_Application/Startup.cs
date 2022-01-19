using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Travel_Application
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddSession();
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();
            app.UseSession();
            app.UseMvc(router =>
            {
                router.MapRoute(
                name: "default",
                template: "{controller=Login}/{action=Login}/{id?}");
            });
        }
    }
}