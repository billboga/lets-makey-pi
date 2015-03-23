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
        public string Word { get; set; }

        public IEnumerable<PlayerLetter> LetterQueue
        {
            get
            {
                return letterQueue.Select(x => x);
            }
        }

        public bool IsNextInQueue(PlayerLetter playerLetter)
        {
            var head = letterQueue.Peek();

            if (playerLetter.Equals(head))
            {
                letterQueue.Dequeue();

                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetWord(string word)
        {
            Word = word;

            var wordLetterIndex = 0;
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

        public class PlayerLetter
        {
            public char Letter { get; set; }
            public int PlayerNumber {  get; set; }

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }

                if (obj as PlayerLetter == null)
                {
                    return false;
                }

                return
                    this.Letter == ((PlayerLetter)obj).Letter
                    && this.PlayerNumber == ((PlayerLetter)obj).PlayerNumber;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}
