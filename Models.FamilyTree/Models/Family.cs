using System.ComponentModel.DataAnnotations;

namespace Models.FamilyTree.Models
{
    public class Family
    {
        [Key]
        public Guid FamilyID { get; set; }
        public string FamilyName { get; set; } // Alphanumeric, unique, limit 18 characters
        public Guid? HeadOfFamilyID { get; set; } // user id will be added for a family.
        public Guid? ParentFamilyID { get; set; } // Self-reference for sub-families Family Id will be enter here.
        public bool IsActive { get; set; }
        public string GovFamilyID { get; set; } // Family ID provided by Government of India
        public virtual ICollection<SubFamily> SubFamilies { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; } // Soft delete flag
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }

        public Family()
        {
            SubFamilies = [];
        }
    }

}
