//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mvcIsIstek.App_Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbIsIstek_EkliDosyalar
    {
        public int IsIstekEkliDosyaID { get; set; }
        public int IsIstekID { get; set; }
        public string EkDosyaTipi { get; set; }
        public string EkDosyaAdı { get; set; }
        public string EkDosyaKlasorYolu { get; set; }
        public int EkleyenKullaniciID { get; set; }
        public Nullable<System.DateTime> KayıtZamanı { get; set; }
        public string KayıtBilgisayar { get; set; }
    
        public virtual tbIsIstekleri tbIsIstekleri { get; set; }
    }
}
