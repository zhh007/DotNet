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
    public class DatabaseFactory : IDatabaseFactory, IDisposable //DisposableObject, 
    {
        private DbContext _dataContext;
        public DbContext Get()
        {
            if (_dataContext == null)
            {
                _dataContext = new DemoContext();
            }
            return _dataContext;
        }

        public void Dispose()
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }

        //protected override void Dispose()
        //{
        //    if (_dataContext != null)
        //        _dataContext.Dispose();
        //}
    }
}
