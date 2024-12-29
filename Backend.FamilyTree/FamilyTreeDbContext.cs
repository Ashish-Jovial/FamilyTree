namespace Backend.FamilyTree
{
    using Microsoft.EntityFrameworkCore;
    using Models.FamilyTree.Models;

    public class FamilyTreeDbContext : DbContext
    {
        public FamilyTreeDbContext(DbContextOptions<FamilyTreeDbContext> options) : 
            base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<FamilyMember> FamilyMembers { get; set; }
        public DbSet<FamilyHeadChangeRequest> FamilyHeadChangeRequests { get; set; }
        public DbSet<UserPersonalDetails> UserPersonalDetails { get; set; }
        public DbSet<UserProfessionalDetails> UserProfessionalDetails { get; set; }
        public DbSet<FamilySetting> FamilySettings { get; set; }
        public DbSet<SuperAdmin> SuperAdmins { get; set; }
        public DbSet<DeletionRequest> DeletionRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Your_Connection_String_Here");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships, keys, etc.
            modelBuilder.Entity<Family>()
                .HasMany(f => f.FamilyMembers)
                .WithOne(fm => fm.Family)
                .HasForeignKey(fm => fm.FamilyID);

            modelBuilder.Entity<FamilyMember>()
                .HasOne(fm => fm.User)
                .WithMany(u => u.FamilyMemberships)
                .HasForeignKey(fm => fm.UserID);

            modelBuilder.Entity<FamilyHeadChangeRequest>()
                .HasOne(fh => fh.RequestedBy)
                .WithMany()
                .HasForeignKey(fh => fh.RequestedByID);
        }
    }

}
