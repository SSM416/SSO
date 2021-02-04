using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SSODataBase.DbModels;
using SSOService.Models;
using System;
using System.Text;

namespace SSOService
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
            JWTTokenOptions jwtTokenOptions = new JWTTokenOptions();
            services.AddControllers();
            // ע��Swagger����
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SSOServiceAPI", Version = "v1" });
            });
            //ע�����ݿ�����
            services.AddScoped<DbContext, LeoGXGDBContext>();
            string LeoGXGDB_base_connection = Configuration.GetConnectionString("LeoGXGDBContextReadDataBase");
            services.AddDbContext<LeoGXGDBContext>(options => options.UseSqlServer(LeoGXGDB_base_connection));


            //cookies
            services.AddAuthentication("CookieAuthentication").AddCookie("CookieAuthentication", options =>
            {
                options.Cookie.Name = "myCookie";//����ͳһ��Cookie����
                options.LoginPath = "/Home/Index";
                options.Cookie.Domain = "localhost";//����Cookie����Ϊ���������������򶼿��Է������Cookie
                options.ExpireTimeSpan = new TimeSpan(1, 0, 0);//Ĭ��14��
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = jwtTokenOptions.Key,

                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtTokenOptions.Audience,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
            });
            //services.Configure<JwtConfig>(Configuration.GetSection("Authentication:JwtBearer"));
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SSOServiceAPI V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
