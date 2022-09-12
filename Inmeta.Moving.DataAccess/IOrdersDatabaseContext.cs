using Inmeta.Moving.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Inmeta.Moving.DataAccess
{
    public interface IOrdersDatabaseContext
    {
        DbSet<Order> Orders { get; }

        DbSet<Customer> Customers { get; }

        DbSet<Service> Services { get; }

        DbSet<User> Users { get; }

        DbSet<OrderService> OrderServices { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        EntityEntry Attach(object entity);

        EntityEntry Entry(object entity);
    }
}
