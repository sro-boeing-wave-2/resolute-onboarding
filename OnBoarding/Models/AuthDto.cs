using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnBoarding.Models
{
    public class AuthDto
    {
        string username;
        string password;

        public string Password { get => password; set => password = value; }
        public string Username { get => username; set => username = value; }
    }
}
