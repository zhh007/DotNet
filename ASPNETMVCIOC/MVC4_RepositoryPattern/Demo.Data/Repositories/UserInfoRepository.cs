using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Data.Models;
using Demo.Infrastructure.Data;
using EntityFramework.Extensions;

namespace Demo.Data.Repositories
{
    public class UserInfoRepository : RepositoryBase<UserInfo>, IUserInfoRepository
    {
        public UserInfoRepository(DemoContext _dbContext)
            : base(_dbContext)
        {

        }


        public void UpdatePartial(UserInfo model)
        {
            //DataContext.Set<UserInfo>().Update<UserInfo>(
            //    p => p.ID == model.ID,
            //    m => new UserInfo
            //{
            //    Name = model.Name,
            //    Title = model.Title,
            //    Remark = model.Remark

            //}
            //);
        }

        //public User GetIncludeAllEntiy(Expression<Func<User, bool>> where)
        //{
        //    var entity = DataContext.Set<User>().Include("Roles").Where(where).FirstOrDefault();
        //    return entity;

        //}

    }

    public interface IUserInfoRepository : IRepository<UserInfo>
    {
        void UpdatePartial(UserInfo model);

    }
}
