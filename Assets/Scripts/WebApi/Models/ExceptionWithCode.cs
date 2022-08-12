using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.WebApi.Models
{
    public class ExceptionWithCode : Exception
    {
        public ExceptionWithCode(int code, string message) : base(message) {
            Code = code;
        }
        public int Code { get; set; }

        // 2001 - wrong login or password
        // 2002 - Log in first
        // 2003 - Couldn't refresh token
    }
}
