using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.CarService.DTOs
{
    class CarInput
    {
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string CarNumber { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string VinCode { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public DateTime StartedSale { get; set; }
        [Required]
        public DateTime FinishedSale { get; set; }
    }
}
