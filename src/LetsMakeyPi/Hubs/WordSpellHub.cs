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
                var team = new Team()
                {
                    ConnectionId = Context.ConnectionId,
                    GameName = model.GameName,
                    NumberOfPlayers = model.NumberOfPlayers,
                    TeamName = model.TeamName
                };

                gameContainer.AddTeam(team);

                Clients.Group(model.GameName).teamJoined(team);

                return Observe(model.GameName);
            }
            else
            {
                throw new HubException("Game does not exist");
            }
        }

        public Task Observe(string gameName)
        {
            return Groups.Add(Context.ConnectionId, gameName);
        }

        public static void StartGame(string gameName)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<WordSpellHub>();

            if (gameContainer.GameExists(gameName))
            {
                var teams = gameContainer.StartGame(gameName);

                foreach (var team in teams)
                {
                    var data = new
                    {
                        word = team.Word,
                        letters = team.LetterQueue
                    };

                    hubContext.Clients.Client(team.ConnectionId).startGame(data);
                }

                hubContext.Clients.Group(gameName).letterSubmitted(teams);
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

            var teams = gameContainer.GetTeams(model.GameName);

            Clients.Group(model.GameName).letterSubmitted(teams);
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

        public IEnumerable<Team> GetTeams(string gameName)
        {
            var teams = games
                .First(x => x.Key == gameName)
                .Value;

            return teams;
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
