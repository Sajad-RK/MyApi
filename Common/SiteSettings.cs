using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class SiteSettings
    {
        public string ElmahPath { get; set; }
        public JWTSettings JWTSettings { get; set; }
        public IdentitySettings IdentitySettings { get; set; }
    }
    public class IdentitySettings
    {
        public bool PasswordRequireDigit { get; set; }
        public int PasswordRequireLength { get; set; }
        public bool PasswordRequireNonAlphanum { get; set; }
        public bool PasswordRequireUppercase { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public bool RequireUniqueEmail { get; set; }
    }
    public class JWTSettings
    {
        public string SecretKey { get; set; }
        public string EncryptKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationTime { get; set; }
    }
}
