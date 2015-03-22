using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsMakeyPi.Models.WordSpell;
using Microsoft.AspNet.SignalR;

namespace LetsMakeyPi.Hubs
{
    public class WordSpellHub : Hub
    {
        private static readonly GameContainer gameContainer = new GameContainer();

        public static void CreateGame(string gameName)
        {
            gameContainer.CreateGame(gameName);
        }

        public Task JoinGame(JoinGame model)
        {
            if (gameContainer.GameExists(model.GameName))
            {
                gameContainer.AddTeam(new Team()
                {
                    ConnectionId = Context.ConnectionId,
                    GameName = model.GameName,
                    NumberOfPlayers = model.NumberOfPlayers,
                    TeamName = model.TeamName
                });

                return Groups.Add(Context.ConnectionId, model.GameName);
            }
            else
            {
                throw new HubException("Game does not exist");
            }
        }

        public static void StartGame(string gameName)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<WordSpellHub>();

            if (gameContainer.GameExists(gameName))
            {
                var teams = gameContainer.StartGame(gameName);

                foreach (var team in teams)
                {
                    hubContext.Clients.Client(team.ConnectionId).startGame(team.LetterQueue);
                }
            }
            else
            {
                throw new HubException("Game does not exist");
            }
        }

        public void SubmitLetter(SubmitLetter model)
        {
            if (!gameContainer.IsLetterGood(model))
            {
                throw new HubException("Player/Letter combination does not match expected value");
            }

            // check if any winners
        }
    }

    public class GameContainer
    {
        public GameContainer()
        {
            games = new Dictionary<string, ICollection<Team>>();
        }

        private readonly IDictionary<string, ICollection<Team>> games;

        public void AddTeam(Team team)
        {
            var game = games.First(x => x.Key == team.GameName);

            game.Value.Add(team);
        }

        public void CreateGame(string gameName)
        {
            games.Add(gameName, new List<Team>());
        }

        public bool GameExists(string gameName)
        {
            return gameName != null && games.ContainsKey(gameName);
        }

        public bool IsLetterGood(SubmitLetter model)
        {
            var team = games
                .First(x => x.Key == model.GameName)
                .Value.First(x => x.TeamName == model.TeamName);

            return team.IsNextInQueue(new Team.PlayerLetter()
            {
                Letter = model.Letter,
                PlayerNumber = model.PlayerNumber
            });
        }

        public IEnumerable<Team> StartGame(string gameName)
        {
            var word = "word"; // TODO generate word

            var teams = games
                .Where(x => x.Key == gameName)
                .First().Value;

            foreach (var team in teams)
            {
                team.SetWord(word);
            }

            return teams;
        }
    }
}
