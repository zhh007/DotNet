using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demo.DTO;

namespace Demo.MvcApp.Models
{
    public class UserInfoViewModel
    {
		[DisplayName("ID")]
		public int ID { get; set; }

		[DisplayName("姓名")]
        [Required(ErrorMessage = "请输入{0}！")]
        public string Name { get; set; }

		[DisplayName("Title")]
		public string Title { get; set; }

		[DisplayName("Remark")]
		public string Remark { get; set; }

		
		//public IEnumerable<SelectListItem> CourseList { get; set; }
    }

    public class UserInfoListViewModel
    {
        public string Text { get; set; }
        //public IPagedList<UserInfoDTO> List { get; set; }
        public IList<UserInfoDTO> List { get; set; }
    }
}
