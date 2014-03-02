using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadLogic.Framework.Data.UOW.Interfaces
{
    public interface IConnectionFactory
    {
        System.Data.IDbConnection Create();
    }
}
