using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Models
{
    public class FamilySetting
    {
        [Key]
        public Guid SettingID { get; set; }
        public Guid FamilyID { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }

        public virtual Family Family { get; set; }
    }

}

//Business Rules and Validation
//To enforce the specific rules (e.g., no sub-family creation for families with children under 18), 
//we can add validation in the service layer of the application, using business logic:

//1) Sub - family creation check: Check the ages of all family members before allowing the creation of sub-families.
//2) Head change approval notifications: Create a notification service that sends the approval/rejection message to the previous head.
//3) Unique family name and username validation: Ensure both the family name and username are unique using constraints in the database and validation in the service layer.
//4) Government ID check: Ensure the user provides at least one valid government ID before creating or joining a family.


//Example Validation in Service Layer (Pseudo-code):

//public void CreateSubFamily(Guid familyId, Guid userId)
//{
//    var family = familyRepository.GetById(familyId);
//    var childrenUnder18 = family.FamilyMembers
//        .Where(fm => (DateTime.Now.Year - fm.User.DateOfBirth.Year) < 18)
//        .ToList();

//    if (childrenUnder18.Any())
//    {
//        throw new InvalidOperationException("Cannot create sub-family if there are children under 18.");
//    }

//    // Proceed with sub-family creation logic
//}


//UI Mapping
//The family name should map to a unique string for easy lookup and display in the UI. This can be enforced in the service layer by converting the family name into a URL-safe string and ensuring it remains unique.

//By using these POCO classes, the application structure will have a clean object-relational mapping that aligns with your SQL schema while enforcing the key rules and business logic defined in the problem statement.


//additional validations for handling scenarios like family member death, where the head of the family must inform the super admin, and the user is logically deleted upon verification.

