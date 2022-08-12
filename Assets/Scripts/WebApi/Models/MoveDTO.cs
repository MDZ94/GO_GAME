using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.WebApi.Models
{
    public enum MoveType
    {
        putStone,
        pass,
        surrender,
        capture
    }


    public class CreateMoveDTO
    {
        public MoveType Type { get; set; }
        /// <summary>
        /// Required if MoveType = 0 (PutStone)
        /// </summary>
        public short PosX { get; set; }
        /// <summary>
        /// Required if MoveType = 0 (PutStone)
        /// </summary>
        public short PosY { get; set; }
    }

    public class MoveDTO : CreateMoveDTO
    {
        public int Id { get; set; }
        public GetUserDTO ApiUser { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
