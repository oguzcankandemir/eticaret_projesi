using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iakademi8_proje.Models;
using PagedList;
using PagedList.Mvc;

namespace iakademi8_proje.Controllers
{
    public class HomeController : BaseController
    {
        AnaSayfaModel ans = new AnaSayfaModel();
        Cls_Urunler cu = new Cls_Urunler();
        Cls_Siparisler cs = new Cls_Siparisler();
        Cls_Users cus = new Cls_Users();
        Cetin_KirtasiyeEntities2 db = new Cetin_KirtasiyeEntities2();
        // string adet = ConfigurationManager.AppSettings["adet"].ToString(); -> web.config içinden okuduk
        int adet = 0;  //this.adet
        // GET: Home
        public ActionResult Index()
        {
            ans.sliderUrunler = cu.sliderUrunlerGetir();
            ans.gununUrunu = cu.gunun_urununu_getir();
            ans.enyeniUrunler = cu.enyeniurunler_getir("anasayfa",adet);
            ans.ozelUrunler = cu.ozelurunler_getir("anasayfa");
            ans.indirimliUrunler = cu.indirimliurunler_getir("anasayfa");
            ans.onecikanUrunler = cu.onecikanurunler_getir("anasayfa");
            ans.coksatanUrunler = cu.coksatanurunler_getir("anasayfa");
            ans.firsatUrunler = cu.firsaturunler_getir();
            ans.yildizliUrunler = cu.yildizliurunler_getir();
            ans.superfiyatteklifUrunler = cu.superfiyatteklifUrunler_getir();
            ans.dikkatcekenUrunler = cu.dikkatcekenUrunler_getir();
            return View(ans);
        }

        public ActionResult EnYeniUrunler()
        {
            ans.enyeniUrunler = cu.enyeniurunler_getir("EnYeniUrunler",0);
            return View(ans);
        }

        public ActionResult _partial_EnYeniUrunler(string sonrakisayfa)
        {
            ans.enyeniUrunler = cu.enyeniurunler_getir(sonrakisayfa,0);
            return View(ans);
        }

        public ActionResult Ozeller()
        {
            ans.ozelUrunler = cu.ozelurunler_getir("Ozeller");
            return View(ans);
        }

        public ActionResult _partial_Ozeller(string sonrakisayfa)
        {
            ans.ozelUrunler = cu.ozelurunler_getir(sonrakisayfa);
            return View(ans);
        }

        public ActionResult indirimliUrunler()
        {
            ans.indirimliUrunler = cu.indirimliurunler_getir("indirimliUrunler");
            return View(ans);
        }

        public ActionResult _partial_indirimliUrunler(string sonrakisayfa)
        {
            ans.indirimliUrunler = cu.indirimliurunler_getir(sonrakisayfa);
            return View(ans);
        }

        public ActionResult Sepet(int id)
        {
            //10=1&20=1&30=1

            HttpCookie cSetCookie;
           // string sepet = "";
            //cookie yi browser da sepete ekleyelim
            Cls_Siparisler s = new Cls_Siparisler();
            s.productID = id;
            s.adet = 1;

            //sepetim -> cookie ismi
            cSetCookie = Request.Cookies.Get("sepetim"); //google chrome da(bütün tarayıcılar için gecerli) , sepetim isminde cookie (çerez) varmı diye okuyoruz

            if (cSetCookie == null)
            {
                //sepetim isminde , daha önceden yaratılmış , cookie yok
                cSetCookie = new HttpCookie("sepetim");
                s.sepet = "";
            }
            else
            {
                //sepetim isminde , daha önceden yaratılmış , cookie var, ürünü sepete ekle demişim
                s.sepet = cSetCookie.Value;
               // 10=1&20=1&30=1 
            }

            //aynı ürün varsa sepete eklemeyeceğiz
            if (s.sepettevarmi(id.ToString()) == true)
            {
                //ürün ekler
                cSetCookie.Values.Add(id.ToString(), "1");
                cSetCookie.Expires = DateTime.Now.AddMinutes(60 * 24 * 7); //1 haftalık yarattık
                Response.Cookies.Add(cSetCookie);
                Session["Mesaj"] = "Ürün Sepetinize Eklendi";
            }
            else
            {
                Session["Mesaj"] = "Bu ürün sepetinizde zaten var.";
            }
            
            cu.one_cikanlar_kolonunu_arttir(id);
            return RedirectToAction("Index");
        }

