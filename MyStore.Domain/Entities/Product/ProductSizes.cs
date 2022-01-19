using MyStore.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Domain.Entities.Product
{
    public class ProductSizes : BaseEntity
    {
        public string Size { get; set; }
        public Product Product { get; set; }
        public long ProductId { get; set; }

    }
}
