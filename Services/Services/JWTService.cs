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
            var secretKey = Encoding.UTF8.GetBytes(settings.Value.JWTSettings.SecretKey); // longer than 16 characters
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionKey = Encoding.UTF8.GetBytes(settings.Value.JWTSettings.EncryptKey);
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionKey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = _getClaims(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = settings.Value.JWTSettings.Issuer,
                Audience = settings.Value.JWTSettings.Audience,
                IssuedAt = DateTime.Now,
                //NotBefore = DateTime.Now.AddMinutes(settings.Value.JWTSettings.NotBeforeMinutes),
                Expires = DateTime.Now.AddMinutes(settings.Value.JWTSettings.ExpirationTime),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(descriptor);

            var encryptedJwt = tokenHandler.WriteToken(securityToken);

            return encryptedJwt;
        }
        private IEnumerable<Claim> _getClaims(User user)
        {
            var list = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone, "09121234567"),
            };
            var roles = new Role[] { new Role { Name = "Admin" } };
            foreach (var item in roles)
            {
                list.Add(new Claim(ClaimTypes.Role, item.Name));
            }
            return list;
        }
    }
}
