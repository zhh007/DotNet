using EF6Query.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EF6Query.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            // Primary Key
            this.HasKey(t => t.RoleID);

            // Properties
            this.Property(t => t.RoleName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Role");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.RoleName).HasColumnName("RoleName");

            // Relationships
            this.HasMany(t => t.Users)
                .WithMany(t => t.Roles)
                .Map(m =>
                {
                    m.ToTable("UserRole");
                    m.MapLeftKey("RoleID");
                    m.MapRightKey("UserID");
                });
        }
    }
}
