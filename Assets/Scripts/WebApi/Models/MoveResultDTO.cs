using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.WebApi.Models
{
    public class MoveResultDTO
    {
        public MoveDTO MoveDTO { get; set; }
        public List<StoneDTO> CapturedStones { get; set; }
    }
}
