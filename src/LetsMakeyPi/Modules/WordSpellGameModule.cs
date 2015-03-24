using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsMakeyPi.Hubs;
using Nancy;

namespace LetsMakeyPi.Modules
{
    public class WordSpellGameModule : NancyModule
    {
        public WordSpellGameModule()
            : base("/word-spell/{gameName}")
        {
            Get["/"] = parameters =>
            {
                parameters.isObserver = Request.Query.observer ?? false;

                return View["Index", parameters];
            };

            Post["/"] = _ =>
            {
                string redirectUri = string.Format("/word-spell/{0}/{1}?numberOfPlayers={2}",
                    Request.Form["gameName"],
                    Request.Form["teamName"],
                    Request.Form["numberOfPlayers"]);

                return Response.AsRedirect(redirectUri);
            };

            Get["/{teamName}"] = parameters =>
            {
                parameters.numberOfPlayers = Request.Query.numberOfPlayers;

                return View["Show", parameters];
            };
        }
    }
}
