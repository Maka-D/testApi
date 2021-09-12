using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Domain.Models.ReportModel
{
    public class ReportData
    {
        public int Month { get; set; }
        public int CarsAmount { get; set; }
        public int CarsPrice { get; set; }
        public double AveragePrice { get; set; }
    }
}
