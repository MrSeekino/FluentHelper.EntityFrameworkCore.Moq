using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentHelper.EntityFrameworkCore.Moq
{
    class DataMocker<T> : IDataMocker<T> where T : class
    {
        List<T> FinalList { get; set; }

        List<T> AddList { get; set; }
        List<T> RemoveList { get; set; }

        public DataMocker(IEnumerable<T> initialData)
        {
            FinalList = initialData == null ? new List<T>() : initialData.ToList();

            AddList = new List<T>();
            RemoveList = new List<T>();
        }

        public int AddListCount()
        {
            return AddList.Count;
        }

        public int RemoveListCount()
        {
            return RemoveList.Count;
        }

        public IQueryable<T> GetAll()
        {
            return FinalList.AsQueryable();
        }

        public void Add(T input)
        {
            AddList.Add(input);
        }

        public void AddRange(IEnumerable<T> inputList)
        {
            AddList.AddRange(inputList);
        }

        public void Remove(T input)
        {
            RemoveList.Add(input);
        }

        public void RemoveRange(IEnumerable<T> inputList)
        {
            RemoveList.AddRange(inputList);
        }

        public int SaveChanges()
        {
            int result = AddList.Count + RemoveList.Count;

            foreach (var addItem in AddList)
            {
                if (FinalList.Any(x => x.Equals(addItem)))
                    throw new Exception("Cannot add twice the same item");

                FinalList.Add(addItem);
            }

            foreach (var removeItem in RemoveList)
            {
                if (!FinalList.Any(x => x.Equals(removeItem)))
                    throw new Exception("Cannot remove items that are not in the list");

                FinalList.Remove(removeItem);
            }

            AddList.Clear();
            RemoveList.Clear();

            return result;
        }
    }
}
