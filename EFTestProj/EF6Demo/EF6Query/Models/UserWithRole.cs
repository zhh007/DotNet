using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6Query.Models
{
    public class UserWithRole
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}
