using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iakademi8_proje.Models;

namespace iakademi8_proje.Controllers
{
    public class AdminController : Controller
    {
        Cetin_KirtasiyeEntities2 db = new Cetin_KirtasiyeEntities2();
        Cls_Urunler cu = new Cls_Urunler();

        // GET: Admin
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(tbl_Users usr)
        {
            string md5parola = Cls_Users.MD5sifrele(usr.parola);

            tbl_Users uy = db.tbl_Users.FirstOrDefault(u => u.email == usr.email && u.parola == usr.parola);

            if (uy != null)
            {
                //null değilse kullanıcı var
                //başarılı
                Session["email"] = uy.email;
                Session["userID"] = uy.userID;
                Session["isimsoyisim"] = uy.isimsoyisim;
                return RedirectToAction("Anasayfa");
            }
            else
            {
                //null ise kullanıcı yok
                return View(); //login sayfasında kalır.tekrar giriş bekler
            }
        }


        public ActionResult Anasayfa()
        {
            if (Session["email"] == null)
            {
                //login girişi yapmamışsa ve url link yazarak enter la gelmişse
                return RedirectToAction("login");
            }
            else
            {
                //login girişi yapmış başarılı
                return View();
            }
        }
        public ActionResult ProductAdd()
        {
            if (Session["email"] == null)
            {
                //login girişi yapmamışsa ve url link yazarak enter la gelmişse
                return RedirectToAction("login");
            }
            else
            {
                //login girişi yapmış başarılı
                List<tbl_Categories> catlist = db.tbl_Categories.ToList();
                ViewData["catlist"] = catlist.Select(a => new SelectListItem { Text = a.categoryname, Value = a.categoryID.ToString() }).ToList();

                List<tbl_Suppliers> marlist = db.tbl_Suppliers.ToList();
                ViewData["marlist"] = marlist.Select(a => new SelectListItem { Text = a.brandname, Value = a.supplierID.ToString() }).ToList();

                return View();
            }
        }

        [HttpPost]
        public ActionResult ProductAdd(HttpPostedFileBase fileuploader,  tbl_Products prd)
        {
            prd.adddate = DateTime.Now;
            prd.onecikanlar = 0;
            prd.coksatanlar = 0;
            prd.bunabakanlar_bunadabakti = "";
            prd.aktif = true;

            if (prd.discount == null)
            {
                prd.discount = 0;
            }

            if (prd.urunkodu == null)
            {
                prd.urunkodu = "";
            }

            if (prd.status == null)
            {
                prd.status = 0;
            }

            if (prd.notes == null)
            {
                prd.notes = "";
            }

            if (fileuploader != null)
            {
              string path =  Path.Combine(Server.MapPath("~/Content/urunresimleri"), Path.GetFileName(fileuploader.FileName));
                fileuploader.SaveAs(path);
                prd.photopath = fileuploader.FileName;
            }
            db.tbl_Products.Add(prd);
            db.SaveChanges();

            return RedirectToAction("ProductAdd");
        }

        public ActionResult logout()
        {
            Session.Abandon();
            Session.Remove("email");
            Session.Remove("userID");
            Session.Remove("isimsoyisim");
            Session.RemoveAll();
            return RedirectToAction("login");
        }


        public ActionResult CategoryAdd()
        {
            List<tbl_Categories> catlist = db.tbl_Categories.Where(c => c.parentID == 0).ToList();
            ViewData["catlist"] = catlist.Select(a => new SelectListItem { Text = a.categoryname, Value = a.categoryID.ToString() }).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult CategoryAdd(tbl_Categories cat)
        {
            //dropdownlist ten seçim yapılmamışsa, categoryID degeri 0 geliyor.Demekki Ana kategori girişi yapılıyor
            //parentID degeri 0 sıfır olmak zorunda

            //dropdownlist ten seçim yapmışsa ,  categoryID degeri dolu geliyor.Demekki Alt kategori girişi yapılıyor
            //parentID degeri , dropdownlist ten seçilen Kategorinin , categoryID degeri

            if (cat.categoryID == 0)
            {
                //ana kategori
                cat.parentID = 0;
            }
            else
            {
                cat.parentID = cat.categoryID;
            }

            cat.aktif = true;
            db.tbl_Categories.Add(cat);
            db.SaveChanges();


            return RedirectToAction("CategoryAdd"); //HttpGet ten gider.orada kategori listesi oldugu için, CategoryAdd.cshtml patlamaz.çünkü CategoryAdd.cshtml yüklenirken ,dropdownlist için, kategori listesi bekliyor

            //eger return view(); ozaman httppost tan , CategoryAdd.cshtml
        }

        public ActionResult SupplierAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SupplierAdd(HttpPostedFileBase fileuploader,tbl_Suppliers sup)
        {
            sup.aktif = true;

            if (fileuploader != null)
            {
                string path = Path.Combine(Server.MapPath("~/Content/urunresimleri"), Path.GetFileName(fileuploader.FileName));
                fileuploader.SaveAs(path);
                sup.photopath = fileuploader.FileName;
            }

            db.tbl_Suppliers.Add(sup);
            db.SaveChanges();
            return View();
        }

        //public ActionResult MesajListele()
        //{
        //    List<tbl_Messages> mesajlar = Cls_Mesaj.mesajlariGetir();
        //    return View(mesajlar);
        //}

        public ActionResult ProductList()
        {
           List<WV_aktifurunler> urn = cu.urunlerigetir();
            return View(urn);
        }

        public ActionResult UrunGuncelle(int id)
        {
            if (Session["email"] == null)
            {
                //login girişi yapmamışsa ve url link yazarak enter la gelmişse
                return RedirectToAction("login");
            }
            else
            {
                //login girişi yapmış başarılı
                List<tbl_Categories> catlist = db.tbl_Categories.ToList();
                ViewData["catlist"] = catlist.Select(a => new SelectListItem { Text = a.categoryname, Value = a.categoryID.ToString() }).ToList();

                List<tbl_Suppliers> marlist = db.tbl_Suppliers.ToList();
                ViewData["marlist"] = marlist.Select(a => new SelectListItem { Text = a.brandname, Value = a.supplierID.ToString() }).ToList();

                WV_aktifurunler urn = db.WV_aktifurunler.FirstOrDefault(p => p.productID == id);

                return View(urn);
            }
        }

        [HttpPost]
        public ActionResult UrunGuncelle(tbl_Products prd,HttpPostedFileBase fileuploader)
        {
            WV_aktifurunler prdt = db.WV_aktifurunler.FirstOrDefault(p => p.productID == prd.productID);
            if (fileuploader != null)
            {
                //yeni resim eklemiş,ürünün resmini değiştirecek
                string path = Path.Combine(Server.MapPath("/Content/urunresimleri"), Path.GetFileName(fileuploader.FileName));
                fileuploader.SaveAs(path); //ürün resmini /Content/urunresimleri klasörünekoplayadım

                prd.photopath = fileuploader.FileName; //ürün resminin ismini tabloda photopath alanına yazdım
            }
            else
            {
                //eski resim duracak
                prd.photopath = prdt.photopath;
            }

            prd.adddate = prdt.adddate;
            prd.aktif = prdt.aktif;
            prd.coksatanlar = prdt.coksatanlar;
            prd.onecikanlar = prdt.onecikanlar;



            db.Entry(prd).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ProductList");
        }


        public ActionResult UrunSil(int id)
        {
            tbl_Products prd = db.tbl_Products.FirstOrDefault(p => p.productID == id);
            db.tbl_Products.Remove(prd);
            db.SaveChanges();
            return RedirectToAction("ProductList");
        }

    }
}