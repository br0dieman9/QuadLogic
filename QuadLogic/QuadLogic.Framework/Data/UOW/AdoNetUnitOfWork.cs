using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using QuadLogic.Framework.Data.UOW.Interfaces;

namespace QuadLogic.Framework.Data.UOW
{
    public class AdoNetUnitOfWork : IUnitOfWork
    {
        private TransactionScope _scope;

        public AdoNetUnitOfWork(TransactionScope transaction)
        {
            _scope = transaction;
        }

        public void Dispose()
        {
            if (_scope == null)
                return;

            _scope.Dispose();
            _scope = null;
        }

        public void SaveChanges()
        {
            if (_scope == null)
                throw new InvalidOperationException("May not call save changes twice.");

            _scope.Complete();
        }
    }
}
