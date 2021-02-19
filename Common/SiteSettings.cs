using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class SiteSettings
    {
        public string ElmahPath { get; set; }
        public JWTSettings JWTSettings { get; set; }
    }
    public class JWTSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int    ExpirationTime { get; set; }
    }
}
