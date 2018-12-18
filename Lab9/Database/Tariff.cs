using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab9.Models
{
    public class Tariff
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Operator { get; set; }
        public int Payroll { get; set; }
        public int InnerCallsMinutes { get; set; }
        public int OuterCallsMinutes { get; set; }
        public int InternetMB { get; set; }
        public int SMSCount { get; set; }
    }
}