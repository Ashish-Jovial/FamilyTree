using System.ComponentModel.DataAnnotations;

namespace Models.FamilyTree.Models
{
    public class Family
    {
        [Key]
        public Guid FamilyID { get; set; }
        public string FamilyName { get; set; } // Alphanumeric, unique, limit 18 characters
        public DateTime CreatedDate { get; set; }
        public Guid? HeadOfFamilyID { get; set; } // Foreign Key to User
        public Guid? ParentFamilyID { get; set; } // Self-reference for sub-families
        public bool IsActive { get; set; }
        public string GovFamilyID { get; set; } // Family ID provided by Government of India

        public virtual User HeadOfFamily { get; set; }
        public virtual Family ParentFamily { get; set; }
        public virtual ICollection<Family> SubFamilies { get; set; }
        public virtual ICollection<FamilyMember> FamilyMembers { get; set; }
        public virtual ICollection<FamilySetting> FamilySettings { get; set; }

        public Family()
        {
            SubFamilies = new List<Family>();
            FamilyMembers = new List<FamilyMember>();
            FamilySettings = new List<FamilySetting>();
        }
    }

}
