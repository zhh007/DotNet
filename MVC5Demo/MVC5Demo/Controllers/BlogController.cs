using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Demo.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _List()
        {
            return PartialView();
        }

        public ActionResult Show(int year, int month)
        {
            ViewBag.Year = year;
            ViewBag.Month = month;

            return View();
        }
    }
}