        public ActionResult detaylar(int id)
        {
            ans.detayliUrun = cu.detayli_urun_getir(id);
            //ans.vw_benzer_urunler = cu.benzer_urunler_getir(ans.detayliUrun.categoryID);
            ans.vw_bunabakanlar = cu.bunabakanlar_getir(ans.detayliUrun.bunabakanlar_bunadabakti);
            cu.one_cikanlar_kolonunu_arttir(id);
            return View(ans);
        }

        public ActionResult kategoriler(int id,int? page)
        {
            //select * from tbl_Products where categoryID = id

            var list = db.WV_aktifurunler.Where(p => p.categoryID == id).OrderBy(p => p.productname).ToList();

            var pagenumber = page ?? 1; //page = null ise , page = 1
            var sayfaliveriler = list.ToPagedList(pagenumber, 4); //4 -> o sayfada gösterilecek ürün adedi

            ViewBag.urunler = sayfaliveriler;
            return View();
        }
        public ActionResult markalar(int id,int? page)
        {

            var list = db.WV_aktifurunler.Where(p => p.supplierID == id).OrderBy(p => p.productname).ToList();

            var pagenumber = page ?? 1;
            var sayfaliveriler = list.ToPagedList(pagenumber, 4);

            ViewBag.urunler = sayfaliveriler;
            return View();
        }



        public ActionResult Onecikanlar(int? page)
        {
            var pagenumber = page ?? 1; //page == null ozaman page = 1

            var list = cu.onecikanurunler_getir("Onecikanlar");

            var urunler = list.ToPagedList(pagenumber, 4);
            ViewBag.Onecikanlar = urunler;

            return View();
        }

        public ActionResult coksatanlar(int? page)
        {
            var pagenumber = page ?? 1; //page == null ozaman page = 1

            var list = cu.coksatanurunler_getir("coksatanlar");

            var urunler = list.ToPagedList(pagenumber, 4);
            ViewBag.coksatanlar = urunler;

            return View();
        }
        public ActionResult uyeol()
        {
            return View();
        }

        [HttpPost]
        public ActionResult uyeol(tbl_Users usr)
        {
            //metodu cağıracagız
            Cls_Users.uyeekle(usr);
            return RedirectToAction("hesabim");
        }

        public ActionResult hesabim()
        {
            return View();
        }

        [HttpPost]
        public ActionResult hesabim(tbl_Users usr)
        {
            string geciciparola = Cls_Users.MD5sifrele(usr.parola);

            tbl_Users us = db.tbl_Users.FirstOrDefault(u => u.parola == geciciparola && u.email == usr.email);

            if (us != null)
            {
                //dogru login ve şifre
                Session["userID"] = us.userID;
                Session["email"] = us.email;

                if (us.adminmi == true)
                {
                    Session["adminmi"] = us.adminmi;
                }

                return RedirectToAction("Index");
            }
            else
            {
                //yanlış login yada şifre
                 Session["Mesaj"] = "Login yada Şifreniz yanlıştır.";
                //  ViewBag.Mesaj = "Login yada Şifreniz yanlıştır.";
                return View();
            }
        }


        public ActionResult  logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }


        public ActionResult Cart()
        {
            if (Request.QueryString["scid"] != null)
            {
                //ürünü sepetten sil butonu ile geldim.
                int scid = Convert.ToInt32(Request.QueryString["scid"]);

                //tarayıcıdaki sepetim bilgilerini aldım
                HttpCookie csetCookie = Request.Cookies.Get("sepetim");
                string sepetcookie = csetCookie.Value;

                cs.sepet = sepetcookie;
                cs.sepetten_sil(scid);

                HttpCookie cKuki = new HttpCookie("sepetim", cs.sepet); //sepetim ismindeki sepetime , cs.sepet içindeki silinmiş olan halinin tanımını yapıyorum
                Response.Cookies.Add(cKuki); //son halini tarayıcıya(browser , google chrome,internet explorer) gönderiyorum
                cKuki.Expires = DateTime.Now.AddMinutes(60 * 24 * 7);
                Session["Mesaj"] = "Ürün Sepetinizden Silindi";
                List<Cls_Siparisler> sepet = cs.sepetiyazdir();
                ViewBag.Sepetim = sepet;
                ViewBag.sepet_tablo_detay = sepet;
            }
            else
            {
                //projede sağ üstteki sepet sayfasına git butonuyla geldim
                HttpCookie csetCookie = Request.Cookies.Get("sepetim");
                List<Cls_Siparisler> sepet;
                //cookie sepeti boşsa
                if (csetCookie == null)
                {
                    csetCookie = new HttpCookie("sepetim");
                    cs.sepet = "";
                    sepet = cs.sepetiyazdir();
                    ViewBag.Sepetim = sepet;
                    ViewBag.sepet_tablo_detay = sepet;
                }
                else
                {
                    //doluysa
                    cs.sepet = csetCookie.Value.ToString();
                    sepet = cs.sepetiyazdir(); // prop + tab + tab döndürüyor,çünkü metodun dönüş tipinde ,List<Cls_Siparisler>,yazdım
                    ViewBag.Sepetim = sepet;
                    ViewBag.sepet_tablo_detay = sepet;
                }
            }
            return View();
        }


