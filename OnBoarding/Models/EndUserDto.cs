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
        long organisartionId;

        public long EndUserId { get => endUserId; set => endUserId = value; }
        public string Name { get => name; set => name = value; }
        public string ProfileImageUrl { get => profileImageUrl; set => profileImageUrl = value; }
        public long OrganisartionId { get => organisartionId; set => organisartionId = value; }
    }
}
