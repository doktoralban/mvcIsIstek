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
    
    public partial class v_Kullanici_Rolleri
    {
        public int KullaniciID { get; set; }
        public string KullanıcıAdı { get; set; }
        public string AdıSoyadı { get; set; }
        public string Grubu { get; set; }
        public string Tipi { get; set; }
        public int KullaniciRolID { get; set; }
        public int RolID { get; set; }
        public string RolName { get; set; }
        public string Ünvanı { get; set; }
        public Nullable<int> LokasyonID { get; set; }
        public string Lokasyon { get; set; }
        public string Şirket { get; set; }
        public string Fabrika { get; set; }
        public string Şube { get; set; }
        public string Departman { get; set; }
        public Nullable<int> FaaliyetAlaniID { get; set; }
        public string FaaliyetAlani { get; set; }
        public string Emaili { get; set; }
        public byte[] Fotografi { get; set; }
        public bool Adminmi { get; set; }
        public string CepTelefonu { get; set; }
        public string DahiliTelefonu { get; set; }
        public Nullable<bool> BakımOnarımYapabilir { get; set; }
        public bool Aktif { get; set; }
    }
}
