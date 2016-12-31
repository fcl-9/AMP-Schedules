using System.Data.Entity;

namespace AMPSystem.DAL
{
    /// <summary>
    ///     Class used to initialize the DB for the first time or ever the model changes
    /// </summary>
    public class AmpInitializer : DropCreateDatabaseIfModelChanges<AmpDbContext>
    {
        protected override void Seed(AmpDbContext context)
        {
        }
    }
}