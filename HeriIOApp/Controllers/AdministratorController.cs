using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HeriIOApp.Controllers
{
    public class AdministratorController : Controller
    {
        // GET: Administrator
        public ActionResult Index()
        {
            if (Request.IsAuthenticated && User.IsInRole("Administrator"))
                return View();
            else
            {
               return RedirectToAction("AdminLogin");
            }
              
        }

        public ActionResult Laporan()
        {
            return View();
        }


        public ActionResult AdminLogin()
        {
            return View();
        }
    }
}