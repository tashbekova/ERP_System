//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EstelaVidaShop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Purchase
    {
        public int ID { get; set; }
        public Nullable<int> Material { get; set; }
        public Nullable<decimal> Count { get; set; }
        public Nullable<decimal> Sum { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> Worker { get; set; }
    
        public virtual Material Material1 { get; set; }
        public virtual Worker Worker1 { get; set; }
    }
}
