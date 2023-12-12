using Boo.Blog.Domain;
using Boo.Blog.ToolKits.Configurations;
using Boo.Blog.ToolKits.Extensions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace Boo.Blog.EntityFrameworkCore.Repositories
{
    public class RepositoryBase<TContext, TEntity> : EfCoreRepository<TContext, TEntity, long>, IRepositoryBase<TEntity>, IDapperRepository where TEntity : class, IEntity<long> where TContext : IEfCoreDbContext
    {
        public RepositoryBase(IDbContextProvider<TContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        [Obsolete("Use GetDbConnectionAsync method.")]
        public IDbConnection DbConnection => GetDbContextAsync().ConfigureAwait(false).GetAwaiter().GetResult().Database.GetDbConnection();

        public async Task<IDbConnection> GetDbConnectionAsync() => (await GetDbContextAsync()).Database.GetDbConnection();

        [Obsolete("Use GetDbTransactionAsync method.")]
        public IDbTransaction DbTransaction => GetDbContextAsync().Result.Database.CurrentTransaction?.GetDbTransaction();

        public async Task<IDbTransaction> GetDbTransactionAsync() => (await GetDbContextAsync()).Database.CurrentTransaction?.GetDbTransaction();

        public async Task<Tuple<IEnumerable<TEntity>, int>> GetPageListAsync(Expression<Func<TEntity, bool>> filter, Dictionary<string, bool> sort, int pageIndex, int pageSize, IUnitOfWork uow = null)
        {
            var _uow = uow ?? UnitOfWorkManager.Begin(new AbpUnitOfWorkOptions());
            try
            {
                var queryable = await GetQueryableAsync();// (await GetDbSetAsync()).AsQueryable();
                if (filter != null)
                {
                    queryable = queryable.Where(filter);
                }
                if (sort != null)
                {
                    foreach (var kv in sort)
                    {
                            queryable = queryable.SortBy(kv.Key,kv.Value);
                    }
                }
                var dataList = queryable.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                var dataCount = queryable.Count(filter);
                return new Tuple<IEnumerable<TEntity>, int>(dataList, dataCount);
            }
            finally
            {
                //外部的uow由外部释放
                if (uow == null)
                    _uow.Dispose();
            }
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="pageIndex">页码，从0开始</param>
        /// <param name="pageSize">每页最多行数</param>
        /// <param name="orderBy">例如：order by id desc,name asc</param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        public async Task<Tuple<IEnumerable<T>, int>> GetPageListAsync<T>(string sql, object param, int pageIndex, int pageSize, string orderBy = null, IDbTransaction dbTransaction = null)
        {

            if (dbTransaction == null)
            {
                dbTransaction = await GetDbTransactionAsync();
            }
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "";

            using (var conn = await GetDbConnectionAsync())
            {
                switch (AppSettings.EnableDb)
                {
                    default:
                    case DatabaseType.MYSQL:
                        sql = $" {sql} {orderBy}  limit  {pageIndex * pageSize},{pageSize} ;";
                        break;
                    case DatabaseType.MSSQL:
                        sql = $" {sql} {orderBy} offset {pageIndex * pageSize} row fetch next {pageSize} row only";
                        break;
                }
                var countSql = $"select count(1) from ({sql}) tb ;";
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                var data = conn.Query<T>(sql, param, dbTransaction);
                var dataCount = await conn.QueryFirstOrDefaultAsync<int>(countSql, param, dbTransaction);

                return new Tuple<IEnumerable<T>, int>(data, dataCount);
            }
        }
    }

    public class RepositoryBase<TEntity> : RepositoryBase<BlogDbContext, TEntity> where TEntity : class, IEntity<long>
    {
        public RepositoryBase(IDbContextProvider<BlogDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
