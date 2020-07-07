using System;

namespace WindowsFormsApp2.data
{
    public class CurrencyRate
    {
        private string code;
        private string bankId;
        private double purchaseRate;
        private double sellingRate;
        private DateTime dateTime;

        public CurrencyRate(string code, string bankId, double purchaseRate, double sellingRate, DateTime dateTime)
        {
            this.code = code;
            this.bankId = bankId;
            this.purchaseRate = purchaseRate;
            this.sellingRate = sellingRate;
            this.dateTime = dateTime;
        }

        public string Code
        {
            get => code;
            set => code = value;
        }

        public double PurchaseRate
        {
            get => purchaseRate;
            set => purchaseRate = value;
        }

        public double SellingRate
        {
            get => sellingRate;
            set => sellingRate = value;
        }

        public DateTime DateTime
        {
            get => dateTime;
            set => dateTime = value;
        }

        public string BankId
        {
            get => bankId;
            set => bankId = value;
        }

        protected bool Equals(CurrencyRate other)
        {
            return code == other.code 
                   && bankId == other.bankId 
                   && purchaseRate.Equals(other.purchaseRate) 
                   && sellingRate.Equals(other.sellingRate) 
                   && dateTime.Equals(other.dateTime);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CurrencyRate) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (code != null ? code.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (bankId != null ? bankId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ purchaseRate.GetHashCode();
                hashCode = (hashCode * 397) ^ sellingRate.GetHashCode();
                hashCode = (hashCode * 397) ^ dateTime.GetHashCode();
                return hashCode;
            }
        }
    }
}