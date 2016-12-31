using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using AMPSystem.Models;

namespace AMPSystem.DAL
{
    /// <summary>
    ///     Class that create the references to the DB
    /// </summary>
    public class AmpDbContext : DbContext
    {
        public AmpDbContext() : base("AmpDbContext")
        {
            Database.SetInitializer(new AmpInitializer());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<EvaluationMoment> EvaluationMoments { get; set; }
        public DbSet<OfficeHour> OfficeHours { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Building> Buildings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}