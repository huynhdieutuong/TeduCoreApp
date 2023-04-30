using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduCoreApp.Infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Tags")]
    public class Tag : DomainEntity<string>
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; }
    }
}
