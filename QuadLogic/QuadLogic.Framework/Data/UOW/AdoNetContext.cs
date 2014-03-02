using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using QuadLogic.Framework.Data.UOW.Interfaces;
using System.EnterpriseServices;
using System.Data.Common;

namespace QuadLogic.Framework.Data.UOW
{
    public class AdoNetContext : IDisposable, IAdoNetContext
    {
        private IDbConnection _connection;
        private readonly IConnectionFactory _connectionFactory;
        private bool disposed;
        private TransactionScope _scope;

        public AdoNetContext(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        private void OpenConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
                _connection = _connectionFactory.Create();
        }

        /// <summary>
        /// This method will create a new transaction using the TransactionScope class. If you need to create
        /// a transaction that spans multiple contexts (ie databases/platforms), this method only needs to be
        /// called one time on any othe contexts invloved in the transaction. This method call should be wrapped 
        /// in a using block to ensure proper disposal of database resources.
        /// </summary>
        public IUnitOfWork CreateUnitOfWork()
        {
            if (this.disposed) throw new ObjectDisposedException("UOW");

            _scope = new TransactionScope();

            var uow = new AdoNetUnitOfWork(_scope);

            return uow;
        }

        public IDbCommand CreateCommand()
        {
            OpenConnection();

            var cmd = _connection.CreateCommand();

            return cmd;
        }

        public void Dispose()
        {
            if (_scope != null)
                _scope.Dispose();
            if (_connection != null)
                _connection.Dispose();
            disposed = true;
        }
    }
}
