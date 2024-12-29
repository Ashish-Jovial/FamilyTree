using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Interfaces
{
    public interface IUserPersonalDetails
    {
        Guid UserID { get; set; }
        string Address { get; set; }
        string PhoneNumber { get; set; }
        string Nationality { get; set; }

        IUser User { get; set; }
    }

}
