using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeduCoreApp.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Call save change from db context
        /// </summary>
        void Commit();
    }
}
