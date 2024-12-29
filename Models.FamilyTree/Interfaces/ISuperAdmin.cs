using Models.FamilyTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Interfaces
{
    public interface ISuperAdmin
    {
        Guid SuperAdminID { get; set; }
        string FullName { get; set; }
        string Email { get; set; }
        string PasswordHash { get; set; }

        ICollection<IDeletionRequest> DeletionRequests { get; set; }
    }

}
