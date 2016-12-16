using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6Query.Samples
{
    public class s4_LoadRef
    {
        public static void Run()
        {
            using (EF6QueryContext ctx = new EF6QueryContext())
            {
                ////延迟加载（Lazy Loading）
                //var user1 = ctx.Users.FirstOrDefault(p => p.UserID == 40);
                //foreach (var r in user1.Roles)
                //{
                //    Console.WriteLine("{0}, {1}", r.RoleID, r.RoleName);
                //}

                //贪婪加载（Eager Loading）
                //var user2 = ctx.Users.Include(p => p.Roles)
                //                .FirstOrDefault(p => p.UserID == 60);
                //foreach (var r in user2.Roles)
                //{
                //    Console.WriteLine("{0}, {1}", r.RoleID, r.RoleName);
                //}

                ////显示加载（Explicit Loading）
                ////显示加载集合对象
                //var user1 = ctx.Users.FirstOrDefault(p => p.UserID == 40);
                //Console.WriteLine("Before load：{0}", ctx.Entry(user1).Collection(p => p.Roles).IsLoaded);
                //ctx.Entry(user1)
                //    .Collection(p => p.Roles)
                //    .Query()
                //    //.Where(r => r.RoleName == "role name") //这里可以设置条件
                //    .Load();
                //foreach (var r in user1.Roles)
                //{
                //    Console.WriteLine("{0}, {1}", r.RoleID, r.RoleName);
                //}
                //Console.WriteLine("After load：{0}", ctx.Entry(user1).Collection(p => p.Roles).IsLoaded);

                ////显示加载非集合对象
                ////ctx.Entry(user1)
                ////    .Reference(p => p.Type);

            }
        }
    }
}
