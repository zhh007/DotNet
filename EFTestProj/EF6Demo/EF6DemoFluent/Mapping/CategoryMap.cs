using EF6DemoFluent.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EF6DemoFluent.Mapping
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.CategoryName)
                .HasMaxLength(20)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Category", "dbo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CategoryName).HasColumnName("CategoryName");

            // Relationships
            this.HasOptional(t => t.Parent)
                .WithMany(d => d.Children)
                .HasForeignKey(t => t.ParentID);

        }
    }
}
