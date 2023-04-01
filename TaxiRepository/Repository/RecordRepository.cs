using System;
using System.Configuration;
using System.Data.Common;
using System.Windows.Forms;
using TaxiRepository.Models;

namespace TaxiRepository.Repository
{
    public class RecordRepository
    {
        public string ConnectionName { get; set; }
        public RecordRepository(string connectionName)
        {
            ConnectionName = connectionName;
        }
        private DbConnection GetDbConnection()
        {
            var settings = ConfigurationManager.ConnectionStrings[ConnectionName];
            var factory = DbProviderFactories.GetFactory(settings.ProviderName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = settings.ConnectionString;

            return connection;
        }

        public int setRecord(Record record)
        {
            string query = $"INSERT INTO TaxiJournal (idUser, idCar, startStation, endStation, price, orderDate) VALUES('{record.idUser}', '{record.idCar}', '{record.startStation}', '{record.endStation}', '{record.price}', '{record.orderDate.ToString("yyyy.MM.dd HH:mm:ss")}');";
            int number = -1;
            using (var connection = GetDbConnection())
            {
                try
                {
                    connection.Open();
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = query;
                    cmd.Connection = connection;
                    number = cmd.ExecuteNonQuery();
                    Console.WriteLine("Add success: {0}", number);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Console.WriteLine(ex.Message);
                }
            }
            return number;
        }

        public double getSumOrders(int ID)
        {
            double sum = 0;

            string query = $"SELECT SUM(price) AS SumOrder FROM TaxiJournal WHERE idUser = '{ID}'";

            using (var connection = GetDbConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    foreach (DbDataRecord row in reader)
                    {
                        sum = Double.Parse(row["SumOrder"].ToString());

                    }
                }
                reader.Close();
            }

            return sum;
        }
    }
}
