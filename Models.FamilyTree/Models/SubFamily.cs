using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Models
{
    public class SubFamily
    {
        [Key]
        public Guid SubFamilyID { get; set; }
        public Guid ParentFamilyID { get; set; }
        public Guid ChildFamilyID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ApprovalStatus { get; set; } // 'Pending', 'Approved', 'Rejected'
        public Guid ApprovedBy { get; set; }

        public virtual Family ParentFamily { get; set; }
        public virtual Family ChildFamily { get; set; }
        public virtual User Approver { get; set; }
    }
}
