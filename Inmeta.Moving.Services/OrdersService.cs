using Microsoft.EntityFrameworkCore;

using Inmeta.Moving.DataAccess;
using Inmeta.Moving.Services.Models;

namespace Inmeta.Moving.Services
{
    public class OrdersService : IService<Order>, ISearchingService<Order>
    {
        private readonly IOrdersDatabaseContext _ordersDatabase;

        public OrdersService(IOrdersDatabaseContext context)
        {
            if (context == null) { throw new ArgumentNullException("context"); }

            _ordersDatabase = context;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            if (order == null) { throw new ArgumentNullException(nameof(order)); }

            if (order.CustomerId == 0)
            {

            }

            var orderToCreate = order.GetDatabaseModel();
            await _ordersDatabase.Orders
                .AddAsync(orderToCreate)
                .ConfigureAwait(false);
            await _ordersDatabase.SaveChangesAsync().ConfigureAwait(false);
            return new Order(orderToCreate);
        }

        public async Task DeleteAsync(int id)
        {
            var orderToRemove = await _ordersDatabase
                .Orders
                .FirstOrDefaultAsync(o => o.Id == id)
                .ConfigureAwait(false);

            if (orderToRemove == null) { return; }

            orderToRemove.IsDeleted = true;

            //var orderDbModel = new Order
            //{
            //    Id = id
            //}.GetDatabaseModel();
            ////_ordersDatabase.Orders.Attach(orderDbModel);
            //_ordersDatabase.Attach(orderDbModel);
            //orderDbModel.IsDeleted = true;
            await _ordersDatabase.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            var orderDbModel = await _ordersDatabase.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .Include(o => o.Services)
                .ThenInclude(s => s.Service)
                .FirstOrDefaultAsync(o => o.Id == id)
                .ConfigureAwait(false);

            return orderDbModel != null ? new Order(orderDbModel) : null;
        }

        public async Task<IEnumerable<Order>> GetAsync()
        {
            var ordersDbModels = await _ordersDatabase.Orders
                .Where(o => !o.IsDeleted)
                .Include(o => o.Customer)
                .Include(o => o.Services)
                .ThenInclude(s => s.Service)
                .ToListAsync()
                .ConfigureAwait(false);
            return ordersDbModels.Select(order => new Order(order));

        }

        public async Task<Order> UpdateAsync(int id, Order order)
        {
            order.Id = id;
            var orderServices = _ordersDatabase.OrderServices.Where(os => os.OrderId == order.Id);
            _ordersDatabase.OrderServices.RemoveRange(orderServices);
            var orderDbModel = order.GetDatabaseModel();
            _ordersDatabase.Orders.Update(orderDbModel);
            _ordersDatabase.Entry(orderDbModel).Reference("Customer").Load();

            await _ordersDatabase.SaveChangesAsync().ConfigureAwait(false);
            return new Order(orderDbModel);
        }

        public async Task<IEnumerable<Order>> FindByText(string text)
        {
            var orderDbModels = await _ordersDatabase.Orders
                .Include(o => o.Customer)
                .Where(o => !o.IsDeleted)
                .Where(o => o.FromAddress.Contains(text) || o.ToAddress.Contains(text) || o.Customer.Name.Contains(text))
                .ToListAsync();

            return orderDbModels.Select(o => new Order(o));
        }
    }
}
