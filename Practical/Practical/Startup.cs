using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Practical.Context;
using Practical.Repositories;

namespace Practical
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
            services.AddControllers();
            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationContext>(opt =>
            opt.UseNpgsql(Configuration.GetConnectionString("ConnectionString")));
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseCors(CorsOptions.);
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.MapSignalR("/signalr", new HubConfiguration());
            app.UseSignalR(route =>
            {
                route.MapHub<NotificationHub>("/notificationHub");
            });
        }
    }
}
