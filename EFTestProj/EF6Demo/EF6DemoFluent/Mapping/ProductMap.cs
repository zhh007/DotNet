using EF6DemoFluent.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EF6DemoFluent.Mapping
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductID);

            // Properties
            this.Property(t => t.ProductID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.ProductName)
                .HasMaxLength(20)
                .IsRequired();
            this.Property(t => t.UnitPrice)
                .HasPrecision(18, 4)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Product", "dbo");
            this.Property(t => t.ProductID).HasColumnName("ProductID");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.CategoryID).HasColumnName("CategoryID");

            // Relationships
            this.HasRequired(t => t.Category)
                .WithMany(t => t.Products)
                .HasForeignKey(d => d.CategoryID)
                .WillCascadeOnDelete(false);
        }
    }
}