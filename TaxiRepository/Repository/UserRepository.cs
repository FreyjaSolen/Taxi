using System;
using System.Configuration;
using System.Data.Common;

namespace TaxiRepository.Repository
{
    public class UserRepository
    {
        public string ConnectionName { get; set; }
        public UserRepository(string connectionName)
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

        public int getID(string nick, string password)
        {
            int result = -1;
            string query = $"SELECT id FROM Users WHERE UserName = '{nick}' AND Password = '{password}'";

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
                        result = Int32.Parse(row["id"].ToString());
                    }
                }
                Console.WriteLine("Result: {0}", result);
                reader.Close();
            }

            return result;
        }

        public bool IsManager(int Id)
        {
            bool result = false;
            string query = $"SELECT UserRole FROM Users WHERE id = '{Id}'";

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
                        string role = row["UserRole"].ToString();
                        if (role == "admin")
                        {
                            result = true;
                        }
                    }
                }
                Console.WriteLine("Result: {0}", result);
                reader.Close();
            }

            return result;
        }
    }
}
