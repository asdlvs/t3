using System.ComponentModel.DataAnnotations;

namespace Inmeta.Moving.DataAccess.Models
{
    public class Customer : HistoricalEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string PhoneNumber { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
