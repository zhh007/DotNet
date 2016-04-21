using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DTO;
using Demo.Infrastructure.Validate;

namespace Demo.ServiceInterface
{
    public interface IUserInfoService : IDisposable
    {
        IList<UserInfoDTO> GetAll();
        UserInfoDTO GetByID(int ID);
        IList<UserInfoDTO> GetPagedList(string text, int page, int pageSize, out int total);

        void Add(UserInfoDTO dto);
        void Update(UserInfoDTO dto);
        void Delete(int ID);

        IEnumerable<ValidateResult> CanAdd(UserInfoDTO dto);
        IEnumerable<ValidateResult> CanUpdate(UserInfoDTO dto);
        IEnumerable<ValidateResult> CanDelete(int ID);

    }
}



