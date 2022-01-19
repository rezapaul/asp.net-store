using MyStore.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Domain.Entities.Product
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public Category ParentCategory { get; set; }
        public long? ParentCategoryId { get; set; }
        public ICollection<Category> SubCategories { get; set; }
    }
}
