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
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	public partial class tbMakinalar
	{
		[Key]
		[ScaffoldColumn(false)]
		public int MakinaID { get; set; }

		[Required]
		[StringLength(50)]
		public string Kodu { get; set; }


		[Required]
		[StringLength(50)]
		public string Tanımı { get; set; } 
		
		public string Grubu { get; set; }
		public string Tipi { get; set; }
		public string SeriNo { get; set; }
		public string Markası { get; set; }
		public string Modeli { get; set; }
		public string Açıklaması { get; set; }


		[Required]
		public int LokasyonID { get; set; }

		[Required]
		public bool PlanlıBakımYapılabilir { get; set; }
		[Required]
		public bool KalibrasyonYapılabilir { get; set; }
		[Required]
		public bool İşİsteklerindeGörüntülenebilir { get; set; }


		public string BakimTalimatKodu { get; set; }
		public string ÖzelKod1 { get; set; }
		public string ÖzelKod2 { get; set; }
		public string ÖzelKod3 { get; set; }


		//[DisplayFormat(DataFormatString = "{0:d}")]
		//[DataType(DataType.Text)]
		public Nullable<System.DateTime>   AlındığıTarih     { get; set; }

		//.................................
			public string AlindigiTarih { get; set; }

		public string AlındığıFirmaBilgileri { get; set; }
		public string SorumluPersonel { get; set; }

		public Nullable<decimal> GünlükÇalışmaSaati { get; set; }
		public byte[] Fotoğrafı { get; set; }
		public bool Aktif { get; set; }
		public Nullable<int> KaydedenID { get; set; }
		public Nullable<System.DateTime> KayıtZamanı { get; set; }
		public string KayıtBilgisayar { get; set; }
		public Nullable<int> SonGuncelleyenID { get; set; }
		public Nullable<System.DateTime> SonGuncellemeZamani { get; set; }
		public string SonGuncellemeBilgisayar { get; set; }
		public string MakinaMesaiBaşlangıçSaati { get; set; }
		public string MakinaMesaiBitişSaati { get; set; }

		public bool xAktif { get; set; }

		public virtual tbLokasyonTanimlari tbLokasyonTanimlari { get; set; }
	}
}