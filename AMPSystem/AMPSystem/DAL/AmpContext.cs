using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using AMPSystem.Classes;
using AMPSystem.Classes.TimeTableItems;

namespace AMPSystem.DAL
{
    public class AmpContext : DbContext
    {
        //The name of the connection string (which you'll add to the Web.config file later) 
        //is passed in to the constructor.
        public AmpContext() : base("AmpContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<EvaluationMoment> EvalMoments { get; set; }
        public DbSet<OfficeHours> OfficeHours { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}