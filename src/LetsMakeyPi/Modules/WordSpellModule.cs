using LetsMakeyPi.Hubs;
using Nancy;

namespace LetsMakeyPi.Modules
{
    public class WordSpellModule : NancyModule
    {
        public WordSpellModule()
            : base("/word-spell")
        {
            Get["/"] = _ =>
            {
                var model = new
                {
                    gameName = Request.Query.gameName ?? string.Empty
                };

                return View["Index",  model];
            };

            Post["/"] = _ =>
            {
                var gameName = Request.Form.gameName;

                if (gameName == null)
                {
                    return Negotiate
                        .WithReasonPhrase("Missing game name")
                        .WithStatusCode(HttpStatusCode.BadRequest);
                }
                else
                {
                    WordSpellHub.CreateGame(gameName);

                    string uri = string.Format("/word-spell?gameName={0}", gameName);

                    return Response.AsRedirect(uri);
                }
            };

            Put["/"] = _ =>
            {
                var gameName = Request.Form.gameName;
                var word = Request.Form.word;
                var action = Request.Form.action;

                if (action == "start")
                {
                    WordSpellHub.StartGame(gameName, word);

                    string uri = string.Format("/word-spell/{0}", gameName);

                    return Response.AsRedirect(uri);
                }

                return Negotiate
                    .WithReasonPhrase("Missing game action")
                    .WithStatusCode(HttpStatusCode.BadRequest);
            };
        }
    }
}
