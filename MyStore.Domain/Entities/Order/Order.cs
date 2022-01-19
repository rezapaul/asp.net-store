using MyStore.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Domain.Entities.Order
{
    public class Order : BaseEntity
    {
        public Cart.Cart Cart { get; set; }
        public long CartId { get; set; }
        public Address.Address Address { get; set; }
        public long AddressId { get; set; }
        public Status Status { get; set; }

    }

    public enum Status
    {
        Recived = 0,
        Accepted = 1,
        Processing = 2,
        Sent = 3,
        RecivedByCustomer = 4
    }
}
