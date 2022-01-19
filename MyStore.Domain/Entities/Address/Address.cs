using MyStore.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Domain.Entities.Address
{
    public class Address : BaseEntity
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Province{ get; set; }
        public string City { get; set; }
        public string FullAddress { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }

    }
}
