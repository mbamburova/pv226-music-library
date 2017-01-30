using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Models.Mapping
{
    public class InterpretMap : EntityTypeConfiguration<Interpret>
    {
        public InterpretMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.InterpretImgUri)
                .HasMaxLength(1024);

            // Table & Column Mappings
            this.ToTable("Interprets");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.InterpretImgUri).HasColumnName("InterpretImgUri");
            this.Property(t => t.Language).HasColumnName("Language");
        }
    }
}
