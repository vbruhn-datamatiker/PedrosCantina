using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Text;

namespace PedrosCantinaLib.Repository
{
    public class DBConnection
    {
        SqlConnection _connection;
        private string _connectionString;


        // properties
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        // constructors

        // config af 
        public DBConnection()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
                .Build();

            _connectionString = config["DBSecretString"];
        }

        public DBConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        // Open connection
        public void ConnectToDB()
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        // Close connection
        public void DisconnectFromDB()
        {
            _connection.Close();
        }

    }
}
