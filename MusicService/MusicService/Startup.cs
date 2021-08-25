using AuthorizationService.Extensions;
using AuthorizationService.SwaggerFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MusicService.Dto;
using MusicService.Models;
using MusicService.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace MusicService
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
            var connection = Configuration.GetConnectionString("MusicDatabase");
            services.AddDbContext<MusicDatabase>(options =>
                options.UseSqlServer(connection));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IStorage<AlbumDto>, AlbumsInSQLRepository>();
            services.AddScoped<IStorage<SongDto>, SongsInSQLRepository>();

            services.AddAutoMapper(typeof(ApiMappingProfile));

            services.AddScoped<IAlbums, AlbumsInSQLRepository>();
            services.AddScoped<IListeners, ListenersInSQLRepository>();
            services.AddScoped<IPerformers, PerformersInSQLRepository>();
            services.AddScoped<ISongs, SongsInSQLRepository>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MusicService", Version = "v1" });
            })
            .ConfigureSwaggerGen(options =>
            {
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "MusicService.xml");
                options.IncludeXmlComments(xmlPath, true);
            });

            services.AddAsymmetricAuthentication(Configuration);


            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] " +
                        "and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
                c.DocumentFilter<SwaggerAddEnumDescriptions>();
                c.OperationFilter<ReqiuredRolesDescriptionFilter>();
            });
        }

    

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MusicService v1"));
            }



            app.UseHttpsRedirection();

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
