using System.ComponentModel.DataAnnotations;

namespace Inmeta.Moving.DataAccess.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        public ICollection<OrderService> OrderServices { get; set; }
    }
}
