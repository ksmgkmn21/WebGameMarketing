﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebGameMarketing.Models.EntitiyFramwork
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WebMarketingEntities : DbContext
    {
        public WebMarketingEntities()
            : base("name=WebMarketingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Belongs_To> Belongs_To { get; set; }
        public virtual DbSet<Games> Games { get; set; }
        public virtual DbSet<Operates_On> Operates_On { get; set; }
        public virtual DbSet<Platforms> Platforms { get; set; }
        public virtual DbSet<Publisher> Publisher { get; set; }
        public virtual DbSet<Img_Path> Img_Path { get; set; }
        public virtual DbSet<Library> Library { get; set; }
        public virtual DbSet<Review> Review { get; set; }
        public virtual DbSet<Scart> Scart { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
