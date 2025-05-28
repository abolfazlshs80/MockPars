using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockPars.Infrastructure.Models.Authebntication
{
    public class UserLoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class UserRegisterDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
