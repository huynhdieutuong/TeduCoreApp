using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeduCoreApp.Data.Interfaces
{
    public interface IHasOwner<T>
    {
        T OwnerId { get; set; }
    }
}
