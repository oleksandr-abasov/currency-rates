using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.Serialization;
using WindowsFormsApp2.data;
using ConsoleApp1;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private Source source;
        private History history;
        private BankRepository _bankRepository;
        private CurrencyRepository _currencyRepository;
        private CurrencyRateRepository _currencyRateRepository;
        
        public Form1(BankRepository bankRepository, CurrencyRepository currencyRepository, CurrencyRateRepository currencyRateRepository)
        {
            _bankRepository = bankRepository;
            _currencyRepository = currencyRepository;
            _currencyRateRepository = currencyRateRepository;
            
            InitializeComponent();
            InitializeData();
            
            history = new History(this, currencyRateRepository);
        }

        private void InitializeData()
        {
            string link = "http://resources.finance.ua/ru/public/currency-cash.xml";
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(link);
            request.Method = "GET";
            request.KeepAlive = true;
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();

            if (response == null) return;
            
            logger.Info("Response received: {0}", response.CharacterSet);
            StreamReader responseStream = new StreamReader(response.GetResponseStream());
            XmlSerializer serializer = new XmlSerializer(typeof(Source));
            
            // parse xml from response
            source = (Source) serializer.Deserialize(responseStream);
            
            logger.Info("Parsing successfuly completed");

            // set currencies in currency box
            // currencyBox.Items.AddRange(source.Currencies.Select(i => i.Code).ToArray());
            bankBox.Items.AddRange(source.Organizations.Select(i => i.Title.Value).ToArray());

            SaveNewBanks();
            SaveNewCurrencies();
            SaveNewCurrencyRates();
        }

        private void SaveNewBanks()
        {
            var banks = _bankRepository.GetAll();
            var receivedBanks = source.Organizations.Select(o => new Bank(o.ID, o.Title.Value)).ToList();
            receivedBanks.RemoveAll(b => banks.Contains(b));

            if (receivedBanks.Count > 0)
            {
                _bankRepository.SaveAll(receivedBanks);
            }
        }

        private void SaveNewCurrencies()
        {
            var currencies = _currencyRepository.GetAll();
            var receivedCurrencies = source.Currencies.Select(c => new Currency(c.Code, c.Title)).ToList();
            receivedCurrencies.RemoveAll(b => currencies.Contains(b));

            if (receivedCurrencies.Count > 0)
            {
                _currencyRepository.SaveAll(receivedCurrencies);
            }
        }

        private void SaveNewCurrencyRates()
        {
            foreach (var organization in source.Organizations)
            {
                var bank = OrganizationToBankConverter.OrganizationtoBank(organization);
                var currencyRates = _currencyRateRepository.GetAllByBankId(bank);
                var newCurrencyRates = organization.CurrencyValues
                    .Select(cv => new CurrencyRate(cv.Code, bank.Id, cv.Buy, cv.Sell, DateTime.Parse(source.Date)))
                    .ToList();
                
                newCurrencyRates.RemoveAll(cr => currencyRates.Contains(cr));

                if (newCurrencyRates.Count > 0)
                {
                    _currencyRateRepository.SaveAll(newCurrencyRates);
                }
            }
        }

        private void bankBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var organisation = source.Organizations.First(b => b.Title.Value == (string) bankBox.SelectedItem);
            currencyBox.Items.AddRange(organisation.CurrencyValues.Select(cv => cv.Code).ToArray());
            UpdateRates();
        }

        private void currencyBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRates();
        }
        
        private void UpdateRates()
        {
            if (bankBox.SelectedItem == null || currencyBox.SelectedItem == null) return;
            
            var organisation = source.Organizations.First(b => b.Title.Value == (string) bankBox.SelectedItem);
            var currencyValue = organisation.CurrencyValues.Find(c => c.Code == (string) currencyBox.SelectedItem);
            
            purchaseTextBox.Text = string.Format("{0:N2}", currencyValue.Buy);
            sellingTextBox.Text = string.Format("{0:N2}", currencyValue.Sell);
        }

        private void currencyBox_Click(object sender, EventArgs e)
        {
            if (bankBox.SelectedItem == null)
            {
                MessageBox.Show("Please select the bank first", 
                    "Error Information", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (bankBox.SelectedItem == null)
            {
                MessageBox.Show("Please select the bank first", 
                    "Error Information", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }
            if (currencyBox.SelectedItem == null)
            {
                MessageBox.Show("Please select the currency first", 
                    "Error Information", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }
            
            history.Show();
            history.UpdateData();
            this.Hide();
        }

        public ComboBox BankBox => bankBox;

        public ComboBox CurrencyBox => currencyBox;

        public Source Source => source;
    }
}
