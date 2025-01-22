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
        public DbSet<Request> Requests { get; set; } = null!;
        public DbSet<SuperAdmin> SuperAdmins { get; set; } = null!;
        public DbSet<UserRoles> UserRoles { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Log> Logs { get; set; }
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
            // Configure relationships, keys, etc.\
            modelBuilder.Entity<Request>()
               .HasOne(r => r.Sender)
               .WithMany(u => u.SentRequests)
               .HasForeignKey(r => r.SenderId)
               .OnDelete(DeleteBehavior.ClientSetNull);// Prevent cascade delete for SenderId

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Receiver)
                .WithMany(u => u.ReceivedRequests)
                .HasForeignKey(r => r.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull);// Allow cascade delete for ReceiverId

            base.OnModelCreating(modelBuilder);

        }
    }

}
