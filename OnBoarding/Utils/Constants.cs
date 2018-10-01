using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnBoarding.Utils
{
    class Constants
    {
        public static string BASE_URL = "http://" + Environment.GetEnvironmentVariable("MACHINE_LOCAL_IPV4");
        public static string POST_AUTHDATA = "/user/add";
    }
}
