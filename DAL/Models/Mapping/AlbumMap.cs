using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Models.Mapping
{
    public class AlbumMap : EntityTypeConfiguration<Album>
    {
        public AlbumMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.AlbumImgUri)
                .HasMaxLength(1024);

            this.Property(t => t.Name)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Albums");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Year).HasColumnName("Year");
            this.Property(t => t.Duration).HasColumnName("Duration");
            this.Property(t => t.AlbumImgUri).HasColumnName("AlbumImgUri");
            this.Property(t => t.InterpretId).HasColumnName("InterpretId");
            this.Property(t => t.Name).HasColumnName("Name");

            // Relationships
            this.HasRequired(t => t.Interpret)
                .WithMany(t => t.Albums)
                .HasForeignKey(d => d.InterpretId);

        }
    }
}
