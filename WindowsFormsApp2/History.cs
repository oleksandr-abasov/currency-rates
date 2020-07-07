using System;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp2.data;
using ConsoleApp1;

namespace WindowsFormsApp2
{
    public partial class History : Form
    {
        private Form1 parent;
        
        private CurrencyRateRepository _currencyRateRepository;
        
        public History(Form1 parent, CurrencyRateRepository currencyRateRepository)
        {
            this.parent = parent;
            _currencyRateRepository = currencyRateRepository;
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            // load rates for selected currency by bank
            var bankOrg = parent.Source.Organizations.Find(o => o.Title.Value == parent.BankBox.SelectedItem);

            if (bankOrg == null) return;
            
            var selectedBank = OrganizationToBankConverter.OrganizationtoBank(bankOrg);

            // select * from rates where bank = parent.bankBox.SelectedItem;
            var currencyRates = _currencyRateRepository.GetAllByBankId(selectedBank)
                .Where(cv => cv.Code == (string) parent.CurrencyBox.SelectedItem)
                .OrderByDescending(cv => cv.DateTime)
                .ToList();
            
            // load this data into datagrid
            currencyRatesTable.Rows.Clear();
            foreach (var currencyRate in currencyRates)
            {
                var index = currencyRatesTable.Rows.Add();
                currencyRatesTable.Rows[index].Cells["Date"].Value = currencyRate.DateTime;
                currencyRatesTable.Rows[index].Cells["PurchaseRate"].Value = currencyRate.PurchaseRate;
                currencyRatesTable.Rows[index].Cells["SellingRate"].Value = currencyRate.SellingRate;   
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parent.Show();
            this.Hide();
        }

        public void UpdateData()
        {
            InitializeData();
        }

    }
}