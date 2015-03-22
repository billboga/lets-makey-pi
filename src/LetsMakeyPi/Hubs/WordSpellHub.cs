using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsMakeyPi.Models.WordSpell;
using Microsoft.AspNet.SignalR;

namespace LetsMakeyPi.Hubs
{
    public class WordSpellHub : Hub
    {
        private static readonly GameContainer gameContainer = new GameContainer();

        public void CreateGame(string gameName)
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

        public void StartGame(string gameName)
        {
            if (gameContainer.GameExists(gameName))
            {
                var teams = gameContainer.StartGame(gameName);

                foreach (var team in teams)
                {
                    Clients.Client(team.ConnectionId).startGame(team.LetterQueue);
                }
            }
            else
            {
                throw new HubException("Game does not exist");
            }
        }

        public void SubmitLetter(SubmitLetter model)
        {
            // get team based on submitted game and team name
            // check if submitted player and letter match next pair in queue
            // adjust queue if good
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
