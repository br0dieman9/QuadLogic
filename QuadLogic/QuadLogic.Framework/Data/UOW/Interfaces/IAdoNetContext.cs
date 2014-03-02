using System;
using System.Data;
using QuadLogic.Framework.Data.UOW.Interfaces;

namespace QuadLogic.Framework.Data.UOW.Interfaces
{
    public interface IAdoNetContext
    {
        IDbCommand CreateCommand();

        /// <summary>
        /// This method will create a new transaction using the TransactionScope class. If you need to create
        /// a transaction that spans multiple contexts (ie databases/platforms), this method only needs to be
        /// called one time on any othe contexts invloved in the transaction. This method call should be wrapped 
        /// in a using block to ensure proper disposal of database resources.
        /// </summary>
        /// <returns></returns>
        IUnitOfWork CreateUnitOfWork();
        void Dispose();
    }
}
