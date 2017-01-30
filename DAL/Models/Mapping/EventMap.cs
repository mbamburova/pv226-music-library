using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Models.Mapping
{
    public class EventMap : EntityTypeConfiguration<Event>
    {
        public EventMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(1024);

            this.Property(t => t.Place)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.EventLink)
                .HasMaxLength(1024);

            // Table & Column Mappings
            this.ToTable("Events");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Time).HasColumnName("Time");
            this.Property(t => t.Place).HasColumnName("Place");
            this.Property(t => t.EventLink).HasColumnName("EventLink");
            this.Property(t => t.InterpretId).HasColumnName("InterpretId");

            // Relationships
            this.HasRequired(t => t.Interpret)
                .WithMany(t => t.Events)
                .HasForeignKey(d => d.InterpretId);

        }
    }
}
