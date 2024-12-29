using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Interfaces
{
    public interface IFamilyMember
    {
        Guid FamilyMemberID { get; set; }
        Guid FamilyID { get; set; }
        Guid UserID { get; set; }
        string Role { get; set; } // 'Member', 'Head', 'Admin'
        DateTime AddedDate { get; set; }
        bool IsActive { get; set; }

        IFamily Family { get; set; }
        IUser User { get; set; }
    }

}
