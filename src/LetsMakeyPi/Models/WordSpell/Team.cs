using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsMakeyPi.Models.WordSpell
{
    public class Team
    {
        public Team()
        {
            letterQueue = new Queue<PlayerLetter>();
        }

        public string ConnectionId { get; set; }
        public string GameName { get; set; }
        public int NumberOfPlayers { get; set; }
        private Queue<PlayerLetter> letterQueue { get; set; }
        public string TeamName { get; set; }
        private string word { get; set; }

        public Queue<PlayerLetter> LetterQueue
        {
            get
            {
                return letterQueue;
            }
        }

        public void SetWord(string word)
        {
            var wordLetterIndex = 0;;
            var playerIndex = 1;

            while (wordLetterIndex < word.Count())
            {
                if (playerIndex > NumberOfPlayers)
                {
                    playerIndex = 1;
                }

                letterQueue.Enqueue(new PlayerLetter()
                {
                    Letter = word[wordLetterIndex],
                    PlayerNumber = playerIndex
                });

                wordLetterIndex++;
                playerIndex++;
            }
        }

        private class PlayerLetter
        {
            public char Letter { get; set; }
            public int PlayerNumber {  get; set; }
        }
    }
}
