using EF6Query.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EF6Query.Samples
{
    public class s5_QueryView
    {
        public static void Run()
        {
            using (EF6QueryContext ctx = new EF6QueryContext())
            {
                //查询视图第10~20的记录
                var list = (from p in ctx.UserWithRoles
                            orderby p.UserID ascending
                            select p).Skip(10).Take(10).ToList();
                foreach (var r in list)
                {
                    Console.WriteLine("{0}, {1}, {2}, {3}", r.RoleID, r.RoleName, r.UserID, r.UserName);
                }

                ////执行SQL查询
                //var list2 = ctx.UserWithRoles.SqlQuery("SELECT TOP 10 * FROM vUserWithRole");
                //foreach (var r in list2)
                //{
                //    Console.WriteLine("{0}, {1}, {2}, {3}", r.RoleID, r.RoleName, r.UserID, r.UserName);
                //}

                ////未有定义表的临时SQL语句查询
                //var list3 = ctx.Database.SqlQuery<tmpCls>("SELECT TOP 10 * FROM vUserWithRole");
                //foreach (var r in list3)
                //{
                //    Console.WriteLine("{0}, {1}, {2}, {3}", r.RoleID, r.RoleName, r.UserID, r.UserName);
                //}
            }
        }
    }
}
