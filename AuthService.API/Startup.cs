namespace AuthService.API
{
    using System;
    using AuthService.API.Entities.Repository;
    using AuthService.API.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.Swagger;

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
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

             services.AddMvc();

            string defaultConnectionString = Environment.GetEnvironmentVariable("UserInfoDb") ??
                                             Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AuthServiceDbContext>(options => options.UseSqlServer(defaultConnectionString));

            services.AddScoped<IAuthServiceDbContext, AuthServiceDbContext>();
            services.AddScoped<IUserInfoRepository, UserInfoRepository>();
            services.AddScoped<IUserService, UserInfoService>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info
                {
                    Version = "v1.0",
                    Title = "UserInfo API"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            // user info api's ui view 
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "UserInfo API v1.0");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action}/{id?}");
            });
        }
    }
}
