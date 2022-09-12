using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmeta.Moving.DataAccess.Models
{
    public class Order : HistoricalEntity
    {
        [Key]
        public int Id { get; set; }

        public Customer Customer { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [MaxLength(200)]
        public string FromAddress { get; set; }

        [MaxLength(200)]
        public string ToAddress { get; set; }

        public ICollection<OrderService> Services { get; set; }

        [MaxLength(2000)]
        public string Comment { get; set; }

        public bool IsDeleted { get; set; }
    }
}
