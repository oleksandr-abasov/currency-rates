using System.Collections.Generic;
using System.Xml.Serialization;

namespace ConsoleApp1
{
    [XmlRoot(ElementName = "source")]
    public class Source
    {
        [XmlAttribute("id")]
        public string ID { get; set; }

        [XmlAttribute("date")]
        public string Date { get; set; }

        [XmlArray(ElementName = "organizations")]
        [XmlArrayItem("organization")]
        public List<Organisation> Organizations { get; set; }
        
        [XmlArray(ElementName = "currencies")]
        [XmlArrayItem("c")]
        public List<Currency> Currencies { get; set; }
    }

    public class Element<T>
    {
        [XmlAttribute("value")]
        public T Value { get; set; }
    }
    
    public class Organisation
    {
        [XmlAttribute("id")]
        public string ID { get; set; }

        [XmlElement("title")]
        public Element<string> Title { get; set; }
        
        [XmlElement("address")]
        public Element<string> Address { get; set; }
        
        [XmlArray("currencies")]
        [XmlArrayItem("c")]
        public List<CurrencyValue> CurrencyValues { get; set; }
    }
    
    // [XmlRoot(ElementName = "c")]
    public class CurrencyValue
    {
        [XmlAttribute("id")] 
        public string Code { get; set; }
        
        [XmlAttribute("br")]
        public double Buy { get; set; }
        
        [XmlAttribute("ar")]
        public double Sell { get; set; }

        public CurrencyValue() { }

        public CurrencyValue(string code, double buy, double sell)
        {
            this.Code = code;
            this.Buy = buy;
            this.Sell = sell;
        }

        public override string ToString()
        {
            return $"{nameof(Code)}: {Code}, {nameof(Buy)}: {Buy}, {nameof(Sell)}: {Sell}";
        }
    }

    public class Currency
    {
        private string code;
        
        private string title;

        public Currency() { }

        public Currency(string code, string title)
        {
            this.code = code;
            this.title = title;
        }

        [XmlAttribute("id")] 
        public string Code
        {
            get => code;
            set => code = value;
        }

        [XmlAttribute("title")] 
        public string Title
        {
            get => title;
            set => title = value;
        }

        public override string ToString()
        {
            return $"{nameof(code)}: {code}, {nameof(title)}: {title}";
        }

        protected bool Equals(Currency other)
        {
            return code == other.code && title == other.title;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Currency) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((code != null ? code.GetHashCode() : 0) * 397) ^ (title != null ? title.GetHashCode() : 0);
            }
        }
    }
}