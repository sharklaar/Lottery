using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LotteryCalculator.Startup))]
namespace LotteryCalculator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
