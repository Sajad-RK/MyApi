using Common;
using Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Services
{
    public class JWTService : IJWTService
    {
        private readonly IOptionsSnapshot<SiteSettings> settings;

        public JWTService(IOptionsSnapshot<SiteSettings> settings)
        {
            this.settings = settings;
        }
        public string Generate(User user)
        {
            //var secretKey = Encoding.UTF8.GetBytes("MysecretKEY123456789"); // longer than 16 characters
            var secretKey = Encoding.UTF8.GetBytes(settings.Value.JWTSettings.SecretKey); // longer than 16 characters
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature);

            var claims = _gerClaims(user);

            var descriptor = new SecurityTokenDescriptor
            {
                //Issuer = "mywebsite",
                //Audience = "mywebsite",
                //IssuedAt = DateTime.Now,
                //NotBefore = DateTime.Now.AddMinutes(0),
                //Expires = DateTime.Now.AddHours(1),
                Expires = DateTime.Now.AddMinutes(settings.Value.JWTSettings.ExpirationTime),
                SigningCredentials = signingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(descriptor);
            string token = tokenHandler.WriteToken(securityToken);
            return token;
        }
        private IEnumerable<Claim> _gerClaims(User user)
        {
            //JwtRegisteredClaimNames.Sub
            var list = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone, "09121234567"),
            };
            var roles = new Role[] { new Role { Name = "Admin" } };
            foreach (var item in roles)
            {
                list.Add(new Claim(ClaimTypes.Role, item.Name));
            }
            //list.Add(new Claim("X", "Y"));
            return list;
        }
    }
}
