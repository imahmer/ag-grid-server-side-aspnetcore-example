using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAngular.Models
{
    public class OlympicWinner
    {
        public int Id { get; set; }
        public string Athlete { get; set; }
        public byte Age { get; set; }
        public string Country { get; set; }
        public int Year { get; set; }
        public string Date { get; set; }
        public string Sport { get; set; }
        public byte Gold { get; set; }
        public byte Silver { get; set; }
        public byte Bronze { get; set; }
        public byte Total { get; set; }
    }
}
