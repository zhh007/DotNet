using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Data
{
    public abstract class RepositoryBase<T> where T : class
    {
        private DbContext dataContext;
        private readonly IDbSet<T> DbSet;
        public RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            DbSet = DataContext.Set<T>();
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected DbContext DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }

        public virtual void Add(T entity)
        {
            DbSet.Add(entity);
        }
        public virtual void Update(T entity)
        {
            DbSet.Attach(entity);
            DataContext.Entry(entity).State = EntityState.Modified;//System.Data.EntityState.Modified
        }
        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = DbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                DbSet.Remove(obj);
        }
        public virtual IQueryable<T> GetAll()
        {
            return DbSet.AsQueryable();
        }
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where).AsEnumerable<T>();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where).FirstOrDefault<T>();
        }

        public virtual int Count(Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where).Count();
        }

        public virtual T GetNoTracking(Expression<Func<T, bool>> where)
        {
            return DbSet.AsNoTracking().Where(where).FirstOrDefault<T>();
        }

        public virtual IQueryable<T> GetAllNoTracking()
        {
            return DbSet.AsNoTracking().AsQueryable();
        }

        public virtual IEnumerable<T> GetManyNoTracking(Expression<Func<T, bool>> where)
        {
            return DbSet.AsNoTracking().Where(where).AsEnumerable<T>();
        }
    }
}
