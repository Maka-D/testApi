using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.DTOs
{
    public class ClientInput
    {
        [Required]
        [RegularExpression("^[0-9]{11}$")]
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
