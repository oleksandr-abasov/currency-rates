using WindowsFormsApp2.data;

namespace ConsoleApp1
{
    public class OrganizationToBankConverter
    {
        public static Bank OrganizationtoBank(Organisation organisation)
        {
            return new Bank(organisation.ID, organisation.Title.Value);
        }
    }
}