using EF6DemoFluent.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EF6DemoFluent.Mapping
{
    public class FamilyMemberMap : EntityTypeConfiguration<FamilyMember>
    {
        public FamilyMemberMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Name)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Family");
            this.Property(t => t.ID).HasColumnName("FamilyID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Sex).HasColumnName("Sex");
            this.Property(t => t.Birthday).HasColumnName("Birthday");

            // Relationships
            this.HasMany(t => t.Parents)
                .WithMany(t => t.Children)
                .Map(m =>
                {
                    m.ToTable("FamilyMemberRelationship");
                    m.MapLeftKey("ParentID");
                    m.MapRightKey("ChildID");
                });

        }
    }
}
