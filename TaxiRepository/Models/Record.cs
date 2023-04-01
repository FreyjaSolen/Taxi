using System;

namespace TaxiRepository.Models
{
    public class Record
    {
        public int id { get; set; }
        public int idUser { get; set; }
        public int idCar { get; set; }
        public string startStation { get; set; }
        public string endStation { get; set; }
        public double price { get; set; }
        public DateTime orderDate { get; set; }

        public Record()
        {
            id = -1;
            orderDate = DateTime.Now;
        }
        public Record(int idUser, int idCar, string startStation, string endStation, double price)
        {
            id = -1;
            this.idUser = idUser;
            this.idCar = idCar;
            this.startStation = startStation;
            this.endStation = endStation;
            this.price = price;
            orderDate = DateTime.Now;
        }
        public Record(int id, int idUser, int idCar, string startStation, string endStation, double price, DateTime orderDate)
        {
            this.id = id;
            this.idUser = idUser;
            this.idCar = idCar;
            this.startStation = startStation;
            this.endStation = endStation;
            this.price = price;
            this.orderDate = orderDate;
        }
    }
}
