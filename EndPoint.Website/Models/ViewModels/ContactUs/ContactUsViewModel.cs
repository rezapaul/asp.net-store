using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.Models.ViewModels.ContactUs
{
    public class ContactUsViewModel
    {
        [Required(ErrorMessage ="نام و نام خانوادگی نمی تواند خالی باشد")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ایمیل نمی تواند خالی باشد")]
        [EmailAddress(ErrorMessage = "لطفا ایمیل را به درستی وارد کنید")]
        public string Email { get; set; }

        [Required(ErrorMessage = "موضوع نمی تواند خالی باشد")]
        public string Title { get; set; }

        [Required(ErrorMessage = "متن پیام نمی تواند خالی باشد")]
        public string Text { get; set; }
    }
}
