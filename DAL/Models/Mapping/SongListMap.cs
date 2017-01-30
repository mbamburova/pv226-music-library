using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Models.Mapping
{
    public class SongListMap : EntityTypeConfiguration<SongList>
    {
        public SongListMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("SongLists");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PlaylistId).HasColumnName("PlaylistId");
            this.Property(t => t.SongId).HasColumnName("SongId");

            // Relationships
            this.HasRequired(t => t.Playlist)
                .WithMany(t => t.SongLists)
                .HasForeignKey(d => d.PlaylistId);
            this.HasRequired(t => t.Song)
                .WithMany(t => t.SongLists)
                .HasForeignKey(d => d.SongId);

        }
    }
}
