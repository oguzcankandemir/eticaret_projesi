using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iakademi8_proje.Models
{

    public class AnaSayfaModel
    {
        public List<WV_aktifurunler> sliderUrunler { get; set; }
        public WV_aktifurunler gununUrunu { get; set; }
        public List<WV_aktifurunler> enyeniUrunler { get; set; }
        public List<WV_aktifurunler> ozelUrunler { get; set; }
        public List<WV_aktifurunler> indirimliUrunler { get; set; }
        public List<WV_aktifurunler> onecikanUrunler { get; set; }
        public List<WV_aktifurunler> coksatanUrunler { get; set; }
        public List<WV_aktifurunler> firsatUrunler { get; set; }
        public List<WV_aktifurunler> yildizliUrunler { get; set; }
        public List<WV_aktifurunler> superfiyatteklifUrunler { get; set; }
        public List<WV_aktifurunler> dikkatcekenUrunler { get; set; }
        public WV_aktifurunler detayliUrun { get; set; }
        public List<WV_aktifurunler> vw_benzer_urunler { get; set; }
        public List<WV_aktifurunler> vw_bunabakanlar { get; set; }
    }


}