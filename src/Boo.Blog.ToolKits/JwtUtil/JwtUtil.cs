using Boo.Blog.ToolKits.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Boo.Blog.ToolKits.JwtUtil
{
    public static class JwtUtil
    {
        /// <summary>
        /// 生成jwtToken
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="email">用户邮箱</param>
        /// <param name="domain">域名</param>
        /// <param name="expires">有效期（分钟）</param>
        /// <param name="securityKey">密钥</param>
        /// <returns></returns>
        public static string JwtSecurityToken(string name, string email, string domain, int expires, string securityKey)
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name,name??""),
                new Claim(ClaimTypes.Email,email??""),
                new Claim(JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(expires)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}")
            };

            var key = new SymmetricSecurityKey(securityKey.SerializeUtf8());
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(
                issuer: domain,
                audience: domain,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expires),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
