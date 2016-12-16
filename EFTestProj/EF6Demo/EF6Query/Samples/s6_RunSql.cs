using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6Query.Samples
{
    public class s6_RunSql
    {
        public static void Run()
        {
            using (EF6QueryContext ctx = new EF6QueryContext())
            {
                //执行SQL查询
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

                //执行存储过程
                var num = new SqlParameter("p0", "20");
                var output = new SqlParameter("p1", SqlDbType.NVarChar, 10)
                {
                    Direction = ParameterDirection.Output
                };
                var list = ctx.Database.SqlQuery<tmpCls>("dbo.procListUserWithRole @p0, @p1 out", num, output);
                foreach (var r in list)
                {
                    Console.WriteLine("{0}, {1}, {2}, {3}", r.RoleID, r.RoleName, r.UserID, r.UserName);
                }
                Console.WriteLine("output: {0}", output.Value);

            }
        }

        class tmpCls
        {
            public int UserID { get; set; }
            public int RoleID { get; set; }
            public string UserName { get; set; }
            public string RoleName { get; set; }
        }
    }
}
