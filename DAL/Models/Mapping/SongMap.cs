using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Models.Mapping
{
    public class SongMap : EntityTypeConfiguration<Song>
    {
        public SongMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.YTLink)
                .HasMaxLength(1024);

            // Table & Column Mappings
            this.ToTable("Songs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Genre).HasColumnName("Genre");
            this.Property(t => t.Duration).HasColumnName("Duration");
            this.Property(t => t.Added).HasColumnName("Added");
            this.Property(t => t.YTLink).HasColumnName("YTLink");
            this.Property(t => t.Size).HasColumnName("Size");
            this.Property(t => t.Lyrics).HasColumnName("Lyrics");
            this.Property(t => t.Album_Id).HasColumnName("Album_Id");

            // Relationships
            this.HasOptional(t => t.Album)
                .WithMany(t => t.Songs)
                .HasForeignKey(d => d.Album_Id);

        }
    }
}
