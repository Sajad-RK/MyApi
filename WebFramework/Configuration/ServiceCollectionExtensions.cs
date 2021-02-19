using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebFramework.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddJwtAuthenticateion(this IServiceCollection services, Common.JWTSettings jWTSettings)
        {
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //var secretKey = Encoding.UTF8.GetBytes("MysecretKEY123456789");
                var secretKey = Encoding.UTF8.GetBytes(jWTSettings.SecretKey);
                //var validationParameters = new TokenValidationParameters
                //{
                //    ClockSkew = TimeSpan.Zero,  // default 5 min
                //    RequireSignedTokens = true,

                //    ValidateIssuerSigningKey = true,
                //    IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                //    RequireExpirationTime = true,
                //    ValidateLifetime = true,

                //    ValidateAudience = true,
                //    ValidAudience = "",

                //    ValidateIssuer = true,
                //    ValidIssuer = ""
                
                //};
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    TokenDecryptionKey = new SymmetricSecurityKey(secretKey)
                };
            });
        }
    }
}
