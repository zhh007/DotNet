using EF6DemoFluent.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EF6DemoFluent.Mapping
{
    public class UserProfileMap : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.ProfileID);

            // Properties
            this.Property(t => t.ProfileID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Telephone)
                .HasMaxLength(50);

            this.Property(t => t.Mobilephone)
                .HasMaxLength(20);

            this.Property(t => t.Address)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("UserProfile");
            this.Property(t => t.ProfileID).HasColumnName("ProfileID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Sex).HasColumnName("Sex");
            this.Property(t => t.Birthday).HasColumnName("Birthday");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Telephone).HasColumnName("Telephone");
            this.Property(t => t.Mobilephone).HasColumnName("Mobilephone");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            // Relationships

            //User表作为主表
            this.HasRequired(t => t.User)
                .WithRequiredDependent(t => t.UserProfile);

            //UserProfile表作为主表
            //this.HasRequired(t => t.User)
            //    .WithRequiredPrincipal(t => t.UserProfile);

            //User表作为主表
            //this.HasOptional(t => t.User)
            //    .WithOptionalDependent(t => t.UserProfile);

            //UserProfile表作为主表
            //this.HasOptional(t => t.User)
            //    .WithOptionalPrincipal(t => t.UserProfile);
        }
    }
}
