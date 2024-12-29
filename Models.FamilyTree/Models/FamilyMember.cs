using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Models
{
    public class FamilyMember
    {
        [Key]
        public Guid FamilyMemberID { get; set; }
        public Guid FamilyID { get; set; }
        public Guid UserID { get; set; }
        public string Role { get; set; } // 'Member', 'Head', 'Admin'
        public DateTime AddedDate { get; set; }
        public bool IsActive { get; set; }

        public virtual Family Family { get; set; }
        public virtual User User { get; set; }
    }
}
