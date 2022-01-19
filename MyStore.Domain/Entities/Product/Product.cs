using MyStore.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Domain.Entities.Product
{
    public class Product : BaseEntity
    {
        public string Title { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int ViewCount { get; set; }
        public bool Displayed { get; set; } = false;
        public Category Category { get; set; }
        public long CategoryId { get; set; }
        public ICollection<ProductColors> Colors { get; set; }
        public ICollection<ProductSizes> Sizes { get; set; }
        public ICollection<ProductImages> Images { get; set; }
    }
}
