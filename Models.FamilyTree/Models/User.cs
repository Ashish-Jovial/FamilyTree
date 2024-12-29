using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Models
{

    //Enhanced POCO Models with New Validations
    //1. User POCO Model - Adding a Soft Delete Field
    //We'll introduce a IsDeleted flag to logically delete users rather than physically removing them from the database.
    public class User
    {
        [Key]
        public Guid UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; } // Unique username
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; } // Hashed password
        public byte[] PasswordSalt { get; set; } // Salt used to hash the password
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } // 'M' or 'F'
        public string PanCard { get; set; }
        public string AadharCard { get; set; }
        public string VoterCard { get; set; }
        public string StudentIdCard { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; } // Soft delete flag
        public DateTime? DeletionDate { get; set; } // Date of logical deletion
        public Guid? DeletedByAdminID { get; set; } // Super Admin who deleted the user

        public virtual ICollection<FamilyMember> FamilyMemberships { get; set; }
        public virtual UserPersonalDetails PersonalDetails { get; set; }
        public virtual UserProfessionalDetails ProfessionalDetails { get; set; }

        public User()
        {
            FamilyMemberships = new List<FamilyMember>();
        }
    }


}
