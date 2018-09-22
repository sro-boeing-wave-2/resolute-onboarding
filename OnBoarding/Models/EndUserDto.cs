using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnBoarding.Models
{
    public class EndUserDto
    {
        long endUserId;
        string name;
        string profileImageUrl;
        long organisationId;
        string organisationEmail;
        public long EndUserId { get => endUserId; set => endUserId = value; }
        public string Name { get => name; set => name = value; }
        public string ProfileImageUrl { get => profileImageUrl; set => profileImageUrl = value; }
        public long OrganisationId { get => organisationId; set => organisationId = value; }
        public string OrganisationEmail { get => organisationEmail; set => organisationEmail = value; }
    }
}
