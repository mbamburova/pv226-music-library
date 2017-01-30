using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DAL.Models.Mapping;

namespace DAL.Models
{
    public partial class MusicLibrary4Context : DbContext
    {
        static MusicLibrary4Context()
        {
            Database.SetInitializer<MusicLibrary4Context>(null);
        }

        public MusicLibrary4Context()
            : base("Name=MusicLibrary4Context")
        {
        }

        public DbSet<AlbumReview> AlbumReviews { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Interpret> Interprets { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<SongList> SongLists { get; set; }
        public DbSet<SongReview> SongReviews { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AlbumReviewMap());
            modelBuilder.Configurations.Add(new AlbumMap());
            modelBuilder.Configurations.Add(new EventMap());
            modelBuilder.Configurations.Add(new InterpretMap());
            modelBuilder.Configurations.Add(new PlaylistMap());
            modelBuilder.Configurations.Add(new SongListMap());
            modelBuilder.Configurations.Add(new SongReviewMap());
            modelBuilder.Configurations.Add(new SongMap());
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}
