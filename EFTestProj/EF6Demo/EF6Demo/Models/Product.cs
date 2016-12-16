using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF6Demo.Models
{
    [Table("Product", Schema = "dbo")]
    public class Product
    {
        [Key]
        [Column("ProductID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        [MaxLength(20)]
        [Required, Column("ProductName")]
        public string ProductName { get; set; }

        [Column("UnitPrice", TypeName = "MONEY")]
        //不支持设置小数的精度，这时可以使用Fluent API进行设置
        public decimal UnitPrice { get; set; }

        [Column("CategoryID")]
        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        [NotMapped]
        public string Remark { get; set; }

        //[ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
    }
}