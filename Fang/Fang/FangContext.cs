using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Fang
{
    public class FangContext : DbContext
    {
        public DbSet<PageUrl> PageUrls { get; set; }
    }
}
