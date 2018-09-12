using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OnBoarding.Models
{

    public class Department
    {
        int departmentId;
        string department;
        DateTime createdOn;
        long createdBy;
        DateTime updatedOn;
        long updatedBy;

        [Key]
        public int DepartmentId { get => departmentId; set => departmentId = value; }
        public string DepartmentName { get => department; set => department = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public long CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public long UpdatedBy { get => updatedBy; set => updatedBy = value; }
    }


    public class UserSocialId
    {
        int socialId;
        string source;
        string identifier;
        DateTime createdOn;
        long createdBy;
        DateTime updatedOn;
        long updatedBy;

        [Key]
        public int SocialId { get => socialId; set => socialId = value; }
        public string Source { get => source; set => source = value; }
        public string Identifier { get => identifier; set => identifier = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public long CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public long UpdatedBy { get => updatedBy; set => updatedBy = value; }
    }

    public class User
    {
        int id;
        string name;
        string email;
        string phonenumber;
        string profileimgurl;
        Organisation organization;
        DateTime createdOn;
        long createdBy;
        DateTime updatedOn;
        long updatedBy;
        [Key]
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Phonenumber { get => phonenumber; set => phonenumber = value; }
        public string ProfileImgUrl { get => profileimgurl; set => profileimgurl = value; }
        public Organisation Organization { get => organization; set => organization = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public long CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public long UpdatedBy { get => updatedBy; set => updatedBy = value; }
    }

    public class EndUser : User
    {
        List<UserSocialId> socialId;
        public List<UserSocialId> SocialId { get => socialId; set => socialId = value; }
    }

    public class Organisation
    {
        int id;
        string organisationName;
        string organisationDisplayName;
        string email;
        string password;
        string logoUrl;
        DateTime createdOn;
        long createdBy;
        DateTime updatedOn;
        long updatedBy;
        [Key]
        public int Id { get => id; set => id = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public long CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public long UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public string LogoUrl { get => logoUrl; set => logoUrl = value; }
        public string OrganisationName { get => organisationName; set => organisationName = value; }
        public string OrganisationDisplayName { get => organisationDisplayName; set => organisationDisplayName = value; }
    }

    public class Agent : User
    {
        Department department;
        String role;
        public Department Department { get => department; set => department = value; }
        public string Role { get => role; set => role = value; }
    }
}