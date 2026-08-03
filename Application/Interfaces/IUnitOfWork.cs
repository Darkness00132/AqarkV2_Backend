
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Interfaces
{
    public interface IUnitOfWork
    {
        public Task SaveChangesAsync();

        public Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
