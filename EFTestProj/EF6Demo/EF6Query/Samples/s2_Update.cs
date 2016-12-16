using EF6Query.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6Query.Samples
{
    public class s2_Update
    {
        public static void Run()
        {
            using (EF6QueryContext ctx = new EF6QueryContext())
            {
                var user1 = ctx.Set<User>().FirstOrDefault();

                user1.UserName = "first user";

                var role1 = (from r in user1.Roles
                             where r.RoleName == "role1"
                             select r).FirstOrDefault();
                if (role1 != null)
                {
                    user1.Roles.Remove(role1);
                }

                if (user1.Roles.Count(p => p.RoleName == "role3") == 0)
                {
                    var role3 = new Role()
                    {
                        RoleName = "role3"
                    };
                    user1.Roles.Add(role3);
                }

                ctx.SaveChanges();

                var u = ctx.Set<User>().FirstOrDefault(p => p.UserID == user1.UserID);
                if (u != null)
                {
                    Console.WriteLine("{0}, {1}", u.UserID, u.UserName);
                    foreach (var r in u.Roles)
                    {
                        Console.WriteLine("{0}, {1}", r.RoleID, r.RoleName);
                    }
                }
            }
        }
    }
}
