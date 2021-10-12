using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Domain.Models
{
    public class Client : BaseEntity
    {
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        [Required]
        [RegularExpression("^[0 - 9] +$")]
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime? BirthDate { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Address { get; set; }


    }
}
