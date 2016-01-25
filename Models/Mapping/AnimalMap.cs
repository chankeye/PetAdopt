using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PetAdopt.Models.Mapping
{
    public class AnimalMap : EntityTypeConfiguration<Animal>
    {
        public AnimalMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Introduction)
                .IsRequired();

            this.Property(t => t.Address)
                .HasMaxLength(30);

            this.Property(t => t.Phone)
                .HasMaxLength(10);

            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Animal");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CoverPhoto).HasColumnName("CoverPhoto");
            this.Property(t => t.Introduction).HasColumnName("Introduction");
            this.Property(t => t.Age).HasColumnName("Age");
            this.Property(t => t.SheltersId).HasColumnName("SheltersId");
            this.Property(t => t.AreaId).HasColumnName("AreaId");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.StatusId).HasColumnName("StatusId");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.OperationId).HasColumnName("OperationId");
            this.Property(t => t.ClassId).HasColumnName("ClassId");
            this.Property(t => t.Title).HasColumnName("Title");

            // Relationships
            this.HasMany(t => t.Messages)
                .WithMany(t => t.Animals)
                .Map(m =>
                    {
                        m.ToTable("Animal_Message_Mapping");
                        m.MapLeftKey("AnimalId");
                        m.MapRightKey("MessageId");
                    });

            this.HasMany(t => t.Pictures)
                .WithMany(t => t.Animals)
                .Map(m =>
                    {
                        m.ToTable("Animal_Picture_Mapping");
                        m.MapLeftKey("AnimalId");
                        m.MapRightKey("PictureId");
                    });

            this.HasOptional(t => t.Area)
                .WithMany(t => t.Animals)
                .HasForeignKey(d => d.AreaId);
            this.HasRequired(t => t.Class)
                .WithMany(t => t.Animals)
                .HasForeignKey(d => d.ClassId);
            this.HasRequired(t => t.OperationInfo)
                .WithMany(t => t.Animals)
                .HasForeignKey(d => d.OperationId);
            this.HasOptional(t => t.Shelter)
                .WithMany(t => t.Animals)
                .HasForeignKey(d => d.SheltersId);
            this.HasRequired(t => t.Status)
                .WithMany(t => t.Animals)
                .HasForeignKey(d => d.StatusId);

        }
    }
}
