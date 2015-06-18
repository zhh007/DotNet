using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.Data.Models;
using Demo.DTO;
using Demo.ServiceInterface;
using Demo.Infrastructure.Data;
using Demo.Infrastructure.Validate;
using Demo.Data.Repositories;
using AutoMapper;

namespace Demo.EFAdapter
{
    public class UserInfoService : IUserInfoService
    {
		private IUnitOfWork fUnitofWork;
        private IUserInfoRepository fUserInfoRepository;

        public UserInfoService(IUnitOfWork _unitofWork, IUserInfoRepository _userInfoRepository)
        {
            fUnitofWork = _unitofWork;
            fUserInfoRepository = _userInfoRepository;
        }

		//[LogException]
        public IList<UserInfoDTO> GetPagedList(string text, int page, int pageSize, out int total)
        {
            var lst = fUserInfoRepository.GetAll();
            if (!string.IsNullOrEmpty(text))
            {
				text = text.Trim();
                lst = lst.Where(p => p.Name.Contains(text) || p.Title.Contains(text) || p.Remark.Contains(text));
            }

            lst = lst.OrderByDescending(p => p.ID);

            total = lst.Count();

            var lstresult = lst.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Mapper.Map<List<UserInfo>, List<UserInfoDTO>>(lstresult);
        }

		//[LogException]
        public void Add(UserInfoDTO dto)
        {
            var model = Mapper.Map<UserInfoDTO, UserInfo>(dto);
            fUserInfoRepository.Add(model);
            fUnitofWork.Commit();
        }

		//[LogException]
        public void Update(UserInfoDTO dto)
        {
			var model = fUserInfoRepository.Get(p => p.ID == dto.ID);
			model.Name = dto.Name;
			model.Title = dto.Title;
			model.Remark = dto.Remark;
            
            fUnitofWork.Commit();
        }

		//[LogException]
        public void Delete(int ID)
        {
            fUserInfoRepository.Delete(p => p.ID == ID);
            fUnitofWork.Commit();
        }

		//[LogException]
        public IEnumerable<ValidateResult> CanAdd(UserInfoDTO dto)
        {
            //int count = fUserInfoRepository.Count(p => p.Name == dto.Name);
            //if (count > 0)
            //{
            //    yield return new ValidateResult("Name", "已经存在相同数据。");
            //}
			return null;
        }

		//[LogException]
        public IEnumerable<ValidateResult> CanUpdate(UserInfoDTO dto)
        {
            //int count = fUserInfoRepository.Count(p => p.Name == dto.Name && p.ID != dto.ID);
            //if (count > 0)
            //{
            //    yield return new ValidateResult("Name", "已经存在相同数据。");
            //}
			return null;
        }

		//[LogException]
        public IEnumerable<ValidateResult> CanDelete(int ID)
        {
            //int count = fModuleRepsitory.Count(p => p.TypeID == ID);
            //if (count > 0)
            //{
            //    yield return new ValidateResult("", "该已经在使用，不能删除。");
            //}
            return null;
        }

		//[LogException]
        public UserInfoDTO GetByID(int ID)
        {
            var model = fUserInfoRepository.Get(p => p.ID == ID);
            return Mapper.Map<UserInfo, UserInfoDTO>(model);
        }

		//[LogException]
        public IList<UserInfoDTO> GetAll()
        {
            var lst = fUserInfoRepository.GetAll().ToList();
            return Mapper.Map<List<UserInfo>, List<UserInfoDTO>>(lst);
        }

    }
}
