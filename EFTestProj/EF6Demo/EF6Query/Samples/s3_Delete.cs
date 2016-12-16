using EF6Query.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6Query.Samples
{
    public class s3_Delete
    {
        public static void Run()
        {
            using (EF6QueryContext ctx = new EF6QueryContext())
            {
                var user1 = ctx.Set<User>().FirstOrDefault();

                ctx.Set<User>().Remove(user1);

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
                else
                {
                    Console.WriteLine("用户已删除。");
                }
            }
        }
    }
}
