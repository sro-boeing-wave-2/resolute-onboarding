﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnBoarding.Models
{

    public class User
    {
        [Key]
        public long Id { get ; set ; }
        public string Name { get ; set ; }
        public string Email { get ; set ; }
        public Organisation Organization { get ; set ; }
        public DateTime CreatedOn { get ; set ; }
        public long CreatedBy { get ; set ; }
        public DateTime UpdatedOn { get ; set ; }
        public long UpdatedBy { get ; set ; }
        public string PhoneNumber { get ; set ; }
        public string ProfileImgUrl { get ; set ; }
    }



    public class Agent : User { }

    public class EndUser : User { }

    public class Organisation
    {
        [Key]
        public long Id { get ; set ; } 
        public string Email { get ; set ; }
        public string Password { get ; set ; }
        public DateTime CreatedOn { get ; set ; }
        public long CreatedBy { get ; set ; }
        public DateTime UpdatedOn { get ; set ; }
        public long UpdatedBy { get ; set ; }
        public string OrganisationName { get ; set ; }
        public string OrganisationDisplayName { get ; set ; }
    }
}
