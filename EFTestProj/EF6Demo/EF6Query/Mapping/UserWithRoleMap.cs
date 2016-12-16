using EF6Query.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EF6Query.Mapping
{
    public class UserWithRoleMap : EntityTypeConfiguration<UserWithRole>
    {
        public UserWithRoleMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserID, t.RoleID });

            // Properties

            // Table & Column Mappings
            this.ToTable("vUserWithRole");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.RoleName).HasColumnName("RoleName");
        }
    }
}
