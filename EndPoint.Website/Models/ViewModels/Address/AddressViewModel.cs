using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.Models.ViewModels.Address
{
    public class AddressViewModel
    {
        [Required(ErrorMessage ="نام ونام خانوادگی نمی تواند خالی باشد")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "استان نمی تواند خالی باشد")]
        public string Province { get; set; }


        [Required(ErrorMessage = "شهر نمی تواند خالی باشد")]
        public string City { get; set; }


        [Required(ErrorMessage = "آدرس نمی تواند خالی باشد")]
        public string FullAddress { get; set; }


        [Required(ErrorMessage = "کد پستی نمی تواند خالی باشد")]
        public string ZipCode { get; set; }


        [Required(ErrorMessage = "شماره تماس نمی تواند خالی باشد")]
        [Phone(ErrorMessage = "شماره تماس را بدرستی وارد نمایید")]
        public string Phone { get; set; }
    }
}
