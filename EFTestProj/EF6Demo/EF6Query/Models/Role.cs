using System.Collections.Generic;

namespace EF6Query.Models
{
    public class Role
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public Role()
        {
            this.Users = new List<User>();
        }
    }
}