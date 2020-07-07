namespace WindowsFormsApp2.data
{
    public class Bank
    {
        private string id;
        private string name;

        public Bank(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public string Id => id;

        public string Name => name;

        protected bool Equals(Bank other)
        {
            return id == other.id && name == other.name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Bank) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((id != null ? id.GetHashCode() : 0) * 397) ^ (name != null ? name.GetHashCode() : 0);
            }
        }
    }
}