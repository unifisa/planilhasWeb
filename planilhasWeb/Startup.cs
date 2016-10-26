using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Planilhas.Startup))]
namespace Planilhas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
