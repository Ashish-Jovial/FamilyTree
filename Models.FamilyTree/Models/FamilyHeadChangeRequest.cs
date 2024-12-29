using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Models
{
    //FamilyHeadChangeRequest POCO Model - Add Notification for Member Death
    // We'll enhance the FamilyHeadChangeRequest model to manage death notifications to the super admin.
    public class FamilyHeadChangeRequest
    {
        [Key]
        public Guid RequestID { get; set; }
        public Guid FamilyID { get; set; }
        public Guid RequestedByID { get; set; }
        public Guid ProposedHeadID { get; set; }
        public Guid PreviousHeadID { get; set; }
        public DateTime RequestDate { get; set; }
        public string ApprovalStatus { get; set; } // 'Pending', 'Approved', 'Rejected'

        // New fields for handling death notifications
        public bool IsDeathNotification { get; set; }
        public Guid? DeceasedMemberID { get; set; } // UserID of deceased member
        public DateTime? DeathReportedDate { get; set; }

        public virtual Family Family { get; set; }
        public virtual User RequestedBy { get; set; }
        public virtual User ProposedHead { get; set; }
        public virtual User PreviousHead { get; set; }
    }
}
