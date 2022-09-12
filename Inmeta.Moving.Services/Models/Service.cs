using ServiceDbModel = Inmeta.Moving.DataAccess.Models.Service;

namespace Inmeta.Moving.Services.Models
{
    public class Service
    {
        public Service()
        { }

        public Service(ServiceDbModel dbService)
        {
            Id = dbService.Id;
            Name = dbService.Name;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ServiceDbModel GetDatabaseModel()
        {
            return new ServiceDbModel
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
