using System.Data.Entity;

namespace DAL
{
    public class MusicLibraryDbInitializer : DropCreateDatabaseAlways<MusicLibraryDbContext>
    {
        public override void InitializeDatabase(MusicLibraryDbContext context)
        {
            Seed(context);
        }
    }
}