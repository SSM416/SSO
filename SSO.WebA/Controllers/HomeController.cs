using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using SSO.WebA.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace SSO.WebA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly JwtConfig jwtModel = null;
        public HomeController(ILogger<HomeController> logger, IOptions<JwtConfig> _jwtModel)
        {
            _logger = logger;
            jwtModel = _jwtModel.Value;
        }

        private string webUrl = "http://localhost:6433";

        private string token = "";

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string UserName)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    //地址

                    string path = $"{webUrl}/value/GetToken?userName=小胖&password=111";

                    token = await client.DownloadStringTaskAsync(path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { code = 0, token = token });
            //var userClaims = new List<Claim>()
            //            {
            //            new Claim(ClaimTypes.Name,UserName)
            //            };
            //var grandmaIdentity = new ClaimsIdentity(userClaims, "myCookie");
            //var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
            //await HttpContext.SignInAsync(userPrincipal);
            //var token = GetToken(UserName);
            //return RedirectToAction("Privacy");
        }
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        [HttpPost]
        public string GetToken()
        {
            #region MyRegion


            ////下面代码自行封装
            //var claims = new List<Claim>();
            //claims.AddRange(new[]
            //{
            //    new Claim("UserName", UserName),
            //    new Claim(JwtRegisteredClaimNames.Sub, UserName),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            //});

            //DateTime now = DateTime.UtcNow;
            //var jwtSecurityToken = new JwtSecurityToken(
            //    issuer: jwtModel.Issuer,
            //    audience: jwtModel.Audience,
            //    claims: claims,
            //    notBefore: now,
            //    expires: now.Add(TimeSpan.FromMinutes(jwtModel.Expiration)),
            //    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtModel.SecurityKey)), SecurityAlgorithms.HmacSha256)
            //);

            //string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            //return token;
            #endregion
            
            try
            {
                using (WebClient client = new WebClient())
                {
                    //地址
                    string path = $"{webUrl}/api/Values/GetToken";
                    //string path = $"{webUrl}/Home/GetToken";
                    token = client.DownloadString(path);
                    //value = await client.DownloadStringTaskAsync(path);

                }
                return token;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public static string HttpGet(string url)
        {
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            client.Encoding = Encoding.UTF8;
            string result = client.DownloadString(url);
            return result;
        }

        [AllowAnonymous]
        [HttpPost]
        public string GetValue(string toknen_) 
        {
            var sss = token;
            string value = "";
            try
            {
                using (WebClient client = new WebClient())
                {
                    //地址
                    string path = $"{webUrl}/api/Values/Get";

                    client.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {toknen_}");
                    value = client.DownloadString(path);
                    //value = await client.DownloadStringTaskAsync(path);

                }
                return value;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public IActionResult Privacy()
        {
            var identity = User.Identity as ClaimsIdentity;
            var userAccessToken = identity.Name;

            var userAssertion = new UserAssertion(userAccessToken);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
