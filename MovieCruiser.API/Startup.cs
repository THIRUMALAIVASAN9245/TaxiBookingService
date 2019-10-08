using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BookingService.API
{
    using System;
    using System.Net.Http;
    using System.Reflection;
    using AutoMapper;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using BookingService.API.Contract;
    using BookingService.API.Entities;
    using BookingService.API.Entities.Repository;
    using Swashbuckle.AspNetCore.Swagger;

    ///<Summary>
    /// Startup class
    ///</Summary>
    public partial class Startup
    {
        ///<Summary>
        /// Startup class constructor
        ///</Summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        ///<Summary>
        /// Configuration
        ///</Summary>
        public IConfiguration Configuration { get; }

        ///<Summary>
        /// ConfigureServices method
        ///</Summary>
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

            ConfigureJwtAuthenticationService(Configuration, services);

            services.AddMvc();
            services.AddMediatR();

            services.AddAutoMapper(Assembly.GetAssembly(typeof(BookingMappingProfile)));
            services.AddScoped<IRepository, Repository>();
            var httpClient = new HttpClient();
            services.AddSingleton(httpClient);

            string defaultConnectionString = Environment.GetEnvironmentVariable("TaxiBookingDb") ??
                                             Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<BookingDbContext>(options => options.UseSqlServer(defaultConnectionString));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info
                {
                    Version = "v1.0",
                    Title = "TaxiBooking API"
                });
            });
        }

        private void ConfigureJwtAuthenticationService(IConfiguration configuration, IServiceCollection services)
        {
            var audience = Configuration.GetSection("Audience");
            var symmetrickey = audience["secret"];
            var byteArray = Encoding.ASCII.GetBytes(symmetrickey);
            var signingkey = new SymmetricSecurityKey(byteArray);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingkey,
                ValidateIssuer = true,
                ValidIssuer = audience["issuer"],
                ValidateAudience = true,
                ValidAudience = audience["audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5)
            };
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o => o.TokenValidationParameters = tokenValidationParameters);
        }

        ///<Summary>
        /// Configure method
        ///</Summary>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            // MovieCruiser api's ui view 
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "TaxiBooking API v1.0");
            });

            //app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action}/{id?}");
            });
        }
    }
}
