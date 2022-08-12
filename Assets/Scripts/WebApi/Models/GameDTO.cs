using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.WebApi.Models
{
    public enum BoardSize
    {
        _9x9,
        _13x13,
        _19x19
    }

    public class CreateGameDTO
    {
        public string name { get; set; }
        public IList<CreateGamePlayerDTO> gamePlayers { get; set; }
        public BoardSize boardSize { get; set; }
        public int timeLimit { get; set; }
    }

    public class GameDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public IList<GamePlayerDTO> gamePlayers { get; set; }
        public BoardSize boardSize { get; set; }
        public int timeLimit { get; set; }
        public DateTime createdDate { get; set; }
        public GameStartDTO gameStart { get; set; }
        public GameFinishDTO gameFinish { get; set; }
        public GameWinnerDTO gameWinner { get; set; }
    }
}
