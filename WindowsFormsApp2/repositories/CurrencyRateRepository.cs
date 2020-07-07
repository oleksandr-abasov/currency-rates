using System;
using System.Collections.Generic;
using ConsoleApp1;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp2.data
{
    public class CurrencyRateRepository
    {
        private Database _database;

        public CurrencyRateRepository(Database database)
        {
            _database = database;
        }
        
        public List<CurrencyRate> GetAllByBankId(Bank bank)
        {
            List<CurrencyRate> currencyRates = new List<CurrencyRate>();

            String query = string.Format("SELECT * FROM currency_rates WHERE bank_id = '{0}'", bank.Id);

            MySqlCommand cmd = new MySqlCommand(query, _database.Connection);

            _database.Connection.Open();

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                String code = reader["code"].ToString();
                String bankId = reader["bank_id"].ToString();
                DateTime dateTime = DateTime.Parse(reader["date_time"].ToString());
                Double purchaseRate = Double.Parse(reader["purchase_rate"].ToString());
                Double sellingRate = Double.Parse(reader["selling_rate"].ToString());

                CurrencyRate currencyRate = new CurrencyRate(code, bankId, purchaseRate, sellingRate, dateTime);

                currencyRates.Add(currencyRate);
            }

            reader.Close();

            _database.Connection.Close();

            return currencyRates;
        }
        
        public CurrencyRate Save(CurrencyRate currencyRate)
        {
            String query = string.Format("INSERT INTO currency_rates(code, bank_id, purchase_rate, selling_rate, date_time)" + 
                                         " VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", 
                currencyRate.Code, currencyRate.BankId, currencyRate.PurchaseRate, 
                currencyRate.SellingRate, currencyRate.DateTime.ToString("yyyy-MM-dd H:mm:ss"));

            MySqlCommand cmd = new MySqlCommand(query, _database.Connection);

            _database.Connection.Open();

            cmd.ExecuteNonQuery();

            _database.Connection.Close();

            return currencyRate;
        }
        
        public List<CurrencyRate> SaveAll(List<CurrencyRate> currencyRates)
        {
            List<CurrencyRate> result = new List<CurrencyRate>();
            foreach (var currencyRate in currencyRates)
            {
                var saved = Save(currencyRate);
                result.Add(saved);
            }

            return result;
        }
    }
}