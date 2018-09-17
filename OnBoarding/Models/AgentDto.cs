using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnBoarding.Models
{
    public class AgentDto
    {
        long agentId;
        string name;
        string profileImageUrl;
        long organisartionId;
        string departmentName;
        string organisationName;

        public long AgentId { get => agentId; set => agentId = value; }
        public string Name { get => name; set => name = value; }
        public string ProfileImageUrl { get => profileImageUrl; set => profileImageUrl = value; }
        public long OrganisationId { get => organisartionId; set => organisartionId = value; }
        public string DepartmentName { get => departmentName; set => departmentName = value; }
        public string OrganisationName { get => organisationName; set => organisationName = value; }
    }
}