        [HttpGet]
        public ActionResult order()
        {
            //login girişin varmı?
            //var ise -> return View();
            //yoksa -> hesabim

            if (Session["userID"] != null)
            {
                //giriş yapılmış
               List<Cls_Users> uye_bilgileri =  cus.uyegetir(Convert.ToInt32(Session["userID"]));
                ViewBag.UyeBilgi = uye_bilgileri;
                return View();
            }
            else
            {
                //giriş yapılmamış
                return RedirectToAction("hesabim");
            }
        }

        string returnvalue = "";

        [HttpPost]
        public ActionResult order(FormCollection frm)
        {
            string creditCardNumber = Request.Form["creditCardNumber"];
            string creditCardMonth = Request.Form["creditCardMonth"];
            string creditCardYear = Request.Form["creditCardYear"];
            string creditCardCVC = Request.Form["creditCardCVC"];


            if (Request.Form["txttckimlikno"] != null)
            {
                string txttckimlikno = Request.Form["txttckimlikno"];
            }
            if (Request.Form["txtvergino"] != null)
            {
                string txtvergino = Request.Form["txtvergino"];
            }

            /* burası şuan banka kontrolü yapamayacagı icin yorum satırı yaptım 
              return RedirectToAction("backref", "Home");
             */
            return RedirectToAction("backref", "Home");

            /*  NameValueCollection data = new NameValueCollection();
              string backref = "site ismi/Home/backref";

              data.Add("BACK_REF", backref);
              data.Add("CC_CVV", creditCardCVC);
              data.Add("CC_NUMBER", creditCardNumber);
              data.Add("EXP_MONTH", creditCardMonth);
              data.Add("EXP_YEAR", creditCardYear);

              var deger = "";
              foreach (var item in data)
              {
                  var value = item as string;
                  var byteCount = Encoding.UTF8.GetByteCount(data.Get(value));
                  deger += byteCount + data.Get(value);
              }

              var signatureKey = "Payu ya üye olunca, Payunun verdigi SECRET_KEY buraya yazılacak";
              var hash = HashWithSignature(deger, signatureKey);

              data.Add("ORDER_HASH", hash);

              var x = POSTFormPayu("https://secure.payu.com.tr/order/alu/v3", data);

              //sanal kart
              if (x.Contains("<STATUS>SUCCESS</STATUS>") && x.Contains("<RETURN_CODE>3DS_ENROLLED</RETURN_CODE>"))
              {
                  //sanal kart ile yapılan alışverişe banka onay vermiş demektir
                  returnvalue = "basarili";
              }
              else
              {
                  //gerçek kart
                  if (x.Contains("INVALID_PAYMENT_INFO"))
                  {
                      returnvalue = "Geçersiz kart bilgileri.Kart bilgilerinizi kontrol ediniz.";
                  }
                  else
                  {
                      returnvalue = "Genel hata";
                  }
              }

              return RedirectToAction("Cart", "Home");
            */
        }

        [HttpGet]
        public ActionResult backref()
        {
            string data = new System.IO.StreamReader(Request.InputStream).ReadToEnd();
            ViewBag.returnvalue = "basarili";

            siparisi_kaydet();

            return RedirectToAction("Onay");
        }


        public static string orderGroupGUID = "";
        public ActionResult siparisi_kaydet()
        {
            HttpCookie cSetCookie;
            cSetCookie = Request.Cookies.Get("sepetim");

            if (cSetCookie != null)
            {
                cs.sepet = cSetCookie.Value.ToString(); //tarayıcıdan sepetteki bilgileri aldım,Cls_Siparis lerdeki, sepet propertysine gönderdim

                orderGroupGUID = cs.cookie_sepetini_siparis_tablosuna_yaz(Convert.ToInt32(Session["userID"]));
                cs.sepet = "";
                HttpCookie cKuki = new HttpCookie("sepetim", cs.sepet);
                Response.Cookies.Add(cKuki);

                cKuki.Expires = DateTime.Now.AddMinutes(60 * 24 * 5);
            }
            return View();
        }


