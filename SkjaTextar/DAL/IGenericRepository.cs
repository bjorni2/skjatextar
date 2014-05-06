using System;
using System.Collections.Generic;
using SkjaTextar.Models;
using System.Linq;

namespace SkjaTextar.DAL
{
    // Code from:
    // http://www.asp.net/mvc/tutorials/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    public interface IGenericRepository<T>
    {
        IQueryable<T> Get();
        T GetByID(object id);
        void Insert(T entity);
        void Delete(object id);
        void Delete(T entity);
        void Update(T entity);
    }
}