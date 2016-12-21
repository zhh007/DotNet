using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Demo.Controllers
{
    public class SrvPerfsController : Controller
    {
        // GET: SrvPerfs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowPerf()
        {
            return View();
        }
    }
}