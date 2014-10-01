using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class HelpMap : EntityTypeConfiguration<Help>
    {
        public HelpMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Address)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.CoverPhoto)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Help");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.ClassId).HasColumnName("ClassId");
            this.Property(t => t.AreaId).HasColumnName("AreaId");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.OperationId).HasColumnName("OperationId");
            this.Property(t => t.CoverPhoto).HasColumnName("CoverPhoto");

            // Relationships
            this.HasMany(t => t.Messages)
                .WithMany(t => t.Helps)
                .Map(m =>
                    {
                        m.ToTable("Help_Message_Mapping");
                        m.MapLeftKey("HelpId");
                        m.MapRightKey("MessageId");
                    });

            this.HasRequired(t => t.Area)
                .WithMany(t => t.Helps)
                .HasForeignKey(d => d.AreaId);
            this.HasRequired(t => t.Class)
                .WithMany(t => t.Helps)
                .HasForeignKey(d => d.ClassId);
            this.HasRequired(t => t.OperationInfo)
                .WithMany(t => t.Helps)
                .HasForeignKey(d => d.OperationId);

        }
    }
}
