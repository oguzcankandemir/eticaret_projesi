﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Cetin_KirtasiyeEntities2 : DbContext
    {
        internal IEnumerable<object> vw_siparislerim;

        public Cetin_KirtasiyeEntities2()
            : base("name=Cetin_KirtasiyeEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbl_Categories> tbl_Categories { get; set; }
        public virtual DbSet<tbl_Orders> tbl_Orders { get; set; }
        public virtual DbSet<tbl_Products> tbl_Products { get; set; }
        public virtual DbSet<tbl_Suppliers> tbl_Suppliers { get; set; }
        public virtual DbSet<tbl_Users> tbl_Users { get; set; }
        public virtual DbSet<vw_arama> vw_arama { get; set; }
        public virtual DbSet<WV_aktifurunler> WV_aktifurunler { get; set; }
        public virtual DbSet<WV_siparislerim> WV_siparislerim { get; set; }
    }
}
