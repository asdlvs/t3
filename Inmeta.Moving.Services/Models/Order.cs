using OrderDbModel = Inmeta.Moving.DataAccess.Models.Order;
using OrderServiceDbModel = Inmeta.Moving.DataAccess.Models.OrderService;
using CustomerDbModel = Inmeta.Moving.DataAccess.Models.Customer;

namespace Inmeta.Moving.Services.Models
{
    public class Order
    {
        public Order()
        { }

        public Order(OrderDbModel dbModel)
        {
            Id = dbModel.Id;
            FromAddress = dbModel.FromAddress;
            ToAddress = dbModel.ToAddress;
            Comment = dbModel.Comment;
            CustomerId = dbModel.CustomerId;
            Services = (dbModel.Services?.Select(s => new OrderService
            {
                Id = s.ServiceId,
                IsDone = s.IsDone,
                PlannedDate = s.PlannedDateTime,
                Name = s.Service?.Name
            }) ?? Enumerable.Empty<OrderService>()).ToList();

            CustomerName = dbModel.Customer?.Name;
            PhoneNumber = dbModel.Customer?.PhoneNumber;
            Email = dbModel.Customer?.Email;
        }

        public int Id { get; set; }

        public string FromAddress { get; set; }

        public string ToAddress { get; set; }

        public string Comment { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public IEnumerable<OrderService> Services { get; set; }

        public OrderDbModel GetDatabaseModel()
        {
            var orderDb = new OrderDbModel
            {
                Comment = Comment,
                Id = Id,
                FromAddress = FromAddress,
                ToAddress = ToAddress,
                CustomerId = CustomerId,
            };

            var services = (Services?.Select(service => new OrderServiceDbModel
            {
                ServiceId = service.Id,
                IsDone = service.IsDone,
                Order = orderDb,
                PlannedDateTime = service.PlannedDate,
            }) ?? Enumerable.Empty<OrderServiceDbModel>()).ToList();

            orderDb.Services = services;
            if (CustomerId == 0)
            {
                orderDb.Customer = new CustomerDbModel
                {
                    Name = CustomerName,
                    PhoneNumber = PhoneNumber,
                    Email = Email,
                    Id = CustomerId,
                };
            }

            return orderDb;
        }
    }
}
