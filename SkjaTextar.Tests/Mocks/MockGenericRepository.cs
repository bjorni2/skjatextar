using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using SkjaTextar.DAL;
using SkjaTextar.Models;
using System.Web.Mvc;
using SkjaTextar.ViewModels;


namespace SkjaTextar.Tests.Mocks
{
    // Code from:
    // http://www.asp.net/mvc/tutorials/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    public class MockGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        //internal ApplicationDbContext _context;
        //internal DbSet<TEntity> _dbSet;
        public List<TEntity> _list;
        /*public MockGenericRepository(ApplicationDbContext context)
        {
            this._context = context;
            this._dbSet = context.Set<TEntity>(); 
        }*/
        
        public MockGenericRepository(List<TEntity> list)
        {
            _list = list;
        }
        
        public virtual IQueryable<TEntity> Get()
        {
            return _list.AsQueryable();
        }

        public virtual TEntity GetByID(object id)
        {
			throw new NotImplementedException();
            //return _list.(id);
        }

        public virtual void Insert(TEntity entity)
        {
            _list.Add(entity);
        }

        public virtual void Delete(object id)
        {
            throw new NotImplementedException();
            //TEntity entityToDelete = _dbSet.Find(id);
            //Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            throw new NotImplementedException();
            /*if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);*/
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            throw new NotImplementedException();
            //_dbSet.Attach(entityToUpdate);
            //_context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}