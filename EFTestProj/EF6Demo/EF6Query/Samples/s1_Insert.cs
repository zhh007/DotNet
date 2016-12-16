using EF6Query.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6Query.Samples
{
    public class s1_Insert
    {
        public static void Run()
        {
            using (EF6QueryContext ctx = new EF6QueryContext())
            {
                User user1 = new User()
                {
                    UserName = "user1",
                    IsValid = true,
                    Password = "abc"
                };

                Role role1 = new Role()
                {
                    RoleName = "role1"
                };

                Role role2 = new Role()
                {
                    RoleName = "role2"
                };

                user1.Roles.Add(role1);
                user1.Roles.Add(role2);

                ctx.Set<User>().Add(user1);
                ctx.SaveChanges();

                var u = ctx.Set<User>().FirstOrDefault(p => p.UserName == "user1");
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
