using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iakademi8_proje.Models;

namespace iakademi8_proje.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base

        Cetin_KirtasiyeEntities2 db = new Cetin_KirtasiyeEntities2();

        public BaseController()
        {
            //kategori listesi
            ViewBag.kategoriListesi = db.tbl_Categories.OrderBy(c => c.categoryname).ToList();
            //marka listesi
            ViewBag.markaListesi = db.tbl_Suppliers.OrderBy(s => s.brandname).ToList();
           
        }
       
    }
}