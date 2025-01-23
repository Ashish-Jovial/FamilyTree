using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree
{
    public class RoleChangeRequestModel
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string RequestedRole { get; set; }
    }
}
