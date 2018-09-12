using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnBoarding.Models
{
    public class OrganisationDto
    {
        string organisationName;
        string email;
        string imageUrl;

        public string OrganisationName { get => organisationName; set => organisationName = value; }
        public string Email { get => email; set => email = value; }
        public string ImageUrl { get => imageUrl; set => imageUrl = value; }
    }
}
