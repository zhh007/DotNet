namespace EF6DemoFluent.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }

        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }

        public string Remark { get; set; }
    }
}