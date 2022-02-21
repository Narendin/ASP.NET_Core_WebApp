using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Timesheets.BD.Interfaces
{
    /// <summary>
    /// Базовый интерфейс рапозитория
    /// </summary>
    /// <typeparam name="T">Обект базы данных</typeparam>
    public interface IDbRepository<T>
    {
        /// <summary>
        /// Асинхрнонное получение объекта из базы
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <param name="cts">Маркер отмены</param>
        /// <returns></returns>
        Task<T> GetAsync(int id, CancellationToken cts);

        /// <summary>
        /// Асинхронное добавление объекта в базу
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="cts">Маркер отмены</param>
        /// <returns></returns>
        Task AddAsync(T obj, CancellationToken cts);

        /// <summary>
        /// Асинхронное обновление полей объекта в базе данных
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="cts">Маркер отмены</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T obj, CancellationToken cts);

        /// <summary>
        /// Асинхронное удаление объекта из базы данных
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <param name="cts">Маркер отмены</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id, CancellationToken cts);

        /// <summary>
        /// Асинхронное получение списка объектов с пагинацией
        /// </summary>
        /// <param name="skip">Количество пропускаемых объектов</param>
        /// <param name="take">Количество получаемых объектов</param>
        /// <param name="cts">Маркер отмены</param>
        /// <returns>Список объектов</returns>
        Task<List<T>> GetPaginationAsync(int skip, int take, CancellationToken cts);

        /// <summary>
        /// Асинхронный поиск объекта по имени
        /// </summary>
        /// <param name="name">Имя объекта</param>
        /// <param name="cts">Маркер отмены</param>
        /// <returns>Искомый объект</returns>
        Task<List<T>> FindByNameAsync(string name, CancellationToken cts);
    }
}