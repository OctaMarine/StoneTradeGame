using Microsoft.EntityFrameworkCore.Storage;

namespace StoneActionServer.DAL;

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task SaveChangesAsync();
}