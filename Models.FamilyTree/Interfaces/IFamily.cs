using Models.FamilyTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Interfaces
{
    public interface IFamily
    {
        Guid FamilyID { get; set; }
        string FamilyName { get; set; } // Alphanumeric, unique, limit 18 characters
        DateTime CreatedDate { get; set; }
        Guid? HeadOfFamilyID { get; set; } // Foreign Key to User
        Guid? ParentFamilyID { get; set; } // Self-reference for sub-families
        bool IsActive { get; set; }
        string GovFamilyID { get; set; } // Family ID provided by the Government of India

        ICollection<IFamily> SubFamilies { get; set; }
        ICollection<IFamilyMember> FamilyMembers { get; set; }
        ICollection<IFamilySetting> FamilySettings { get; set; }
    }

}
