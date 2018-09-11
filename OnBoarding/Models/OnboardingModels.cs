using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnBoarding.Models
{

    public class Department
    {
        long departmentId;
        string department;
        DateTime createdOn;
        long createdBy;
        DateTime updatedOn;
        long updatedBy;

        [Key]
        public long DepartmentId { get => departmentId; set => departmentId = value; }
        public string DepartmentName { get => department; set => department = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public long CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public long UpdatedBy { get => updatedBy; set => updatedBy = value; }
    }

    public class UserSocialId
    {
        long socialId;
        string source;
        string identifier;
        DateTime createdOn;
        long createdBy;
        DateTime updatedOn;
        long updatedBy;

        [Key]
        public long SocialId { get => socialId; set => socialId = value; }
        public string Source { get => source; set => source = value; }
        public string Identifier { get => identifier; set => identifier = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public long CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public long UpdatedBy { get => updatedBy; set => updatedBy = value; }
    }

    public class User
    {
        long id;
        string name;
        string email;
        string phoneNumber;
        string profileImgUrl;
        Organisation organization;
        DateTime createdOn;
        long createdBy;
        DateTime updatedOn;
        long updatedBy;
        [Key]
        public long Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public Organisation Organization { get => organization; set => organization = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public long CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public long UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string ProfileImgUrl { get => profileImgUrl; set => profileImgUrl = value; }
    }

    public class EndUser : User
    {
        List<UserSocialId> socialId;

        public List<UserSocialId> SocialId { get => socialId; set => socialId = value; }
    }

    public class Organisation
    {
        long id;
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
        public long Id { get => id; set => id = value; } 
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string LogoUrl { get => logoUrl; set => logoUrl = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public long CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public long UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public string OrganisationName { get => organisationName; set => organisationName = value; }
        public string OrganisationDisplayName { get => organisationDisplayName; set => organisationDisplayName = value; }
    }
    public class Agent : User
    {
        Department department;

        public Department Department { get => department; set => department = value; }
    }
}
