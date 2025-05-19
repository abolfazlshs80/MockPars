using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockPars.Application.DTO.Users
{
    public class RegisterDto
    {
        [RegularExpression("^[A-Za-z0-9]+$",ErrorMessage = "نام کاربری باید انگلیسی باشد")]
        public string UserName { get; set; }
        [MinLength(4,ErrorMessage = "حداقل 4 حرف باشد")]
        public string Password { get; set; }
    }
}
