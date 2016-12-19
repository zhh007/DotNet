using EF6Query.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6Query.Samples
{
    public class s7_Concurrency
    {
        public static void Run()
        {
            init();

            using (EF6QueryContext ctx = new EF6QueryContext())
            {
                var product = new Product();
                product.Name = "肥皂";
                product.Count = 10;
                ctx.Products.Add(product);
                ctx.SaveChanges();

                bool executeUpdate = false;
                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        product.Count = product.Count - 1;

                        if (!executeUpdate)
                        {
                            ctx.Database.ExecuteSqlCommand("UPDATE Product SET Count = 5 WHERE Name = '肥皂'");
                            executeUpdate = true;
                        }
                        ctx.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;

                        // Update the values of the entity that failed to save from the store
                        ex.Entries.Single().Reload();
                    }

                } while (saveFailed);

            }
        }

        public static void init()
        {
            using (EF6QueryContext ctx = new EF6QueryContext())
            {
                var list = ctx.Products.ToList();
                ctx.Products.RemoveRange(list);
                ctx.SaveChanges();
            }
        }
    }
}
