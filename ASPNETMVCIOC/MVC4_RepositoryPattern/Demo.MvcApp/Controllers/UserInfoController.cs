using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Demo.DTO;
using Demo.ServiceInterface;
using Demo.MvcApp.Models;
using Demo.Infrastructure.Validate;
using Demo.Infrastructure.Mvc;

namespace Demo.MvcApp.Controllers
{
    public class UserInfoController : Controller
    {
        private IUserInfoService UserInfoService;

        public UserInfoController(IUserInfoService _userInfoService)
        {
            UserInfoService = _userInfoService;
        }

        public ActionResult Index(int? id, string searchText)
        {
            UserInfoListViewModel model = new UserInfoListViewModel();
            model.Text = searchText;
			
			int totalRecords = 0;
            int pagesize = 20;
            var list = UserInfoService.GetPagedList(searchText, id ?? 1, pagesize, out totalRecords);
            model.List = list;

            //IPagedList<UserInfoDTO> pagedList = new StaticPagedList<UserInfoDTO>(list.ToArray<UserInfoDTO>(), id ?? 1, pagesize, totalRecords);
            //model.List = pagedList;
            return View(model);
        }

        public ActionResult Add()
        {
            UserInfoViewModel model = new UserInfoViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(UserInfoViewModel model)
        {
			try
			{
				if (ModelState.IsValid)
				{
					UserInfoDTO dto = Mapper.Map<UserInfoViewModel, UserInfoDTO>(model);
					//dto.ID = Guid.NewGuid();
					//dto.CreateTime = DateTime.Now;
     //               dto.CreateUserID = CurrentUser.ID;
     //               dto.ModifyTime = DateTime.Now;
     //               dto.ModifyUserID = CurrentUser.ID;
					IEnumerable<ValidateResult> resultList = UserInfoService.CanAdd(dto);
					ModelState.AddModelErrors(resultList);
					if (ModelState.IsValid)
					{
						UserInfoService.Add(dto);
						return Json(new { success = true, message = "保存成功。" }, "text/html");
					}
				}
				return Json(new { success = false, message = "" }, "text/html");
			}
            catch (Exception ex)
            {
                //LogHelper.Error("保存失败", ex);
                return Json(new { result = false, message = "发生错误，请联系管理员。" });
            }
        }

   //     public ActionResult Edit(int id)
   //     {
   //         var dto = UserInfoService.GetByID(id);
   //         UserInfoViewModel model = Mapper.Map<UserInfoDTO, UserInfoViewModel>(dto);
   //         return View(model);
   //     }

   //     [HttpPost]
   //     [ValidateAntiForgeryToken]
   //     public ActionResult Edit(UserInfoViewModel model)
   //     {
			//try
   //         {
			//	if (ModelState.IsValid)
			//	{
			//		UserInfoDTO dto = Mapper.Map<UserInfoViewModel, UserInfoDTO>(model);
			//		dto.ModifyTime = DateTime.Now;
   //                 dto.ModifyUserID = CurrentUser.ID;
			//		IEnumerable<ValidateResult> resultList = UserInfoService.CanUpdate(dto);
			//		ModelState.AddModelErrors(resultList);
			//		if (ModelState.IsValid)
			//		{
			//			UserInfoService.Update(dto);
			//			return Json(new { success = true, message = "保存成功。" }, "text/html");
			//		}
			//	}
			//	return Json(new { success = false, message = this.GetErrors() }, "text/html");
			//}
   //         catch (Exception ex)
   //         {
   //             LogHelper.Error("保存失败", ex);
   //             return Json(new { result = false, message = "发生错误，请联系管理员。" });
   //         }
   //     }

   //     [HttpPost]
   //     public ActionResult Delete(int id)
   //     {
   //         try
   //         {
			//	IEnumerable<ValidateResult> resultList = UserInfoService.CanDelete(id);
   //             ModelState.AddModelErrors(resultList);                
   //             if (ModelState.IsValid)
   //             {
   //                 UserInfoService.Delete(id);
			//		return Json(new { result = true, message = "" });
   //             }
   //             return Json(new { result = false, message = this.GetErrors() });
   //         }
   //         catch (Exception ex)
   //         {
   //             LogHelper.Error("删除失败", ex);
   //             return Json(new { result = false, message = "发生错误，请联系管理员。" });
   //         }
   //     }
    }
}
