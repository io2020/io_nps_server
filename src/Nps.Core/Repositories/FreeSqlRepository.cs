using FreeSql;
using Nps.Core.Entities;
using Nps.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nps.Core.Repositories
{
    /// <summary>
    /// 实现FreeSql ORM仓储
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class FreeSqlRepository<TEntity> : FreeSqlRepository<TEntity, long>, IFreeSqlRepository<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 构造函数，初始化一个<see cref="FreeSqlRepository<TEntity>"/>实例
        /// </summary>
        /// <param name="unitOfWorkManager">工作单元</param>
        /// <param name="currentUser">当前登录用户</param>
        public FreeSqlRepository(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser)
            : base(unitOfWorkManager, currentUser)
        {

        }
    }

    /// <summary>
    /// 实现FreeSql ORM仓储
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    public class FreeSqlRepository<TEntity, TKey> : DefaultRepository<TEntity, TKey>, IFreeSqlRepository<TEntity, TKey>
        where TEntity : class, new()
    {
        /// <summary>
        /// 当前登录用户
        /// </summary>
        protected readonly ICurrentUser _currentUser;

        /// <summary>
        /// 构造函数，初始化一个<see cref="FreeSqlRepository<TEntity, TKey>"/>实例
        /// </summary>
        /// <param name="unitOfWorkManager">工作单元</param>
        /// <param name="currentUser">当前登录用户</param>
        public FreeSqlRepository(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser)
            : base(unitOfWorkManager?.Orm, unitOfWorkManager)
        {
            _currentUser = currentUser;
        }

        #region Insert

        /*
         * 若实体继承了创建审计属性
         * 则在插入实体前，自动匹配创建审计属性值
         */
        private void BeforeInsert(TEntity entity)
        {
            if (entity is IHasCreateTime hct)
            {
                hct.CreateTime = DateTime.Now;
            }
            if (entity is ICreateAuditEntity cae)
            {
                if (cae.CreateUserId == 0 && _currentUser.UserId != null)
                {
                    cae.CreateUserId = _currentUser.UserId ?? 0;
                }
            }
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回实体对象</returns>
        public override TEntity Insert(TEntity entity)
        {
            BeforeInsert(entity);
            return base.Insert(entity);
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回实体对象</returns>
        public override Task<TEntity> InsertAsync(TEntity entity)
        {
            BeforeInsert(entity);
            return base.InsertAsync(entity);
        }

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>返回实体列表对象</returns>
        public override List<TEntity> Insert(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                BeforeInsert(entity);
            }
            return base.Insert(entities);
        }

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>返回实体列表对象</returns>
        public override Task<List<TEntity>> InsertAsync(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                BeforeInsert(entity);
            }
            return base.InsertAsync(entities);
        }

        #endregion

        #region Update

        /*
         * 若实体继承了更新审计属性
         * 则在更新实体前，自动匹配更新审计属性值
         */
        private void BeforeUpdate(TEntity entity)
        {
            if (entity is IHasUpdateTime hut)
            {
                hut.UpdateTime = DateTime.Now;
            }
            if (entity is IUpdateAuditEntity uae)
            {
                uae.UpdateUserId = _currentUser.UserId;
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回受影响的行数</returns>
        public override int Update(TEntity entity)
        {
            BeforeUpdate(entity);
            return base.Update(entity);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回受影响的行数</returns>
        public override Task<int> UpdateAsync(TEntity entity)
        {
            BeforeUpdate(entity);
            return base.UpdateAsync(entity);
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>返回受影响的行数</returns>
        public override int Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                BeforeUpdate(entity);
            }
            return base.Update(entities);
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>返回受影响的行数</returns>
        public override Task<int> UpdateAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                BeforeUpdate(entity);
            }
            return base.UpdateAsync(entities);
        }

        #endregion

        #region InsertOrUpdate

        /// <summary>
        /// 插入或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回实体对象</returns>
        public override TEntity InsertOrUpdate(TEntity entity)
        {
            BeforeInsert(entity);
            BeforeUpdate(entity);
            base.InsertOrUpdate(entity);
            return entity;
        }

        /// <summary>
        /// 插入或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回实体对象</returns>
        public override async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            BeforeInsert(entity);
            BeforeUpdate(entity);
            await base.InsertOrUpdateAsync(entity);
            return entity;
        }

        #endregion

        #region Delete

        /*
         * 若实体继承SoftDelete审计属性
         * 则将删除实体转换为更新实体
         */
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns>返回受影响的行数</returns>
        public override int Delete(TKey id)
        {
            TEntity entity = Get(id);
            if (entity is IDeleteAuditEntity)
            {
                return Orm.Update<TEntity>(entity)
                           .Set(a => (a as IDeleteAuditEntity).IsDeleted, true)
                           .Set(a => (a as IDeleteAuditEntity).DeleteUserId, _currentUser.UserId)
                           .Set(a => (a as IDeleteAuditEntity).DeleteTime, DateTime.Now)
                           .ExecuteAffrows();
            }
            return base.Delete(id);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回受影响的行数</returns>
        public override int Delete(TEntity entity)
        {
            if (entity is IDeleteAuditEntity)
            {
                return Orm.Update<TEntity>(entity)
                           .Set(a => (a as IDeleteAuditEntity).IsDeleted, true)
                           .Set(a => (a as IDeleteAuditEntity).DeleteUserId, _currentUser.UserId)
                           .Set(a => (a as IDeleteAuditEntity).DeleteTime, DateTime.Now)
                           .ExecuteAffrows();
            }
            return base.Delete(entity);
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>返回受影响的行数</returns>
        public override int Delete(IEnumerable<TEntity> entities)
        {
            if (entities.Any())
            {
                Attach(entities);
                foreach (TEntity entity in entities)
                {
                    if (entity is IDeleteAuditEntity softDelete)
                    {
                        softDelete.IsDeleted = true;
                        softDelete.DeleteUserId = _currentUser.UserId;
                        softDelete.DeleteTime = DateTime.Now;
                    }
                }
                return Update(entities);
            }
            return base.Delete(entities);
        }

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="predicate">实体删除条件</param>
        /// <returns>返回受影响的行数</returns>
        public override int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            if (typeof(IDeleteAuditEntity).IsAssignableFrom(typeof(TEntity)))
            {
                List<TEntity> items = Orm.Select<TEntity>().Where(predicate).ToList();
                if (items.Count == 0)
                {
                    return 0;
                }
                return Orm.Update<TEntity>(items)
                    .Set(a => (a as IDeleteAuditEntity).IsDeleted, true)
                    .Set(a => (a as IDeleteAuditEntity).DeleteUserId, _currentUser.UserId)
                    .Set(a => (a as IDeleteAuditEntity).DeleteTime, DateTime.Now)
                    .ExecuteAffrows();
            }
            return base.Delete(predicate);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns>返回受影响的行数</returns>
        public override async Task<int> DeleteAsync(TKey id)
        {
            TEntity entity = await GetAsync(id);
            if (entity is IDeleteAuditEntity)
            {
                return Orm.Update<TEntity>(entity)
                           .Set(a => (a as IDeleteAuditEntity).IsDeleted, true)
                           .Set(a => (a as IDeleteAuditEntity).DeleteUserId, _currentUser.UserId)
                           .Set(a => (a as IDeleteAuditEntity).DeleteTime, DateTime.Now)
                           .ExecuteAffrows();
            }
            return await base.DeleteAsync(id);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回受影响的行数</returns>
        public override async Task<int> DeleteAsync(TEntity entity)
        {
            if (entity is IDeleteAuditEntity)
            {
                return await Orm.Update<TEntity>(entity)
                    .Set(a => (a as IDeleteAuditEntity).IsDeleted, true)
                    .Set(a => (a as IDeleteAuditEntity).DeleteUserId, _currentUser.UserId)
                    .Set(a => (a as IDeleteAuditEntity).DeleteTime, DateTime.Now)
                    .ExecuteAffrowsAsync();
            }
            return base.Delete(entity);
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>返回受影响的行数</returns>
        public override async Task<int> DeleteAsync(IEnumerable<TEntity> entities)
        {
            if (entities.Any())
            {
                Attach(entities);
                foreach (TEntity entity in entities)
                {
                    if (entity is IDeleteAuditEntity softDelete)
                    {
                        softDelete.IsDeleted = true;
                        softDelete.DeleteUserId = _currentUser.UserId;
                        softDelete.DeleteTime = DateTime.Now;
                    }
                }
                return await UpdateAsync(entities);
            }
            return await base.DeleteAsync(entities);
        }

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="predicate">实体删除条件</param>
        /// <returns>返回受影响的行数</returns>
        public override async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (typeof(IDeleteAuditEntity).IsAssignableFrom(typeof(TEntity)))
            {
                List<TEntity> items = Orm.Select<TEntity>().Where(predicate).ToList();
                if (items.Count == 0)
                {
                    return 0;
                }
                return await Orm.Update<TEntity>(items)
                     .Set(a => (a as IDeleteAuditEntity).IsDeleted, true)
                     .Set(a => (a as IDeleteAuditEntity).DeleteUserId, _currentUser.UserId)
                     .Set(a => (a as IDeleteAuditEntity).DeleteTime, DateTime.Now)
                     .ExecuteAffrowsAsync();
            }
            return await base.DeleteAsync(predicate);
        }

        #endregion
    }
}