﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VillageGame.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class VillageGameEntities2 : DbContext
    {
        public VillageGameEntities2()
            : base("name=VillageGameEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<GameData> GameDatas { get; set; }
        public virtual DbSet<Perk> Perks { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<Villager> Villagers { get; set; }
    }
}
