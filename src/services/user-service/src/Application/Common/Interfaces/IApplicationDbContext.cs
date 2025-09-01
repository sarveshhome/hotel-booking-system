using Microsoft.EntityFrameworkCore;
using User.Service.Domain.Entities;

namespace User.Service.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Domain.Entities.User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
