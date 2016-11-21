﻿#region

using Repository.Providers.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        private readonly IDataContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly Guid _instanceId;
        //private bool _includeDeleted;

        public Repository(IDataContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _instanceId = Guid.NewGuid();
        }

        public Guid InstanceId
        {
            get { return _instanceId; }
        }

        public virtual void Delete(TEntity entity)
        {
            ((IObjectState)entity).ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
        }

        public virtual void Delete(params object[] keyValues)
        {
            var ent = _dbSet.Find(keyValues);
            this.Delete(ent);
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public virtual UpdateEntity<TSource, TEntity> FindAndUpdateFromModel<TSource>(TSource source, params object[] keyValues)
        {
            var data = Find(keyValues);
            data.ObjectState = ObjectState.Modified;
            return new UpdateEntity<TSource, TEntity>(source, data, this);
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbSet.FindAsync(cancellationToken, keyValues);
        }

        public IDataContext GetContext()
        {
            return _context;
        }

        public virtual TEntity Insert(TEntity entity)
        {
            ((IObjectState)entity).ObjectState = ObjectState.Added;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
            return entity;
        }

        public virtual void InsertGraph(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void InsertGraphRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                Insert(entity);
        }

        public TEntity NewEntity(Action<UpdateEntity<string, TEntity>> config)
        {
            TEntity entity = Activator.CreateInstance<TEntity>();
            var ue = new UpdateEntity<string, TEntity>("", entity, this);
            config(ue);
            ue.Insert();
            return ue.GetEntity();
        }

        public UpdateEntity<string, TEntity> NewEntity()
        {
            TEntity entity = Activator.CreateInstance<TEntity>();
            var ue = new UpdateEntity<string, TEntity>("", entity, this);
            return ue;
        }

        public virtual UpdateEntity<TSource, TEntity> PrepareEntity<TSource>(TSource source)
        {
            TEntity entity = Activator.CreateInstance<TEntity>();
            return new UpdateEntity<TSource, TEntity>(source, entity, this);
        }

        public virtual IRepositoryQuery<TEntity> Query(bool includeDeleted = false)
        {
            var repositoryGetFluentHelper = new RepositoryQuery<TEntity>(this);

            if (includeDeleted == false)
                repositoryGetFluentHelper.Filter(x => !x.deleted);

            return repositoryGetFluentHelper;
        }

        //public virtual IRepositoryQuery<TEntity> Query(Expression<Func<TEntity, bool>> filter, bool includeDeleted = false)
        //{
        //    var repositoryGetFluentHelper = new RepositoryQuery<TEntity>(this);
        //    repositoryGetFluentHelper.Filter(filter);
        //    if (includeDeleted == false)
        //        repositoryGetFluentHelper.Filter(x => !x.deleted);
        //    return repositoryGetFluentHelper;
        //}

        public virtual IQueryable<TEntity> QueryGet(Expression<Func<TEntity, bool>> filter)
        {
            var repositoryGetFluentHelper = new RepositoryQuery<TEntity>(this);
            repositoryGetFluentHelper.Filter(filter);
            return repositoryGetFluentHelper.Get();
        }

        public virtual IQueryable<TEntity> QueryGet()
        {
            var repositoryGetFluentHelper = new RepositoryQuery<TEntity>(this);
            return repositoryGetFluentHelper.Get();
        }

        public virtual IRepositoryQuery<TEntity> QueryWithDeleted()
        {
            var repositoryGetFluentHelper = new RepositoryQuery<TEntity>(this);
            return repositoryGetFluentHelper;
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return this.Get().Select(selector);
        }

        public IQueryable<TEntity> Select()
        {
            return this.Get();
        }

        public virtual IQueryable<TEntity> SqlQuery(string query, params object[] parameters)
        {
            return _dbSet.SqlQuery(query, parameters).AsQueryable();
        }

        public virtual void Update(TEntity entity)
        {
            ((IObjectState)entity).ObjectState = ObjectState.Modified;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
        }

        public virtual UpdateEntity<TSource, TEntity> UpdateFromModel<TSource>(TEntity entity, TSource source)
        {
            return new UpdateEntity<TSource, TEntity>(source, entity, this);
        }

        internal IQueryable<TEntity> Get(
            List<Expression<Func<TEntity, bool>>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includeProperties != null)
                includeProperties.ForEach(i => query = query.Include(i));

            if (filter != null)
                filter.ForEach(f =>
                {
                    query = query.Where(f);
                });

            if (orderBy != null)
                query = orderBy(query);

            if (page != null && pageSize != null)
                query = query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

            return query;
        }

        internal async Task<IEnumerable<TEntity>> GetAsync(
            List<Expression<Func<TEntity, bool>>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            var result = await Task.Run(() =>
            {
                return Get(filter, orderBy, includeProperties, page, pageSize).AsEnumerable();
            });

            return result;
        }
    }
}