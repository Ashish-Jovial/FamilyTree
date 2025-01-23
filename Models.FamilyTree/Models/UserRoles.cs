using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Models
{
    public class UserRoles
    {
        [Key]
        public Guid RoleID { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; } // Soft delete flag
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();

    }
}
