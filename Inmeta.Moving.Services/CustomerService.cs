using Inmeta.Moving.Services.Models;
using Inmeta.Moving.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Web.Http;

namespace Inmeta.Moving.Services
{
    public class CustomerService : IService<Customer>, ISearchingService<Customer>
    {
        private readonly IOrdersDatabaseContext _ordersDatabase;

        public CustomerService(IOrdersDatabaseContext ordersDatabase)
        {
            if (ordersDatabase == null) {  throw new ArgumentNullException(nameof(ordersDatabase)); }

            _ordersDatabase = ordersDatabase;
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            if (customer == null) { throw new ArgumentNullException(nameof(customer)); }

            var customerToCreate = customer.GetDatabaseModel();
            await _ordersDatabase.Customers
                .AddAsync(customerToCreate)
                .ConfigureAwait(false);
            await _ordersDatabase
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return new Customer(customerToCreate);
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            var customersDbModel = await _ordersDatabase.Customers.FirstOrDefaultAsync(c => c.Id == id).ConfigureAwait(false);
            return customersDbModel != null ? new Customer(customersDbModel) : null;
        }

        public async Task<IEnumerable<Customer>> GetAsync()
        {
            var customerDbModels = await _ordersDatabase.Customers.ToListAsync().ConfigureAwait(false);
            return customerDbModels.Select(dbCustomer => new Customer(dbCustomer));
        }

        public async Task<Customer> UpdateAsync(int id, Customer customer)
        {
            customer.Id = id;
            var customersDbModel = customer.GetDatabaseModel();
            _ordersDatabase.Customers.Update(customersDbModel);

            await _ordersDatabase.SaveChangesAsync().ConfigureAwait(false);
            return new Customer(customersDbModel);
        }

        public async Task DeleteAsync(int id)
        {
            var serviceDbModel = new Customer
            {
                Id = id,
            }.GetDatabaseModel();
            //_ordersDatabase.Services.Attach(serviceDbModel);
            _ordersDatabase.Customers.Remove(serviceDbModel);
            await _ordersDatabase.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Customer>> FindByText(string text)
        {
            var customersDbModels = await _ordersDatabase.Customers
                .Where(c => c.Name.Contains(text))
                .ToListAsync()
                .ConfigureAwait(false);

            return customersDbModels.Select(c => new Customer(c));
        }
    }
}
