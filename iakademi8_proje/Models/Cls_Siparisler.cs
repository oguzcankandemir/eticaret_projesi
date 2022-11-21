using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iakademi8_proje.Models
{
    public class Cls_Siparisler
    {
        public int productID { get; set; }
        public int adet { get; set; }
        public string sepet { get; set; }
        public decimal price { get; set; }
        public string productname { get; set; }
        public int kdv { get; set; }
        public string photopath { get; set; }

        Cetin_KirtasiyeEntities2 db = new Cetin_KirtasiyeEntities2();
        //10=1
        //20=1
        //30=1

        //ürün sepette varmı kontrolü
        public bool sepettevarmi(string id)
        {
            bool varmi = true;

            string[] sepetdizi = sepet.Split('&');

            for (int i = 0; i < sepetdizi.Length; i++)
            {
                string[] sepetdizi2 = sepetdizi[i].Split('=');
                //sepetdizi2[0] -> productID
                //sepetdizi2[1} -> adet

                if (sepetdizi2[0] == id)
                {
                    //bu ürün sepete daha önceden eklenmiş
                    //return false
                    varmi = false;
                }
                /*
                else
                {
                    //bu ürün sepete daha önceden eklenmemiş
                    //return true
                    return varmi;
                }*/
            }
            return varmi;
        }

        //sepetim üstüne gelince , sepet bilgilerini görmek için
        public List<Cls_Siparisler> sepetiyazdir()
        {

            List<Cls_Siparisler> sip = new List<Cls_Siparisler>();
            string[] sepetdizi = sepet.Split('&');

            if (sepetdizi[0] != "")
            {

                for (int i = 0; i < sepetdizi.Length; i++)
                {
                    string[] sepetdizi2 = sepetdizi[i].Split('=');
                    int sepetid = Convert.ToInt32(sepetdizi2[0]);

                    if (sepetdizi2[0] != "")
                    {
                        //tarayıcıdaki sepetim içindeki productID lerin karsılıgı olan ürünün bütün bilgilerini alıp geliyorum
                        tbl_Products prd = db.tbl_Products.FirstOrDefault(p => p.productID == sepetid);
                        //prd içinde o ürünün bütün bilgileri

                        //prd ile ürünün bilgilerini, prop + tab + tab ların içine yazdım
                        Cls_Siparisler s = new Cls_Siparisler();
                        s.price = Convert.ToDecimal(prd.price); 
                        s.productname = prd.productname; //ürünün veritabanındaki productname bilgisini,property(prop+tab+tab içine yazdırıyorum)
                        s.productID = prd.productID;
                        s.kdv = Convert.ToInt32(prd.kdv);
                        s.adet = Convert.ToInt32(sepetdizi2[1]);
                        s.photopath = prd.photopath; //ürünün tbl_Products tablosundaki photopath bilgisini,photopath prop + tab + tab a yazdırdım
                        sip.Add(s);
                    }
                }
            }
            return sip;
        }

       //cookie sepetindeki ürünü silelim
        public void sepetten_sil(int scid)
        {
            string yenisepet = "";
            int sayac = 1;
            //10=1&20=1&30=1
            string[] sepetdizi = sepet.Split('&');

            for (int i = 0; i < sepetdizi.Length; i++)
            {
                string[] sepetdizi2 = sepetdizi[i].Split('=');
                int sepetid = Convert.ToInt32(sepetdizi2[0]);
                int miktar = Convert.ToInt32(sepetdizi2[1]);

                if (sepetid != scid)
                {
                    //10 numara silinecekse,10 hariç diger numaralar için girer,silinmeyecek olan numaralar için girer
                    if (sayac == 1)
                    {
                        yenisepet += sepetid + "=" + miktar; //  yenisepet = yenisepet + sepetid;
                        sayac++;
                    }
                    else
                    {
                        yenisepet += "&" + sepetid + "=" + miktar;
                        sayac++;
                    }
                }
            }
            sepet = yenisepet;
        }


        public string cookie_sepetini_siparis_tablosuna_yaz(int ID)
        {
            //sepetdizi
            //10=1 ->  sepetdizi[0]
            //20=1 ->  sepetdizi[1]
            //30=1 ->  sepetdizi[2]

            //sepetdizi2

            //for ilk dönüş
            //10=1
            //10 -> sepetdizi2[0]
            //1 -> sepetdizi2[1]

            //for ikinci dönüş
            //20 -> sepetdizi2[0]
            //1 -> sepetdizi2[1]

            //for üçüncü dönüş
            //30 -> sepetdizi2[0]
            //1 -> sepetdizi2[1]

            DateTime tarih = DateTime.Now;
            string orderGroupGUID = tarih.ToString().Replace(".", "").Replace(" ", "").Replace(":", "");

            string[] sepetdizi = sepet.Split('&');
            for (int i = 0; i < sepetdizi.Length; i++)
            {
                string[] sepetdizi2 = sepetdizi[i].Split('=');

                int ProductID = Convert.ToInt32(sepetdizi2[0]);
                int miktar = Convert.ToInt32(sepetdizi2[1]);

                //yeni kayıt icin tbl_Orders tablosu tipinde,ord isminde bir değişken tanımladım
                tbl_Orders ord = new tbl_Orders();

                //ord içini doldurdum
                ord.userID = ID;
                ord.OrderDate = tarih;
                ord.orderGroupGUID = orderGroupGUID;
                //ord.invoiceAddress = db.tbl_Users.FirstOrDefault(u => u.userID == ID).faturaadresi;
                ord.ProductID = ProductID;
                ord.quantity = miktar;
                ord.aktif = true;

                //dolu olan ord nin icindeki bilgileri , tbl_Orders tablosuna kaydettim
                db.tbl_Orders.Add(ord);
                db.SaveChanges();

            }
            return orderGroupGUID;
        }


        public List<Cls_Siparisler> urunlergetirdetaylisorgu(string hepsi)
        {
            List<Cls_Siparisler> prd = new List<Cls_Siparisler>();

            SqlConnection sqlcon = Models.connection.baglanti;
            SqlCommand sqlcmd = new SqlCommand(hepsi,sqlcon);
            sqlcon.Open();
            SqlDataReader sdr = sqlcmd.ExecuteReader();
            while (sdr.Read())
            {
                Cls_Siparisler p = new Cls_Siparisler();
                p.productID = Convert.ToInt32(sdr["productID"]);

                //sdr["productname"].ToString(); -> veritabanı
                // p.productname -> property (prop + tab + tab)
                p.productname = sdr["productname"].ToString();
                p.price = Convert.ToDecimal(sdr["price"]);
                p.photopath = sdr["photopath"].ToString();
                prd.Add(p);
            }
            sqlcon.Close();
            return prd;
        }

    }
}