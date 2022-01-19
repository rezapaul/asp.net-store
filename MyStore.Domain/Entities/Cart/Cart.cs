using MyStore.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Domain.Entities.Cart
{
    public class Cart : BaseEntity
    {
        public string UserId { get; set; }
        public Guid BrowserId { get; set; }
        public bool Finished { get; set; }
        public DateTime FinishTime { get; set; }
        public ICollection<CartItem> CartItems { get; set; }

    }


    public class CartItem : BaseEntity
    {
        public Product.Product Product { get; set; }
        public long ProductId { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public Cart Cart { get; set; }
        public long CartId { get; set; }
    }
}
