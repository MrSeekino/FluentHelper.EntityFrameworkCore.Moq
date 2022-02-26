using System.Collections.Generic;
using System.Linq;

namespace FluentHelper.EntityFrameworkCore.Moq
{
    interface IDataMocker
    {
        int SaveChanges();
    }

    interface IDataMocker<T> : IDataMocker
    {
        IQueryable<T> GetAll();

        void Add(T input);
        void AddRange(IEnumerable<T> inputList);

        void Remove(T input);
        void RemoveRange(IEnumerable<T> inputList);
    }
}
