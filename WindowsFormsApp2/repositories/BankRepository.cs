using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp2.data
{
    public class BankRepository
    {
        private Database _database;

        public BankRepository(Database database)
        {
            _database = database;
        }

        public List<Bank> GetAll()
        {
            List<Bank> banks = new List<Bank>();

            String query = "SELECT * FROM banks";

            MySqlCommand cmd = new MySqlCommand(query, _database.Connection);

            _database.Connection.Open();

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                String id = reader["id"].ToString();
                String name = reader["name"].ToString();

                Bank bank = new Bank(id, name);

                banks.Add(bank);
            }

            reader.Close();

            _database.Connection.Close();

            return banks;
        }

        public Bank Save(Bank bank)
        {
            String query = string.Format("INSERT INTO banks(id, name) VALUES ('{0}', '{1}')",
                bank.Id, bank.Name);

            MySqlCommand cmd = new MySqlCommand(query, _database.Connection);

            _database.Connection.Open();

            cmd.ExecuteNonQuery();

            _database.Connection.Close();

            return bank;
        }

        public List<Bank> SaveAll(List<Bank> banks)
        {
            List<Bank> result = new List<Bank>();
            foreach (var bank in banks)
            {
                var saved = Save(bank);
                result.Add(saved);
            }

            return result;
        }
    }
}