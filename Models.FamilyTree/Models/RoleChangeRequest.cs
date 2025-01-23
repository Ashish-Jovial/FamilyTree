using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Models
{
    public class RoleChangeRequest
    {
        [Key]
        public Guid RequestId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string RequestedRole { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
