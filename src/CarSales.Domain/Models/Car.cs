using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Domain.Models
{
    public class Car : BaseEntity
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string CarNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string VinCode { get; set; }
        public int Price { get; set; }
        public DateTime StartedSale { get; set; }
        public DateTime FinishedSale { get; set; }
        public bool IsSold { get; set; }

    }
}
