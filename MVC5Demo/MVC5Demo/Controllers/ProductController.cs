using MVC5Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVC5Demo.Controllers
{
    public class ProductController : ApiController
    {
        private static List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
        };

        // GET: api/Product
        public IEnumerable<Product> Get()
        {
            return products;
        }

        // GET: api/Product/5
        public Product Get(int id)
        {
            var product = products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                return null;
            }
            return product;
        }

        // POST: api/Product
        public HttpResponseMessage Post([FromBody]Product product)
        {
            if (ModelState.IsValid)
            {
                product.Id = (from p in products
                              orderby p.Id descending
                              select p.Id).FirstOrDefault() + 1;
                products.Add(product);

                return Request.CreateResponse(HttpStatusCode.OK, product);
                //return new HttpResponseMessage(HttpStatusCode.OK);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, GetErrors());

            //return product;
        }

        // PUT: api/Product/5
        public void Put(int id, [FromBody]Product product)
        {
            var model = (from p in products
                         where p.Id == id
                         select p).FirstOrDefault();

            if(model != null)
            {
                model.Name = product.Name;
                model.Category = product.Category;
                model.Price = product.Price;
            }
        }

        // DELETE: api/Product/5
        public void Delete(int id)
        {
            var product = products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                products.Remove(product);
            }
        }

        private string GetErrors()
        {
            List<string> errors = new List<string>();
            if (this.ModelState != null)
            {
                var query = from state in this.ModelState.Values
                            from error in state.Errors
                            select error.ErrorMessage;

                errors.AddRange(query.ToList());
            }
            return string.Join("<br />", errors);
        }
    }
}
