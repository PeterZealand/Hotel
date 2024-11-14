using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel
{
    internal class Facility
    {
        public int Facility_ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"ID: {Facility_ID}, Name: {Name}";
        }
    }
}
