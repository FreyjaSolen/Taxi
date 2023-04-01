using System;

namespace TaxiRepository.Models
{
    public class CarRecord
    {
        public int id { get; set; }
        public string Car { get; set; }
        public string Driver { get; set; }
        public bool IsEnable { get; set; }

        public CarRecord() { }
        public CarRecord(int id, string Car, string Driver, bool IsEnable)
        {
            this.id = id;
            this.Car = Car;
            this.Driver = Driver;
            this.IsEnable = IsEnable;
        }
        public override string ToString()
        {
            return $"{id} {Car} {Driver} {IsEnable}";
        }
    }
}
