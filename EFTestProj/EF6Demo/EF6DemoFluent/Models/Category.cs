using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6DemoFluent.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public int? ParentID { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }

        public Category()
        {
            this.Products = new List<Product>();
            this.Children = new List<Category>();
        }
    }
}
