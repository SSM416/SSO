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
            //ע�����ݿ�����
            services.AddScoped<DbContext, LeoGXGDBContext>();
            string LeoGXGDB_base_connection = Configuration.GetConnectionString("LeoGXGDBContextReadDataBase");
            services.AddDbContext<LeoGXGDBContext>(options => options.UseSqlServer(LeoGXGDB_base_connection));

            services.AddDataProtection()
                .SetApplicationName("SSO");  //��������ϵͳ������Ϊͳһ��Ӧ������
            //cookies
            services.AddAuthentication("CookieAuthentication")
                .AddCookie("CookieAuthentication", options =>
                {
                    options.Cookie.Name = "myCookie";//����ͳһ��Cookie����
                    options.LoginPath = "/Home/Index";
                    options.Cookie.Domain = "localhost";//����Cookie����Ϊ���������������򶼿��Է������Cookie
                    options.ExpireTimeSpan = new TimeSpan(1, 0, 0);//Ĭ��14��
                }
                ).AddJwtBearer("JwtBearer", options =>
                {
                    options.Audience = Configuration["Authentication:JwtBearer:Audience"];

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // ǩ����Կ����ƥ�䣡
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Authentication:JwtBearer:SecurityKey"])),
                        // ��֤JWT�䷢�ߣ�iss������
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Authentication:JwtBearer:Issuer"],

                        // ��֤JWT���ڣ�aud������
                        ValidateAudience = true,
                        ValidAudience = Configuration["Authentication:JwtBearer:Audience"],

                        // �Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
                        ValidateLifetime = true,

                        // ���������ʱ��ƫ����300�룬���������õĹ���ʱ������������ƫ�Ƶ�ʱ��ֵ�������������ڵ�ʱ��(����ʱ�� +ƫ��ֵ)��Ҳ��������Ϊ0��ClockSkew = TimeSpan.Zero
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