        public ActionResult Onay()
        {
            ViewBag.orderGroupGUID = orderGroupGUID;
            return View();
        }



        public static string HashWithSignature(string hashString,string signature)
        {
            var binaryHash = new HMACMD5(Encoding.UTF8.GetBytes(signature)).ComputeHash(Encoding.UTF8.GetBytes(hashString));

            var hash = BitConverter.ToString(binaryHash).Replace("-", string.Empty).ToLowerInvariant();
            return hash;
        }

        private static string POSTFormPayu(string url, NameValueCollection data)
        {
            var result = new List<StringString>();

            var webClient = new WebClient();
            try
            {
                string request = Encoding.UTF8.GetString(webClient.UploadValues(url, data)).Trim();
                return request;
            }
            catch (Exception)
            {
            }
            return "";
        }

        public class StringString
        {
            public string Text1 { get; set; }
            public string Text2 { get; set; }
        }



        public ActionResult siparislerim()
        {
            if (Session["userID"] != null)
            {
                int userID = Convert.ToInt32(Session["userID"]);
                //siparisleri getir
                List<WV_siparislerim> siplist = db.WV_siparislerim.Where(s => s.userID == userID).ToList();
                return View(siplist);
            }
            else
            {
                //giriş yapılmamış
                return RedirectToAction("hesabim");
            }
        }


        public ActionResult urunlerstring(string id)
        {
            id = id.ToUpper(new System.Globalization.CultureInfo("tr-TR", false));

            List<vw_arama> list = Cls_Urunler.arama_getir(id);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult detayliarama()
        {
            return View();
        }

        ArrayList mr = new ArrayList();

        [HttpPost]
        public ActionResult dproducts(string kategoriler,string[] markalar,string stoktavarmi)
        {
            // string kategoriler = Request.Form["kategoriler];
            //string markalar = Request.Form["markalar"];


            //Hobi Eğlence
            //GRUNDIG,SONY,ASUS

            string amount = Request.Form["amount"];//  $150-$300
            amount = amount.ToString().Replace("$", "").Replace(" ", "");

            string[] fiyatdizi = amount.Split('-');

            string baslangicpara = fiyatdizi[0];
            string bitispara = fiyatdizi[1];

            int categoryID = Convert.ToInt32(kategoriler); 

            //markalar 
            if (markalar != null && markalar.Length >0)
            {
                foreach (var item in markalar)
                {
                    mr.Add(item);
                }
            }

            string markaID = "";
            for (int i = 0; i < mr.Count; i++)
            {
                if (i != mr.Count - 1)
                {
                    //or ..birden fazla kayıt
                    markaID += "supplierID = " + mr[i].ToString() + " or ";
                }
                else
                {
                    //or yok ...tek kayıt
                    markaID += "supplierID = " + mr[i].ToString();
                }
            }

            string stock = "";
            if (stoktavarmi == "0")
            {
                stock = "stock >0";
            }
            else
            {
                stock = "stock >=0";
            }


            /*
             * select * from tbl_Products where 
            categoryID = 4 and
            (supplierID = 6 or supplierID = 4 or supplierID = 5) and
            stock >= 0 and
            (price >= 150 and price <= 420)
            */

            string sorgu = "select * from tbl_Products where categoryID = "+ categoryID +" and   ("+ markaID +") and " +stock+  " and (price >= "+ baslangicpara +" and price <= "+ bitispara +")";

            ViewBag.Urunlers = cs.urunlergetirdetaylisorgu(sorgu);

            return View();
        }

        public ActionResult hakkimizda()
        {
            return View();
        }

        public ActionResult hakkimizda_dahafazla()
        {
            return View();
        }

        public ActionResult iletisim()
        {
            return View();
        }

        public ActionResult Mesaj()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Mesaj(tbl_Messages msg)
        //{
        //    db.tbl_Messages.Add(msg);
        //    db.SaveChanges();
        //    Session["Mesaj"] = "Mesajınız bize ulaşmıştır.Teşekkürler.";
        //    //sms gönder ve email gönder metodunu çağır. 
        //    return View();
        //}

    }
}