using Nancy;

namespace LetsMakeyPi.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ =>
            {
                return View["Index"];
            };
        }
    }
}
