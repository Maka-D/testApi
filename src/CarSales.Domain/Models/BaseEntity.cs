using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Domain.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }       
        public DateTime? DeletedAt { get; set; }
    }
}
