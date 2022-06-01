using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Volo.Abp.Uow.EntityFrameworkCore;
using Boo.Blog.Domain.MultiTenant;
using Microsoft.Extensions.Configuration;
using Volo.Abp.DependencyInjection;

namespace Boo.Blog.EntityFrameworkCore
{

    public class BlogDbContextProvider : IDbContextProvider<BlogDbContext>//, IScopedDependency
    {
        public ILogger<BlogDbContextProvider> Logger { get; set; }
        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }
        private readonly IConfiguration _configurations;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ICancellationTokenProvider _cancellationTokenProvider;
        private readonly ITenant _currentTenant;
        private readonly IDatabaseConnectionFactory _dbConnFactory;
        private BlogDbContext _currentBlogDbContext;
        public BlogDbContextProvider(
            IConfiguration configurations,
            IUnitOfWorkManager unitOfWorkManager,
            ICancellationTokenProvider cancellationTokenProvider,
            ITenant currentTenant,
            IDatabaseConnectionFactory dbConnFactory)
        {
            _configurations = configurations;
            _unitOfWorkManager = unitOfWorkManager;
            _cancellationTokenProvider = cancellationTokenProvider;
            _currentTenant = currentTenant;
            _dbConnFactory = dbConnFactory;

            Logger = NullLogger<BlogDbContextProvider>.Instance;
        }

        public async Task<BlogDbContext> GetDbContextAsync()
        {
            var unitOfWork = _unitOfWorkManager.Current;
            if (unitOfWork == null)
            {
                throw new AbpException("A DbContext can only be created inside a unit of work!");
            }

            var targetDbContextType = typeof(BlogDbContext);
            var connectionStringName = ConnectionStringNameAttribute.GetConnStringName(targetDbContextType);
            var connectionString = _dbConnFactory.GetConnectionString(_currentTenant.Id);

            var dbContextKey = $"{targetDbContextType.FullName}_{connectionString}";

            var databaseApi = unitOfWork.FindDatabaseApi(dbContextKey);

            if (databaseApi == null)
            {
                databaseApi = new EfCoreDatabaseApi(
                    await CreateDbContextAsync(unitOfWork, connectionStringName, connectionString)
                );

                unitOfWork.AddDatabaseApi(dbContextKey, databaseApi);
            }

            return (BlogDbContext)((EfCoreDatabaseApi)databaseApi).DbContext;
        }

        private async Task<BlogDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork, string connectionStringName, string connectionString)
        {
            var creationContext = new DbContextCreationContext(connectionStringName, connectionString);
            using (DbContextCreationContext.Use(creationContext))
            {
                var dbContext = await CreateDbContextAsync(unitOfWork);

                if (dbContext is IAbpEfCoreDbContext abpEfCoreDbContext)
                {
                    abpEfCoreDbContext.Initialize(
                        new AbpEfCoreDbContextInitializationContext(
                            unitOfWork
                        )
                    );
                }

                return dbContext;
            }
        }

        private async Task<BlogDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork)
        {
            return unitOfWork.Options.IsTransactional
                ? await CreateDbContextWithTransactionAsync(unitOfWork)
                : await CreateDbContextAsync();
        }

        private async Task<BlogDbContext> CreateDbContextAsync()
        {
            if (_currentBlogDbContext != null)
                return _currentBlogDbContext;

            var connectionStr = _dbConnFactory.GetConnectionString(_currentTenant.Id);
            if (string.IsNullOrWhiteSpace(connectionStr))
                return _unitOfWorkManager.Current.ServiceProvider.GetRequiredService<BlogDbContext>();

            var dbOptionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();
            switch (_configurations.GetConnectionString("EnableDb"))
            {
                default:
                case "MSSQL":
                    dbOptionsBuilder = dbOptionsBuilder.UseSqlServer(connectionStr);
                    break;
                case "MYSQL":
                    dbOptionsBuilder = dbOptionsBuilder.UseMySql(connectionStr, ServerVersion.AutoDetect(connectionStr));
                    break;
            }
            _currentBlogDbContext = ActivatorUtilities.CreateInstance<BlogDbContext>(_unitOfWorkManager.Current.ServiceProvider, dbOptionsBuilder.Options);
            _currentBlogDbContext.LazyServiceProvider = LazyServiceProvider;
            return _currentBlogDbContext;
        }
        private async Task<BlogDbContext> CreateDbContextWithTransactionAsync(IUnitOfWork unitOfWork)
        {
            var transactionApiKey = $"EntityFrameworkCore_{DbContextCreationContext.Current.ConnectionString}";
            var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as EfCoreTransactionApi;

            if (activeTransaction == null)
            {
                var dbContext = unitOfWork.ServiceProvider.GetRequiredService<BlogDbContext>();

                var dbTransaction = unitOfWork.Options.IsolationLevel.HasValue
                    ? await dbContext.Database.BeginTransactionAsync(unitOfWork.Options.IsolationLevel.Value, GetCancellationToken())
                    : await dbContext.Database.BeginTransactionAsync(GetCancellationToken());

                unitOfWork.AddTransactionApi(
                    transactionApiKey,
                    new EfCoreTransactionApi(
                        dbTransaction,
                        dbContext,
                        _cancellationTokenProvider
                    )
                );

                return dbContext;
            }
            else
            {
                DbContextCreationContext.Current.ExistingConnection = activeTransaction.DbContextTransaction.GetDbTransaction().Connection;

                var dbContext = unitOfWork.ServiceProvider.GetRequiredService<BlogDbContext>();

                if (dbContext.As<DbContext>().Database.GetService<IDbContextTransactionManager>() is IRelationalTransactionManager)
                {
                    if (dbContext.Database.GetDbConnection() == DbContextCreationContext.Current.ExistingConnection)
                    {
                        await dbContext.Database.UseTransactionAsync(activeTransaction.DbContextTransaction.GetDbTransaction(), GetCancellationToken());
                    }
                    else
                    {
                        /* User did not re-use the ExistingConnection and we are starting a new transaction.
                         * EfCoreTransactionApi will check the connection string match and separately
                         * commit/rollback this transaction over the DbContext instance. */
                        if (unitOfWork.Options.IsolationLevel.HasValue)
                        {
                            await dbContext.Database.BeginTransactionAsync(
                                unitOfWork.Options.IsolationLevel.Value,
                                GetCancellationToken()
                            );
                        }
                        else
                        {
                            await dbContext.Database.BeginTransactionAsync(
                                GetCancellationToken()
                            );
                        }
                    }
                }
                else
                {
                    /* No need to store the returning IDbContextTransaction for non-relational databases
                     * since EfCoreTransactionApi will handle the commit/rollback over the DbContext instance.
                     */
                    await dbContext.Database.BeginTransactionAsync(GetCancellationToken());
                }

                activeTransaction.AttendedDbContexts.Add(dbContext);

                return dbContext;
            }
        }

        protected virtual CancellationToken GetCancellationToken(CancellationToken preferredValue = default)
        {
            return _cancellationTokenProvider.FallbackToProvider(preferredValue);
        }

        public BlogDbContext GetDbContext()
        {
            return GetDbContextAsync().Result;
        }
    }
}

