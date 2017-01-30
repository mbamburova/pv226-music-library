using System.Data.Entity;
using DAL.Entities;

namespace DAL
{
    public class MusicLibraryDbContext : DbContext
    {
        public MusicLibraryDbContext() : base("MusicLibrary")
        {
            InitializeDbContext();
        }

        public MusicLibraryDbContext(string connectionName) : base(connectionName)
        {
            InitializeDbContext();
        }

        public DbSet<Song> Songs { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<AlbumReview> AlbumReviews { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Interpret> Interprets { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<SongReview> SongReviews { get; set; }
        public DbSet<SongList> SongLists { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureMembershipRebootUserAccounts<UserAccount>();
        }

        private void InitializeDbContext()
        {
            Database.SetInitializer(new MusicLibraryDbInitializer());
            this.RegisterUserAccountChildTablesForDelete<UserAccount>();
        }
    }
}