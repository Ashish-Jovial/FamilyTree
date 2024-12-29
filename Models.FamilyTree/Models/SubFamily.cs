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
        public Guid SubFamilyId { get; set; }
        public Family NestedFamily { get; set; }
    }
}
