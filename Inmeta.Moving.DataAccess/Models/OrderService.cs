using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmeta.Moving.DataAccess.Models
{
    [PrimaryKey("OrderId", "ServiceId")]
    public class OrderService
    {
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        public Order Order { get; set; }

        [ForeignKey("Service")]
        public int ServiceId { get; set; }

        public Service Service { get; set; }

        public bool IsDone { get; set; }

        public DateTime PlannedDateTime { get; set; }
    }
}
