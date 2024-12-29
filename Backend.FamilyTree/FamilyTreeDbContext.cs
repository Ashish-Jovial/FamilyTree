namespace Backend.FamilyTree
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Models.FamilyTree.Models;

    public class FamilyTreeDbContext : DbContext
    {

        public FamilyTreeDbContext(DbContextOptions<FamilyTreeDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Family> Families { get; set; } = null!;
        public DbSet<FamilyMember> FamilyMembers { get; set; } = null!;
        public DbSet<FamilyHeadChangeRequest> FamilyHeadChangeRequests { get; set; } = null!;
        public DbSet<UserPersonalDetails> UserPersonalDetails { get; set; } = null!;
        public DbSet<UserProfessionalDetails> UserProfessionalDetails { get; set; } = null!;
        public DbSet<FamilySetting> FamilySettings { get; set; } = null!;
        public DbSet<SuperAdmin> SuperAdmins { get; set; } = null!;
        public DbSet<DeletionRequest> DeletionRequests { get; set; } = null!;

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        //        if (string.IsNullOrEmpty(connectionString))
        //        {
        //            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        //        }
        //        optionsBuilder.UseSqlServer(connectionString);
        //    }
        //}

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
