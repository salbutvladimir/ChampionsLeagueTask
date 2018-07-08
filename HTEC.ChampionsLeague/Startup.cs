using HTEC.ChampionsLeague.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using HTEC.ChampionsLeague.Services;
using Swashbuckle.AspNetCore.Swagger;
using HTEC.ChampionsLeague.Utils.Middlewares;

namespace HTEC.ChampionsLeague
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
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc();

            services.AddTransient<IMatchService, MatchService>();
            services.AddTransient<ITableService, TableService>();
            services.AddTransient<IStandingService, StandingService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "HTEC champions league", Version = "v1" });
                c.IncludeXmlComments(string.Format(@"{0}\HTEC.ChampionsLeague.xml", System.AppDomain.CurrentDomain.BaseDirectory));
            });

            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCustomExceptionHandler();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HTEC champions league V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}
