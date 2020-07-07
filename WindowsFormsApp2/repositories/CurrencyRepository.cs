using System;
using System.Collections.Generic;
using ConsoleApp1;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp2.data
{
    public class CurrencyRepository
    {
        private Database _database;

        public CurrencyRepository(Database database)
        {
            _database = database;
        }
        
        public List<Currency> GetAll()
        {
            List<Currency> currencies = new List<Currency>();

            String query = "SELECT * FROM currencies";

            MySqlCommand cmd = new MySqlCommand(query, _database.Connection);

            _database.Connection.Open();

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                String code = reader["code"].ToString();
                String title = reader["title"].ToString();

                Currency currency = new Currency(code, title);

                currencies.Add(currency);
            }

            reader.Close();

            _database.Connection.Close();

            return currencies;
        }
        
        public Currency Save(Currency currency)
        {
            String query = string.Format("INSERT INTO currencies(code, title) VALUES ('{0}', '{1}')", 
                currency.Code, currency.Title);

            MySqlCommand cmd = new MySqlCommand(query, _database.Connection);

            _database.Connection.Open();

            cmd.ExecuteNonQuery();

            _database.Connection.Close();

            return currency;
        }

        public List<Currency> SaveAll(List<Currency> currencies)
        {
            List<Currency> result = new List<Currency>();
            foreach (var currency in currencies)
            {
                var saved = Save(currency);
                result.Add(saved);
            }

            return result;
        }
    }
}