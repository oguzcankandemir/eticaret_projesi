using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace iakademi8_proje.Models
{
    public class Cls_Users
    {
        public string email { get; set; }
        public string parola { get; set; }
        public string isimsoyisim { get; set; }
        public string telefon { get; set; }
        public string faturaadresi { get; set; }


        Cetin_KirtasiyeEntities2 db = new Cetin_KirtasiyeEntities2();

        public static void uyeekle(tbl_Users usr)
        {
            //tbl_Users tablosuna kaydedecek
            using (Cetin_KirtasiyeEntities2 db2 = new Cetin_KirtasiyeEntities2())
            {
                usr.adminmi = false;
                usr.aktif = true;
                usr.parola = MD5sifrele(usr.parola);
                db2.tbl_Users.Add(usr);
                db2.SaveChanges();
            }
        }

        public static string MD5sifrele(string metin)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] btr = Encoding.UTF8.GetBytes(metin);
            btr = md5.ComputeHash(btr);

            StringBuilder sb = new StringBuilder();
            foreach (byte item in btr)
            {
                sb.Append(item.ToString("x2").ToLower());
            }

            return sb.ToString();
        }

        public List<Cls_Users> uyegetir(int userID)
        {
            List<Cls_Users> list = new List<Cls_Users>();
            try
            {
                var usr = db.tbl_Users.FirstOrDefault(us => us.userID == userID);
                Cls_Users u = new Cls_Users();
                u.email = usr.email;
                u.parola = usr.parola;
                u.isimsoyisim = usr.isimsoyisim;
                u.telefon = usr.telefon;
                u.faturaadresi = usr.faturaadresi;
                list.Add(u);
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}