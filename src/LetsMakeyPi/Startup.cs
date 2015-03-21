using Nancy;
using Nancy.Owin;
using Owin;

namespace LetsMakeyPi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app
                .UseNancy(configuration => configuration.PassThroughWhenStatusCodesAre(
                    HttpStatusCode.NotFound,
                    HttpStatusCode.InternalServerError))
                .MapSignalR();
        }
    }
}
