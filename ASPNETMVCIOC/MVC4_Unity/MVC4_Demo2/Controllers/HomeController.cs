using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demo.Interface;

namespace MVC4_Demo2.Controllers
{
    public class HomeController : Controller
    {
        protected IServiceA _serviceA;
        protected IServiceB _serviceB;

        public HomeController(IServiceA serviceA, IServiceB serviceB)
        {
            _serviceA = serviceA;
            _serviceB = serviceB;
        }

        public ActionResult Index()
        {
            ViewBag.A = _serviceA.Say();
            ViewBag.B = _serviceB.Write();
            return View();
        }

    }
}
