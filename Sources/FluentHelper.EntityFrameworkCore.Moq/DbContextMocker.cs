using FluentHelper.EntityFrameworkCore.Interfaces;
using Moq;
using System;
using System.Collections.Generic;

namespace FluentHelper.EntityFrameworkCore.Moq
{
    public class DbContextMocker
    {
        Dictionary<Type, IDataMocker> MockData { get; set; }
        Mock<IDbContext> MockContext { get; set; }

        public IDbContext Object
        {
            get
            {
                return MockContext.Object;
            }
        }

        public DbContextMocker()
        {
            MockData = new Dictionary<Type, IDataMocker>();
            MockContext = new Mock<IDbContext>();
        }

        public void AddSupportTo<T>(IEnumerable<T> initialData = null) where T : class
        {
            MockData.Add(typeof(T), new DataMocker<T>(initialData));

            MockContext.Setup(c => c.Query<T>()).Returns(((IDataMocker<T>)MockData[typeof(T)]).GetAll());

            MockContext.Setup(c => c.Add(It.IsAny<T>())).Callback<T>(x => ((IDataMocker<T>)MockData[typeof(T)]).Add(x));
            MockContext.Setup(c => c.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(x => ((IDataMocker<T>)MockData[typeof(T)]).AddRange(x));

            MockContext.Setup(c => c.Remove(It.IsAny<T>())).Callback<T>(x => ((IDataMocker<T>)MockData[typeof(T)]).Remove(x));
            MockContext.Setup(c => c.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(x => ((IDataMocker<T>)MockData[typeof(T)]).RemoveRange(x));

            MockContext.Setup(c => c.SaveChanges()).Callback(() =>
            {
                foreach (var mockData in MockData)
                    mockData.Value.SaveChanges();
            });
        }
    }
}
