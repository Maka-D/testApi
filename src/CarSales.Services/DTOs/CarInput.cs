using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.DTOs
{
    public class CarInput
    {
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string CarNumber { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        [Required]
        [MinLength(17)]
        [MaxLength(17)]
        public string VinCode { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public int Price { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartedSale { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FinishedSale { get; set; }
    }
}
