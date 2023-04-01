using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using TaxiRepository.Models;

namespace TaxiRepository.Repository
{
    public class CarsRepository
    {
        public string ConnectionName { get; set; }
        public CarsRepository(string connectionName)
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

        public void AddCarRecord(CarRecord car)
        {
            string query = $"INSERT INTO Cars (Car, Driver, IsEnable) VALUES('{car.Car}', '{car.Driver}', '{car.IsEnable}'); SELECT SCOPE_IDENTITY();";
            using (var connection = GetDbConnection())
            {
                try
                {
                    connection.Open();
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = query;
                    cmd.Connection = connection;
                    int number = cmd.ExecuteNonQuery();
                    Console.WriteLine(new string('_', 100));
                    Console.WriteLine("Add success: {0}", number);                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);                   
                }
            }
        }

        public void SetCarEnabled(int id, bool isEnabled)
        {
            string query = $"UPDATE Cars SET IsEnable = '{isEnabled}' WHERE id = '{id}'";
            using (var connection = GetDbConnection())
            {
                try
                {
                    connection.Open();
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = query;
                    cmd.Connection = connection;
                    int number = cmd.ExecuteNonQuery();
                    Console.WriteLine(new string('_', 100));
                    Console.WriteLine("Update success: {0}", number);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public List<CarRecord> getCars()
        {
            List<CarRecord> carRecords = null;
            string query = $"SELECT * FROM Cars";

            using (var connection = GetDbConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;

                var reader = cmd.ExecuteReader();
                carRecords = new List<CarRecord>();
                if (reader.HasRows)
                {
                    foreach (DbDataRecord row in reader)
                    {
                        CarRecord cr = new CarRecord()
                        {
                            id = (Int32.Parse(row["id"].ToString())),
                            Car = row["Car"].ToString(),
                            Driver = row["Driver"].ToString(),
                            IsEnable = Boolean.Parse(row["IsEnable"].ToString())
                        };                       
                        carRecords.Add(cr);
                    }
                }
                reader.Close();
            }
            return carRecords;
        }

        public List<CarRecord> getEnabledCars()
        {
            List<CarRecord> carRecords = null;
            string query = $"SELECT * FROM Cars WHERE IsEnable = 'True'";

            using (var connection = GetDbConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;

                var reader = cmd.ExecuteReader();
                carRecords = new List<CarRecord>();
                if (reader.HasRows)
                {
                    foreach (DbDataRecord row in reader)
                    {
                        CarRecord cr = new CarRecord()
                        {
                            id = (Int32.Parse(row["id"].ToString())),
                            Car = row["Car"].ToString(),
                            Driver = row["Driver"].ToString(),
                            IsEnable = Boolean.Parse(row["IsEnable"].ToString())
                        };
                        carRecords.Add(cr);
                    }
                }
                reader.Close();
            }
            return carRecords;
        }
    }
}
