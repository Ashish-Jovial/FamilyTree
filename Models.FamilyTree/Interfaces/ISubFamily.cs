using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Interfaces
{
    public interface ISubFamily
    {
        Guid SubFamilyID { get; set; }
        Guid ParentFamilyID { get; set; }
        Guid ChildFamilyID { get; set; }
        DateTime CreatedDate { get; set; }
        string ApprovalStatus { get; set; } // 'Pending', 'Approved', 'Rejected'
        Guid ApprovedBy { get; set; }

        IFamily ParentFamily { get; set; }
        IFamily ChildFamily { get; set; }
        IUser Approver { get; set; }
    }

}
