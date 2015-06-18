using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Data
{
    public interface IDatabaseFactory : IDisposable
    {
        DbContext Get();
    }
}
