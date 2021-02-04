using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using SSODataBase.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.WebB
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
            services.AddControllersWithViews();
            //注册数据库连接
            services.AddScoped<DbContext, LeoGXGDBContext>();
            string LeoGXGDB_base_connection = Configuration.GetConnectionString("LeoGXGDBContextReadDataBase");
            services.AddDbContext<LeoGXGDBContext>(options => options.UseSqlServer(LeoGXGDB_base_connection));

            services.AddDataProtection()
                .SetApplicationName("SSO");  //把所有子系统都设置为统一的应用名称
            //cookies
            services.AddAuthentication("CookieAuthentication")
                .AddCookie("CookieAuthentication", options =>
                {
                    options.Cookie.Name = "myCookie";//设置统一的Cookie名称
                    options.LoginPath = "/Home/Index";
                    options.Cookie.Domain = "localhost";//设置Cookie的域为根域，这样所有子域都可以发现这个Cookie
                    options.ExpireTimeSpan = new TimeSpan(1, 0, 0);//默认14天
                }
                ).AddJwtBearer("JwtBearer", options =>
                {
                    options.Audience = Configuration["Authentication:JwtBearer:Audience"];

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // 签名密钥必须匹配！
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Authentication:JwtBearer:SecurityKey"])),
                        // 验证JWT颁发者（iss）声明
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Authentication:JwtBearer:Issuer"],

                        // 验证JWT受众（aud）声明
                        ValidateAudience = true,
                        ValidAudience = Configuration["Authentication:JwtBearer:Audience"],

                        // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                        ValidateLifetime = true,

                        // 允许服务器时间偏移量300秒，即我们配置的过期时间加上这个允许偏移的时间值，才是真正过期的时间(过期时间 +偏移值)你也可以设置为0，ClockSkew = TimeSpan.Zero
                        ClockSkew = TimeSpan.Zero
                    };
                });
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    //options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
            services.Configure<JwtConfig>(Configuration.GetSection("Authentication:JwtBearer"));

            services.AddControllers().AddControllersAsServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
