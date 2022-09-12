using Inmeta.Moving.DataAccess;
using Inmeta.Moving.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Inmeta.Moving.Services
{
    public class ServicesService : IService<Service>
    {
        private readonly IOrdersDatabaseContext _ordersDatabase;

        public ServicesService(IOrdersDatabaseContext context)
        {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }

            _ordersDatabase = context;
        }

        public async Task<Service> CreateAsync(Service service)
        {
            var serviceToCreate = service.GetDatabaseModel();
            await _ordersDatabase.Services.AddAsync(serviceToCreate);
            await _ordersDatabase.SaveChangesAsync().ConfigureAwait(false);

            return new Service(serviceToCreate);
        }

        public async Task DeleteAsync(int id)
        {
            var serviceDbModel = new Service
            {
                Id = id,
            }.GetDatabaseModel();
            //_ordersDatabase.Services.Attach(serviceDbModel);
            _ordersDatabase.Services.Remove(serviceDbModel);
            await _ordersDatabase.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Service>> GetAsync()
        {
            var servicesDbModels = await _ordersDatabase.Services.ToListAsync().ConfigureAwait(false);
            return servicesDbModels.Select(dbModel => new Service(dbModel));
        }

        public async Task<Service?> GetByIdAsync(int id)
        {
            var servicesDbModel = await _ordersDatabase.Services.FirstOrDefaultAsync(s => s.Id == id).ConfigureAwait(false);
            return servicesDbModel != null ? new Service(servicesDbModel) : null;
        }

        public async Task<Service> UpdateAsync(int id, Service service)
        {
            service.Id = id;
            var servicesDbModel = service.GetDatabaseModel();
            _ordersDatabase.Services.Update(servicesDbModel);

            await _ordersDatabase.SaveChangesAsync().ConfigureAwait(false);
            return new Service(servicesDbModel);
        }
    }
}
