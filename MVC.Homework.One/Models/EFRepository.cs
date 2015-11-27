using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MVC.Homework.One.Models
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        public IUnitOfWork UnitOfWork { get; set; }

        private IDbSet<T> _objectset;
        private IDbSet<T> ObjectSet
        {
            get
            {
                if (_objectset == null)
                {
                    _objectset = UnitOfWork.Context.Set<T>();
                }
                return _objectset;
            }
        }

        public IQueryable<T> ObjectDataSet
        {
            get
            {
                return UnitOfWork.Context.Set<T>().AsQueryable();
            }
        }

        public virtual IQueryable<T> All()
        {
            return ObjectSet.AsQueryable();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return ObjectSet.Where(expression);
        }

        public virtual void Add(T entity)
        {
            ObjectSet.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            ObjectSet.Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            UnitOfWork.Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public virtual T Find(params object[] keyValues)
        {
            return ObjectSet.Find(keyValues);
        }

        public virtual void Commit()
        {
            UnitOfWork.Commit();
        }
    }
}