using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Models
{

    //Enhanced POCO Models with New Validations
    //1. User POCO Model - Adding a Soft Delete Field
    //We'll introduce a IsDeleted flag to logically delete users rather than physically removing them from the database.
    public class User
    {
        [Key]
        public Guid UserID { get; set; }

        public ICollection<Guid> FamilyId { get; set; } = new List<Guid>();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty; // Unique username
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty; // 'M' or 'F'
        public string PanCard { get; set; } = string.Empty;
        public bool HaveFamily { get; set; }
        public string AadharCard { get; set; } = string.Empty;
        public string VoterCard { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; } = Array.Empty<byte>(); // Hashed password
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>(); // Salt used to hash the password

        // Professional information
        public List<string> CompanyName { get; set; } = new List<string>();
        public string Designation { get; set; } = string.Empty;
        public float AnnualIncome { get; set; }
        public string StudentIdCard { get; set; } = string.Empty;

        // Application roles
        public ICollection<UserRoles> Roles { get; set; } = new List<UserRoles>();

        // Requests (Actions details)
        public ICollection<Request> SentRequests { get; set; } = new List<Request>();
        public ICollection<Request> ReceivedRequests { get; set; } = new List<Request>();

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; } // Soft delete flag
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
