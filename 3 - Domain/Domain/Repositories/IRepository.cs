using NewTigerBox.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NewTigerBox.Domain.Repositories
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> SaveAsync(T data, CancellationToken cancellationToken);

        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<List<T>> ListAsync(CancellationToken cancellationToken);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
