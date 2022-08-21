using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mvcIsIstek.Startup))]
namespace mvcIsIstek
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureAuth(app);
		}
	}

}