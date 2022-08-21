using mvcIsIstek.App_Data;
using System;
using System.Linq;



namespace mvcIsIstek
{
	internal static  class ClsKullanici
	{
		internal static int pr_KullaniciID
		{
			get { return Convert.ToInt32(System.Web.HttpContext.Current.Session["sesKullaniciID"]); }
		}
		internal static string pr_KullanıcıAdı
		{
			get { return Convert.ToString(System.Web.HttpContext.Current.Session["sesKullanıcıAdı"]); }
		}
		internal static Boolean pr_Adminmi
		{
			get { return Convert.ToBoolean(System.Web.HttpContext.Current.Session["sesAdminmi"]); }
		}
 		internal static Boolean pr_BakımOnarımYapabilir
		{
			get { return Convert.ToBoolean(System.Web.HttpContext.Current.Session["sesBakımOnarımYapabilir"]); }
		}
		internal static byte[] pr_Fotografi
		{
			get { return (byte[])(System.Web.HttpContext.Current.Session["sesFotografi"]); }
		}
		internal static string pr_Grubu
		{
			get { return Convert.ToString(System.Web.HttpContext.Current.Session["sesGrubu"]); }
		}
		 	internal static int pr_LokasyonID
		{
			get { return Convert.ToInt32(System.Web.HttpContext.Current.Session["sesLokasyonID"]); }
		}

		internal static string[] pr_KullanicininRolleri => Fn_KullanicininRolleri();
 
		private static string[]  Fn_KullanicininRolleri()
			{
				dbOnemliIstekMakinaBakimEntities db = new dbOnemliIstekMakinaBakimEntities();

			var roller = (from u in db.v_Kullanici_Rolleri
										where u.KullaniciID == pr_KullaniciID
											&& u.RolName != null 
											select new { u.RolName }).ToList() ;

 			string[] sRoller = new string[roller.Count];
 			int i = 0;
			try
			{
				foreach (var rn in roller)
				{
					sRoller.SetValue(rn.RolName, i);
					i++;
				}
			}
			catch (Exception ex)
			{
				var ss = ex.Message;
			}

			//pr_KullanicininRolleri = sRoller;

			return sRoller;
		}

		internal static string  Fn_KullanicininYetkileriControllerVeAction(string sController,string sActionMethod,int sKullaniciID)
		{
			dbOnemliIstekMakinaBakimEntities db = new dbOnemliIstekMakinaBakimEntities();

			var syetki  = "a.turker,veli.Duman,ayşeK";//TODO: bu aslında veritabanında kullanıcının yetkileri tablosundan gelecek.(tablo: tbKullanici_Yetkileri_Controller_Actions)

 

			return syetki;
		}



	}
}