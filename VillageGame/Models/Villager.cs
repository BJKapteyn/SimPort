//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Villager
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> Perk { get; set; }
    
        public virtual Perk Perk1 { get; set; }
    }
}
