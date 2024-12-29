using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Models
{
    //    Super Admin Model for Handling Verification of Deaths
    //The super admin should be able to verify and delete users logically.The SuperAdmin class will handle this process.
    public class SuperAdmin
    {
        [Key]
        public Guid SuperAdminID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public virtual ICollection<DeletionRequest> DeletionRequests { get; set; }

        public SuperAdmin()
        {
            DeletionRequests = new List<DeletionRequest>();
        }
    }

    public class DeletionRequest
    {
        [Key]
        public Guid RequestID { get; set; }
        public Guid UserID { get; set; } // User to be deleted logically
        public Guid VerifiedByAdminID { get; set; } // SuperAdmin who verified the death
        public DateTime VerifiedDate { get; set; }

        public virtual User User { get; set; }
        public virtual SuperAdmin VerifiedBy { get; set; }
    }

}


//Validations and Business Logic
//1. Notify Super Admin of a Death
//If a family member passes away, the head of the family creates a request to notify the super admin, who verifies the death.

//Pseudo-Code for Service Layer:

//public void NotifyDeath(Guid headOfFamilyID, Guid deceasedMemberID)
//{
//    // Ensure the user is the head of the family
//    var headOfFamily = familyRepository.GetHeadOfFamily(headOfFamilyID);

//    if (headOfFamily == null)
//    {
//        throw new InvalidOperationException("Only the head of the family can report a death.");
//    }

//    // Create a death notification request for super admin
//    var deathNotification = new FamilyHeadChangeRequest
//    {
//        RequestID = Guid.NewGuid(),
//        FamilyID = headOfFamily.FamilyID,
//        RequestedByID = headOfFamily.UserID,
//        DeceasedMemberID = deceasedMemberID,
//        IsDeathNotification = true,
//        DeathReportedDate = DateTime.Now,
//        ApprovalStatus = "Pending"
//    };

//    familyHeadChangeRequestRepository.Add(deathNotification);
//}


//2.Super Admin Verifies and Logically Deletes the User
//The super admin verifies the death and logically deletes the user from the system, ensuring the IsDeleted flag is set to true.

//Pseudo-Code for Service Layer:

//public void VerifyAndDeleteUser(Guid superAdminID, Guid deceasedUserID)
//{
//    var user = userRepository.GetById(deceasedUserID);

//    if (user == null || user.IsDeleted)
//    {
//        throw new InvalidOperationException("User either does not exist or is already deleted.");
//    }

//    // Verify and logically delete the user
//    user.IsDeleted = true;
//    user.DeletionDate = DateTime.Now;
//    user.DeletedByAdminID = superAdminID;

//    // Create a deletion request record for tracking
//    var deletionRequest = new DeletionRequest
//    {
//        RequestID = Guid.NewGuid(),
//        UserID = user.UserID,
//        VerifiedByAdminID = superAdminID,
//        VerifiedDate = DateTime.Now
//    };

//    deletionRequestRepository.Add(deletionRequest);
//}

//Additional Rules:
//Head of Family Notifications: The head of the family must notify the super admin of the family member's passing, and the system will ensure only the head of the family has this privilege.
//Super Admin Verification: The super admin manually verifies the death before deleting the user logically. This is an additional safeguard to ensure accuracy.
//Soft Delete for Users: When a user is deleted, they are logically deleted from the system (IsDeleted = true), meaning the user data is preserved but marked inactive.
//By adding these validations, the system will handle the lifecycle of users more effectively while maintaining control and auditability through the super admin role.