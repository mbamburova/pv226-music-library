using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Models.Mapping
{
    public class SongReviewMap : EntityTypeConfiguration<SongReview>
    {
        public SongReviewMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("SongReviews");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Rating).HasColumnName("Rating");
            this.Property(t => t.SongId).HasColumnName("SongId");
            this.Property(t => t.UserId).HasColumnName("UserId");

            // Relationships
            this.HasRequired(t => t.Song)
                .WithMany(t => t.SongReviews)
                .HasForeignKey(d => d.SongId);
            this.HasRequired(t => t.User)
                .WithMany(t => t.SongReviews)
                .HasForeignKey(d => d.UserId);

        }
    }
}
