using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadLogic.Framework.Data.UOW.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class 
    {
        List<TEntity> GetAll();
        TEntity GetById(object id);
        int Create(TEntity entity); 
        void Update(TEntity entity); 
        void Delete(object id); 
        void Delete(TEntity entity);         
    }
}
