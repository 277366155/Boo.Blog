using Boo.Blog.ToolKits.Extensions;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Boo.Blog.ToolKits.JwtUtil
{
    public static class JwtUtil
    {
        /// <summary>
        /// 生成jwtToken
        /// </summary>
        /// <param name="tenantCode">用户code</param>
        /// <param name="tenantName">用户名</param>
        /// <param name="domain">域名</param>
        /// <param name="expires">有效期（分钟）</param>
        /// <param name="securityKey">密钥</param>
        /// <returns></returns>
        public static string JwtSecurityToken(string tenantCode, string tenantName, string domain, int expires, string securityKey)
        {
            var claims = new[] {
            new Claim("tenantCode", tenantCode),
            new Claim("tenantName", tenantName),
            new Claim("exp", new DateTimeOffset(DateTime.Now.AddMinutes(expires)).ToUnixTimeSeconds().ToString())
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

        /// <summary>
        /// 解密jwt票据
        /// </summary>
        /// <param name="token">票据</param>
        /// <param name="secret">密钥</param>
        /// <param name="verify">是否验证过期，过期会抛出异常</param>
        /// <returns></returns>
        public static JwtSecurityDTO JwtDecodeToken(string token, string secret,bool verify=true)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
            var json = decoder.Decode(token, secret, verify);

            return json.ToObj<JwtSecurityDTO>();
        }
    }
}
