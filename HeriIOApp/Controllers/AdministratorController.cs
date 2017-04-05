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
                return Content("<Script language='javascript' type='text/javascript'> alert('Anda Tidak memiliki Akses'); </script>");
            }
              
        }
    }
}