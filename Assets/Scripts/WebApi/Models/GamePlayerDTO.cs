using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.WebApi.Models
{
    public class CreateGamePlayerDTO
    {
        public bool blackColor { get; set; }
    }

    public class GamePlayerDTO
    {
        public int gameId { get; set; }
        public GetUserDTO apiUser { get; set; }
        public bool blackColor { get; set; }
        public bool gameOwner { get; set; }
        public bool ready { get; set; }
    }
}
