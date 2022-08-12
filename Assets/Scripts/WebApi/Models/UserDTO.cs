using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.WebApi.Models
{
    public class LoginUserDTO
    {
        public string email { get; set; }
        public string password { get; set; }

    }

    public class UserDTO : LoginUserDTO
    {
        public string nick { get; set; }
    }

    public class GetUserDTO
    {
        public string id { get; set; }
        public string nick { get; set; }
    }
}
