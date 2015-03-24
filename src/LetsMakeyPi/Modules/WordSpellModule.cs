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
                // Show list of games
                return View["Index"];
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

                    return Response.AsRedirect("/word-spell");
                }
            };

            Put["/"] = _ =>
            {
                var gameName = Request.Form.gameName;
                var action = Request.Form.action;

                if (action == "start")
                {
                    WordSpellHub.StartGame(gameName);

                    return Response.AsRedirect("/word-spell");
                }

                return Negotiate
                    .WithReasonPhrase("Missing game action")
                    .WithStatusCode(HttpStatusCode.BadRequest);
            };
        }
    }
}
