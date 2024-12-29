using Models.FamilyTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Interfaces
{
    public interface IUser
    {
        Guid UserID { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string UserName { get; set; } // Unique username
        string Email { get; set; }
        public byte[] PasswordHash { get; set; } // Hashed password
        public byte[] PasswordSalt { get; set; } // Salt used to hash the password
        DateTime DateOfBirth { get; set; }
        string Gender { get; set; }
        bool IsDeleted { get; set; } // Soft delete flag
        DateTime? DeletionDate { get; set; }
        Guid? DeletedByAdminID { get; set; } // Super Admin who deleted the user

        ICollection<IFamilyMember> FamilyMemberships { get; set; }
        IUserPersonalDetails PersonalDetails { get; set; }
        IUserProfessionalDetails ProfessionalDetails { get; set; }
    }

}
