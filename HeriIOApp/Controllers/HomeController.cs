using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace HeriIOApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            if (Request.IsAuthenticated && User.IsInRole("Administrator"))
            {
                return RedirectToAction("Index", "Administrator");
            }
            else if (Request.IsAuthenticated && User.IsInRole("Company"))
            {
                return RedirectToAction("Index", "Company");
            }
           else
                return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Home";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PendaftaranPelanggan()
        {
            ViewBag.Message = "Form Pendaftaran Pelanggan";
            return View();
        }

        [HttpPost]
        public ActionResult PendaftaranPelanggan(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var customer = new ModelData.pelanggan();
                customer.Alamat= collection.GetValue("Alamat").AttemptedValue;
                customer.Telepon= collection.GetValue("Telepon").AttemptedValue;
                customer.Tanggal= DateTime.Now;
                customer.Email = collection.GetValue("Email").AttemptedValue;
                customer.Nama= collection.GetValue("Nama").AttemptedValue;
                customer.UserId = User.Identity.GetUserId();

                using (var db = new OcphDbContext())
                {
                    var result = db.Customers.InsertAndGetLastID(customer);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult PendaftaranPengusaha()
        {
            ViewBag.Message = "Form Pendaftaran Pengusaha";
            return View();
        }

        [HttpPost]
        public ActionResult PendaftaranPengusaha(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var company = new ModelData.perusahaan();
                company.Alamat = collection.GetValue("Alamat").AttemptedValue;
                company.Telepon= collection.GetValue("Telepon").AttemptedValue;
                company.Tanggal= DateTime.Now;
                company.Email = collection.GetValue("Email").AttemptedValue;
                company.Nama = collection.GetValue("Nama").AttemptedValue;
                company.Pemilik = collection.GetValue("Pemilik").AttemptedValue;
                var id = User.Identity.GetUserId();
                company.UserId = User.Identity.GetUserId();
                using (var db = new OcphDbContext())
                {
                    var result = db.Companies.InsertAndGetLastID(company);
                  
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}