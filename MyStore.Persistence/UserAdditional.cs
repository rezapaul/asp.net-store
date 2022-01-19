using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Persistence
{
    public class UserAdditional : IdentityUser
    {
        [MinLength(10)]
        [MaxLength(50)]
        public string FullName { get; set; }
    }
}
