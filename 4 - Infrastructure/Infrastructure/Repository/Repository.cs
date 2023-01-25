using Microsoft.EntityFrameworkCore;
using NewTigerBox.Domain.Model;
using NewTigerBox.Domain.Repositories;
using NewTigerBox.Infrastructure.Repository.Context;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NewTigerBox.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        protected NewTigerBoxContext Context { get; private set; }
        protected DbSet<T> DbSet { get; private set; }

        protected AsyncRetryPolicy Retry { get; private set; }

        public Repository(NewTigerBoxContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
            Retry = Policy.Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(3, retryAttempt)));
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entityDb = DbSet.FindAsync(id, cancellationToken);

            Context.Remove(entityDb);

            await Retry.ExecuteAsync(async() => await Context.SaveChangesAsync(cancellationToken));
        }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Retry.ExecuteAsync(async () => await DbSet.FindAsync(id, cancellationToken));
        }

        public async Task<List<T>> ListAsync(CancellationToken cancellationToken)
        {
            return await Retry.ExecuteAsync(async () => await DbSet.ToListAsync(cancellationToken));
        }

        public async Task<T> SaveAsync(T data, CancellationToken cancellationToken)
        {
            if (IsNewEntity(data))
            {
                return await InsertAsync(data, cancellationToken);
            }
            else
            {
                return await UpdateAsync(data, cancellationToken);
            }
        }

        private async Task<T> InsertAsync(T data, CancellationToken cancellationToken)
        {
            data.Id = Guid.NewGuid();

            await DbSet.AddAsync(data, cancellationToken);

            await Retry.ExecuteAsync(async () => await Context.SaveChangesAsync(cancellationToken));

            return data;
        }

        private async Task<T> UpdateAsync(T data, CancellationToken cancellationToken)
        {
            var entityDb = await DbSet.FindAsync(data.Id, cancellationToken);
            data.Id = entityDb.Id;

            data.UpdatedAt = DateTime.Now;
            data.CreatedAt = entityDb.CreatedAt;

            Context.Entry(entityDb).Property(x => x.Id).IsModified = false;
            Context.Entry(entityDb).CurrentValues.SetValues(data);
            await Retry.ExecuteAsync(async () => await Context.SaveChangesAsync(cancellationToken));

            return data;
        }

        private bool IsNewEntity(T data)
        {
            return data.Id.Equals(Guid.Empty);
        }
    }
}
