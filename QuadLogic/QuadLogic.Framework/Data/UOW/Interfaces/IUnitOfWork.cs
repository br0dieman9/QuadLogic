using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace QuadLogic.Framework.Data.UOW.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Dispose();
        void SaveChanges();
    }
}
