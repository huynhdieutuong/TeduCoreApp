using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduCoreApp.Data.Enums;

namespace TeduCoreApp.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { get; set; }
    }
}
