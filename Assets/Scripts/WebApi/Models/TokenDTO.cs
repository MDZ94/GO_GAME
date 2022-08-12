using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.WebApi.Models
{
    class TokenDTO
    {
        public TokenDTO(string token) {
            this.token = token;
        }

        public string token { get; set; }
    }
}
