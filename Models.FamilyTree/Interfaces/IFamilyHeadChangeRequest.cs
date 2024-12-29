using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Interfaces
{
    public interface IFamilyHeadChangeRequest
    {
        Guid RequestID { get; set; }
        Guid FamilyID { get; set; }
        Guid RequestedByID { get; set; }
        Guid ProposedHeadID { get; set; }
        Guid PreviousHeadID { get; set; }
        DateTime RequestDate { get; set; }
        string ApprovalStatus { get; set; } // 'Pending', 'Approved', 'Rejected'

        // Death notification fields
        bool IsDeathNotification { get; set; }
        Guid? DeceasedMemberID { get; set; }
        DateTime? DeathReportedDate { get; set; }

        IFamily Family { get; set; }
        IUser RequestedBy { get; set; }
        IUser ProposedHead { get; set; }
        IUser PreviousHead { get; set; }
    }

}
