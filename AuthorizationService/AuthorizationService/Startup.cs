using AuthorizationService.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AuthorizationService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public async Task ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("AuthorizationDatabase");
            services.AddDbContext<AuthorizationDbContext>(options =>
                options.UseSqlServer(connection));

            //services.AddScoped<IAccounts, AccountsInSQlRepository>();
            //services.AddScoped<IRefreshTokens, RefreshTokensInSqlRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthorizationService", Version = "v1" });
            })
                  .ConfigureSwaggerGen(options =>
                  {
                      var xmlPath = Path.Combine(AppContext.BaseDirectory, "AuthorizationService.xml");
                      options.IncludeXmlComments(xmlPath, true);

                  });
            await services.AddAsymmetricAuthentication(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NewProject v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

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
