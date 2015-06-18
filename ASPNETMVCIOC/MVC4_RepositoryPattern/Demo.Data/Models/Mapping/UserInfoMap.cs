using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Data.Models.Mapping
{
    public class UserInfoMap : EntityTypeConfiguration<UserInfo>
    {
        public UserInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Title)
                //.IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Remark)
                //.IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("UserInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Remark).HasColumnName("Remark");

        }
    }
}
