//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAPI
{
    using System;
    using System.Collections.Generic;
    
    public partial class HistoryTable
    {
        public System.Guid Id { get; set; }
        public System.Guid PumpId { get; set; }
        public System.DateTime Date { get; set; }
        public System.TimeSpan Time { get; set; }
        public int State { get; set; }
    
        public virtual PumpTable PumpTable { get; set; }
    }
}
