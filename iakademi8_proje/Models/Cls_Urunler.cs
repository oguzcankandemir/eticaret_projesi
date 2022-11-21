using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iakademi8_proje.Models
{
    public class Cls_Urunler
    {
       Cetin_KirtasiyeEntities2 db = new Cetin_KirtasiyeEntities2();
        List<WV_aktifurunler> list;
        WV_aktifurunler lst;


        //public , dönüs tipi  , metodun adı, metod parametre almayacak
        //return 
        public List<WV_aktifurunler> sliderUrunlerGetir()
        {
             list = db.WV_aktifurunler.Where(p => p.stock == 10).ToList();
            return list;
        }

        public void one_cikanlar_kolonunu_arttir(int id)
        {
            tbl_Products prd = db.tbl_Products.FirstOrDefault(p => p.productID == id);
            prd.onecikanlar = prd.onecikanlar + 1;
            db.SaveChanges();
        }

        public WV_aktifurunler gunun_urununu_getir()
        {
             lst = db.WV_aktifurunler.FirstOrDefault(p => p.status == 1);
            return lst;
        }

        
        public List<WV_aktifurunler> enyeniurunler_getir(string hepsimi,int adet)
        {
            if (hepsimi == "anasayfa")
            {
                //index sayfası
                list = db.WV_aktifurunler.Where(p => p.stock == 6).OrderBy(p => p.productname).Take(8).ToList();
            }
            else if (hepsimi == "EnYeniUrunler")
            {
                //en yeni ürünler sayfası,ilk yüklenen 4 ürün
                list = db.WV_aktifurunler.OrderByDescending(p => p.stock == 7).Take(4).ToList();
            }
            else
            {
                //en yeni ürünler sayfası , partial sayfa, daha fazla ürün getir
                int sayfano = Convert.ToInt32(hepsimi);
                list = db.WV_aktifurunler.OrderByDescending(p => p.adddate).Skip(sayfano * 4).Take(4).ToList();
            }
            return list;
        }


        public List<WV_aktifurunler> ozelurunler_getir(string hepsimi)
        {
            if (hepsimi == "anasayfa")
            {
                list = db.WV_aktifurunler.Where(p => p.stock == 5).OrderBy(p => p.productname).Take(8).ToList();
            }
            else if (hepsimi == "Ozeller")
            {
                list = db.WV_aktifurunler.Where(p => p.status == 5).OrderBy(p => p.productname).Take(4).ToList();
            }
            else
            {
                int sayfano = Convert.ToInt32(hepsimi);
                list = db.WV_aktifurunler.Where(p => p.stock == 7).OrderBy(p => p.productname).Skip(sayfano * 4).Take(4).ToList();
            }
            return list;
        }


       // List<vw_aktifurunler> list;
        public List<WV_aktifurunler> indirimliurunler_getir(string hepsimi)
        {
            if (hepsimi == "anasayfa")
            {
                list = db.WV_aktifurunler.OrderByDescending(p => p.categoryID).Take(8).ToList();
            }
            else if (hepsimi == "indirimliUrunler")
            {
                list = db.WV_aktifurunler.OrderByDescending(p => p.discount).Take(4).ToList();
            }
            else
            {
                int sayfano = Convert.ToInt32(hepsimi);
                list = db.WV_aktifurunler.OrderByDescending(p => p.discount).Skip(sayfano * 4).Take(4).ToList();
            }
             
            return list;
        }


        public List<WV_aktifurunler> onecikanurunler_getir(string hepsimi)
        {
            if (hepsimi == "anasayfa")
            {
                list = db.WV_aktifurunler.OrderByDescending(p => p.onecikanlar).Take(8).ToList();
            }
            else
            {
                list = db.WV_aktifurunler.OrderByDescending(p => p.onecikanlar).ToList();
            }
            return list;
        }


        public List<WV_aktifurunler> coksatanurunler_getir(string hepsimi)
        {
            if (hepsimi == "anasayfa")
            {
                list = db.WV_aktifurunler.OrderByDescending(p => p.coksatanlar).Take(8).ToList();
            }
            else
            {
                list = db.WV_aktifurunler.OrderByDescending(p => p.coksatanlar).ToList();
            }
            return list;
        }

        public List<WV_aktifurunler> firsaturunler_getir()
        {
             list = db.WV_aktifurunler.Where(p => p.status == 4).Take(8).ToList();
            return list;
        }
        public List<WV_aktifurunler> yildizliurunler_getir()
        {
             list = db.WV_aktifurunler.Where(p => p.status == 5).Take(8).ToList();
            return list;
        }

        public List<WV_aktifurunler> superfiyatteklifUrunler_getir()
        {
             list = db.WV_aktifurunler.Where(p => p.status == 6).Take(8).ToList();
            return list;
        }

        public List<WV_aktifurunler> dikkatcekenUrunler_getir()
        {
             list = db.WV_aktifurunler.Where(p => p.status == 4).Take(8).ToList();
            return list;
        }


        public static List<vw_arama> arama_getir(string aranan)
        {
            List<vw_arama> sonuc = new List<vw_arama>();
            using (Cetin_KirtasiyeEntities2 db2 = new Cetin_KirtasiyeEntities2())
            {
                sonuc = db2.vw_arama.Where(p => p.ARAMAISMI.Contains(aranan)).Take(10).ToList();
            }
            return sonuc;
        }



        public WV_aktifurunler detayli_urun_getir(int id)
        {
            WV_aktifurunler prd = db.WV_aktifurunler.FirstOrDefault(p => p.productID == id);
            return prd;
        }

        public List<WV_aktifurunler> benzer_urunler_getir(int catID)
        {
            List<WV_aktifurunler> ulist = db.WV_aktifurunler.Where(u => u.categoryID == catID).Take(4).ToList();
            return ulist;
        }

        public List<WV_aktifurunler> bunabakanlar_getir(string bunabakan)
        {
            List<WV_aktifurunler> ulist = db.WV_aktifurunler.Where(u => u.bunabakanlar_bunadabakti == bunabakan).Take(4).ToList();
            return ulist;
        }


        public List<WV_aktifurunler> urunlerigetir()
        {
            List<WV_aktifurunler> lst = db.WV_aktifurunler.OrderBy(p => p.productID).ToList();
            return lst;
        }

        

    }
}