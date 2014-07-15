using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class ActivityMap : EntityTypeConfiguration<Activity>
    {
        public ActivityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(1000);

            this.Property(t => t.Address)
                .HasMaxLength(30);

            this.Property(t => t.CoverPoto)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Activity");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.AreaId).HasColumnName("AreaId");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.OperationId).HasColumnName("OperationId");
            this.Property(t => t.CoverPoto).HasColumnName("CoverPoto");

            // Relationships
            this.HasOptional(t => t.Area)
                .WithMany(t => t.Activities)
                .HasForeignKey(d => d.AreaId);
            this.HasRequired(t => t.OperationInfo)
                .WithMany(t => t.Activities)
                .HasForeignKey(d => d.OperationId);

        }
    }
}
