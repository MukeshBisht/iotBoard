
using System;
using MySql.Data.MySqlClient;

namespace IOT.DB {

    public class AppDB : IDisposable
    {
        public MySqlConnection Connection;

        public AppDB(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}