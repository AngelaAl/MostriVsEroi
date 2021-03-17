using System;
using System.Collections.Generic;
using System.Text;

namespace MostriVsEroi.Core.Interfaces
{
    public interface IRepository<T>
    {
        //CRUD
        void Create(T obj);
        
        
        IEnumerable<T> GetAll();
        bool Update(T obj);
        bool Delete(T obj);
    }
}
