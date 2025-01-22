using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Models
{
    public class Log
    {
        [Key]
        public Guid LogId { get; set; }
        public string EventType { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
