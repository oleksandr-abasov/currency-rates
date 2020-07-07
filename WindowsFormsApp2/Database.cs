using System;
using System.Collections.Generic;
using WindowsFormsApp2.data;
using MySql.Data.MySqlClient;

namespace ConsoleApp1
{

    public class Database
    {
        private MySqlConnection _connection;

        public Database()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.UserID = "root";
            builder.Password = "strongpassword";
            builder.Database = "temp";
            builder.Port = 3306;

            String connString = builder.ToString(); // server=localhost;username=postgres итд

            Console.WriteLine(connString);

            _connection = new MySqlConnection(connString);
        }

        public MySqlConnection Connection => _connection;
    }
}