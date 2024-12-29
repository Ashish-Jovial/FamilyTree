using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Interfaces
{
    public interface IUserProfessionalDetails
    {
        Guid UserID { get; set; }
        string Occupation { get; set; }
        string CompanyName { get; set; }
        string Position { get; set; }
        decimal AnnualIncome { get; set; }

        IUser User { get; set; }
    }

}
