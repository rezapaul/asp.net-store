using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.Models.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="لطفا نام کابری را وارد نمایید")]
        public string Username { get; set; }

        [Required(ErrorMessage ="لطفا رمز عبور را وارد نمایید")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
