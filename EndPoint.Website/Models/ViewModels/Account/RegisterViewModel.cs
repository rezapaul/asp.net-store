using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.Models.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="لطفا نام و نام خانوادگی را وارد نمایید")]
        public string FullName { get; set; }

        [Required(ErrorMessage ="لطفا ایمیل را وارد نمایید ")]
        [EmailAddress(ErrorMessage ="لطفا ایمیل را بدرستی وارد نمایید")]
        [Remote("IsEmailInUse","Account",HttpMethod ="POST",AdditionalFields = "__RequestVerificationToken")]
        public string Email { get; set; }

        [Required(ErrorMessage ="لطفا نام کاربری را وارد نمایید")]
        [Remote("IsUserNameInUse","Account",HttpMethod ="POST",AdditionalFields = "__RequestVerificationToken")]
        public string Username { get; set; }

        [Required(ErrorMessage ="لطفا رمز عبور را وارد نمایید")]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا تکرار رمز عبور را وارد نمایید")]
        [Compare("Password",ErrorMessage ="رمز عبور وارد شده یکسان نمی باشد ")]
        public string RePassword { get; set; }

        [Required(ErrorMessage ="لطفا شماره موبایل را وارد نمایید")]
        [Phone(ErrorMessage = "لطفا شماره موبایل را بدرستی وارد نمایید")]
        public string Phone { get; set; }

    }
}
