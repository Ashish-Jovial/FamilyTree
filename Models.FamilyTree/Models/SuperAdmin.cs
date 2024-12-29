﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Models
{
    public class SuperAdmin
    {
        [Key]
        public Guid SuperAdminID { get; set; }
        public ICollection<Guid> FamilyId { get; set; }
        public ICollection<Guid> Users { get; set; }
        public ICollection<Guid> Requests { get; set; }
        public ICollection<Guid> Roles { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; } // Soft delete flag
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }

    }
}