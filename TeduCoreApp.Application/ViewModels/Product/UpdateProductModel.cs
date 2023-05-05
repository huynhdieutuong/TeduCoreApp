using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeduCoreApp.Application.ViewModels.Product
{
    public class UpdateProductModel
    {
        [StringLength(255)]
        public string Name { get; set; }

        public int CategoryId { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        [DefaultValue(0)]
        public decimal Price { get; set; }

        [StringLength(255)]
        public string Tags { get; set; }
    }
}