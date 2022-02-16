using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Timesheets.BD.Interfaces;
using Timesheets.BD.Models;

namespace Timesheets.BD.Repositories
{
    /// <summary>
    /// Репозиторий для локальной базы данных.
    /// Хоть методы и называются асинхронными,
    /// реализация будет без асинхронности.
    /// </summary>
    public class LocalDbRepository : IDbRepository<Person>
    {
        public Task<Person> GetAsync(int id, CancellationToken cts)
        {
            var person = LocalDb.Data.Find(x => x.Id == id);

            if (person == null)
            {
                return Task.Run(() => new Person(), cts);
            }

            return Task.Run(() => person, cts);
        }

        public Task AddAsync(Person obj, CancellationToken cts)
        {
            obj.Id = LocalDb.GetNextId();
            return Task.Run(() => LocalDb.Data.Add(obj), cts);
        }

        public Task UpdateAsync(Person obj, CancellationToken cts)
        {
            var personIndex = LocalDb.Data.FindIndex(x => x.Id == obj.Id);
            if (personIndex <= 0)
            {
                return Task.CompletedTask;
            }
            return Task.Run(() => LocalDb.Data[personIndex] = obj, cts);
        }

        public Task DeleteAsync(int id, CancellationToken cts)
        {
            var personIndex = LocalDb.Data.Find(x => x.Id == id);
            if (personIndex == null)
            {
                return Task.CompletedTask;
            }

            return Task.Run(() => LocalDb.Data.Remove(personIndex), cts);
        }

        public Task<List<Person>> GetPaginationAsync(int skip, int take, CancellationToken cts)
        {
            int maxNum = LocalDb.Data.Count;
            List<Person> list = new List<Person>();

            if (skip < 0 || take < 0 || skip + take == maxNum || skip >= maxNum)
            {
                return Task.Run(() => list, cts);
            }

            if (skip + take > maxNum)
            {
                take = maxNum - skip;
            }

            for (int i = skip; i < skip + take; i++)
            {
                list.Add(LocalDb.Data[i]);
            }

            return Task.Run(() => list, cts);
        }

        public Task<Person> FindByNameAsync(string name, CancellationToken cts)
        {
            var person = LocalDb.Data.Find(x => x.FirstName == name);

            if (person == null)
            {
                return Task.Run(() => new Person(), cts);
            }

            return Task.Run(() => person, cts);
        }
    }
}