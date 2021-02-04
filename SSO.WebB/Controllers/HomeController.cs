using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SSO.WebB.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SSO.WebB.Controllers
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
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string UserName)
        {
            var userClaims = new List<Claim>()
                        {
                        new Claim(ClaimTypes.Name,UserName)
                        };
            var grandmaIdentity = new ClaimsIdentity(userClaims, "myCookie");
            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
            await HttpContext.SignInAsync(userPrincipal);
            //if (UserName == "123")
            //{

            //    var token = GetToken(UserName);
            //}
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult GetToken(string UserName)
        {
            //下面代码自行封装
            var claims = new List<Claim>();
            claims.AddRange(new[]
            {
                new Claim("UserName", UserName),
                new Claim(JwtRegisteredClaimNames.Sub, UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            DateTime now = DateTime.UtcNow;
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtModel.Issuer,
                audience: jwtModel.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(jwtModel.Expiration)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtModel.SecurityKey)), SecurityAlgorithms.HmacSha256)
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return Ok(new
            {
                access_token = token,
                token_type = "Bearer"
            });
            //return token;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
