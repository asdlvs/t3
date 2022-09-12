using CustomerDbModel = Inmeta.Moving.DataAccess.Models.Customer;

namespace Inmeta.Moving.Services.Models
{
    public class Customer
    {
        public Customer()
        { 
        }

        public Customer(CustomerDbModel dbModel)
        {
            Id = dbModel.Id;
            Name = dbModel.Name;
            PhoneNumber = dbModel.PhoneNumber;
            Email = dbModel.Email;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public CustomerDbModel GetDatabaseModel()
        {
            return new CustomerDbModel
            {
                Email = Email,
                Id = Id,
                Name = Name,
                PhoneNumber = PhoneNumber
            };
        }
    }
}
