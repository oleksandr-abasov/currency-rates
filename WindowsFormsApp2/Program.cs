using System;
using System.Windows.Forms;
using WindowsFormsApp2.data;
using ConsoleApp1;

namespace WindowsFormsApp2
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // List<User> users = Database.GetUsers();
            // foreach (var user in users)
            // {
            //     Console.WriteLine(user);
            // }

            var database = new Database();
            var bankRepository = new BankRepository(database);
            var currencyRepository = new CurrencyRepository(database);
            var currencyRateRepository = new CurrencyRateRepository(database);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(bankRepository, currencyRepository, currencyRateRepository));
        }
    }
}
