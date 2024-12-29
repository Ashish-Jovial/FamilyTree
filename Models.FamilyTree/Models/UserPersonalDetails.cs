using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Models
{
    public class UserPersonalDetails
    {
        [Key]
        public Guid UserID { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Nationality { get; set; }

        public virtual User User { get; set; }
    }

}
