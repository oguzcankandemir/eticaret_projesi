using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iakademi8_proje.Controllers
{
    public class FooterController : BaseController
    {
        // GET: Footer
        public ActionResult tedarikci()
        {
            return View();
        }
        public ActionResult klavuz()
        {
            return View();
        }
        public ActionResult verikoruma()
        {
            return View();
        }
        public ActionResult teslimatnoktasi()
        {
            return View();
        }

        public ActionResult odemesecenekleri()
        {
            return View();
        }

        public ActionResult bankakampanyalari()
        {
            return View();
        }

       
    }
}