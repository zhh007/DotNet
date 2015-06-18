using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Data.Models;
using Demo.Infrastructure.Data;

namespace Demo.Data
{
    public class DemoUnitOfWork : IUnitOfWork, IDisposable
    {
        private DbContext dataContext;

        public DemoUnitOfWork()
        {
            dataContext = new DemoContext();
        }
        
        public DbContext GetContext()
        {
            return dataContext;
        }

        public void Commit()
        {
            if (dataContext != null)
                dataContext.SaveChanges();
        }

        public void Dispose()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}
