using System;
using System.Collections.Generic;
using System.Text;

namespace c2c_flexiseason.Services
{
    public class Settings
    {
        public ApiSettings ApiSettings { get; set; }
        public SmartcardInfo SmartcardInfo { get; set; }
    }

    public class ApiSettings
    {
        public string BaseUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SmartcardInfo
    {
        public string SerialNumber { get; set; }
    }
}
