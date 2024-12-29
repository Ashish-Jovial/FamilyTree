using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Models
{
    public class UserProfessionalDetails
    {
        [Key]
        public Guid UserID { get; set; }
        public string Occupation { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public float AnnualIncome { get; set; }

        public virtual User User { get; set; }
    }

}
