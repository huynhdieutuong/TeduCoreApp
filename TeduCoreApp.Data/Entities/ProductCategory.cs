using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.Interfaces;
using TeduCoreApp.Infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("ProductCategories")]
    public class ProductCategory : DomainEntity<int>, IHasSeoMetaData, IDateTracking, ISortable, ISwitchable
    {
        public ProductCategory()
        {
            Products = new List<Product>();
        }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? ParentId { get; set; }

        public int? HomeOrder { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        public bool? HomeFlag { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public string SeoPageTitle { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(255)]
        public string SeoAlias { get; set; }

        [StringLength(255)]
        public string SeoKeywords { get; set; }

        [StringLength(255)]
        public string SeoDescription { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public int SortOrder { get; set; }

        public Status Status { get; set; }
    }
}
