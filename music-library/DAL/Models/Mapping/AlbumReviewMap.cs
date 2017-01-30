using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Models.Mapping
{
    public class AlbumReviewMap : EntityTypeConfiguration<AlbumReview>
    {
        public AlbumReviewMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("AlbumReviews");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Rating).HasColumnName("Rating");
            this.Property(t => t.AlbumId).HasColumnName("AlbumId");
            this.Property(t => t.UserId).HasColumnName("UserId");

            // Relationships
            this.HasRequired(t => t.Album)
                .WithMany(t => t.AlbumReviews)
                .HasForeignKey(d => d.AlbumId);
            this.HasRequired(t => t.User)
                .WithMany(t => t.AlbumReviews)
                .HasForeignKey(d => d.UserId);

        }
    }
}
