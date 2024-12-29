using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Models
{
    public class Request
    {
        [Key]
        public Guid RequestId { get; set; }

        // Foreign key for the user sending the request
        [Required]
        public Guid SenderId { get; set; }
        public User Sender { get; set; }

        // Foreign key for the user receiving the request
        [Required]
        public Guid ReceiverId { get; set; }
        public User Receiver { get; set; }

        [Required]
        public string Message { get; set; } = "Please add into my Family.";

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Approved, or Rejected

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } // Nullable for updates like acceptance

        // Method to update the request status
        public void UpdateStatus(string newStatus)
        {
            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
