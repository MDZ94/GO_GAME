using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.WebApi.Models
{
    public class ScoreDTO
    {
        public int BlackTerritory { get; set; }
        public int WhiteTerritory { get; set; }
        public int NeutralTerritory { get; set; }
    }
}
