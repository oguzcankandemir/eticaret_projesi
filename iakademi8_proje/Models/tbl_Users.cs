//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iakademi8_proje.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Users
    {
        public int userID { get; set; }
        public string email { get; set; }
        public string parola { get; set; }
        public string isimsoyisim { get; set; }
        public string telefon { get; set; }
        public string faturaadresi { get; set; }
        public Nullable<bool> adminmi { get; set; }
        public Nullable<bool> aktif { get; set; }
    }
}
