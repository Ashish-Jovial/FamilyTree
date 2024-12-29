using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Interfaces
{
    public interface IDeletionRequest
    {
        Guid RequestID { get; set; }
        Guid UserID { get; set; } // User to be deleted logically
        Guid VerifiedByAdminID { get; set; } // SuperAdmin who verified the death
        DateTime VerifiedDate { get; set; }

        IUser User { get; set; }
        ISuperAdmin VerifiedBy { get; set; }
    }

}

//Benefits of Using Interfaces:
//Separation of Concerns: Interfaces define the contract that each model must fulfill, allowing for easier testing and substitution of different implementations if needed.
//Loose Coupling: The application components are not tightly bound to the specific implementation of these models, making the system more flexible and extensible.
//Adherence to SOLID Principles: Specifically, the Interface Segregation Principle (ISP) is enforced, as the interfaces are fine-grained and focused on specific responsibilities.
//These interfaces give you the flexibility to swap out the underlying implementations of these models without affecting the business logic or other parts of the system. It also enhances testability and maintainability.