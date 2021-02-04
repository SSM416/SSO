using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SSOService.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SSOService.Controllers
{
    
    //路由设置
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// 获取文本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<string> Get()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            string Sid = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value;
            string Name = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            string Role = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            return new string[] { $"Sid={Sid }, Name={Name}, Role={Role}" };
        }

        [HttpGet]
        public string GetToken(string UserName="zhangsan")
        {
            JWTTokenOptions jwtModel = new JWTTokenOptions();

            //创建用户身份标识
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Sid, UserName),
                new Claim(ClaimTypes.Name, UserName),
                new Claim(ClaimTypes.Role, "user"),
            };
            DateTime now = DateTime.UtcNow;
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtModel.Issuer,
                audience: jwtModel.Audience,
                claims: claims,
                notBefore: now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtModel.SecurityKey)), SecurityAlgorithms.HmacSha256)
            );
            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }
        /// <summary>
        /// 两数相减
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<int> Subtract(Param param)
        {
            int result = param.num1 - param.num2;
            return result;
        }
    }
    /// <summary>
    /// 参数
    /// </summary>
    public class Param
    {
        /// <summary>
        /// 第一个数
        /// </summary>
        public int num1 { get; set; }
        /// <summary>
        /// 第二个数
        /// </summary>
        public int num2 { get; set; }
    }
}
