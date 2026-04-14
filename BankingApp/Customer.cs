using System;

namespace BankAccountApp.Models
{
    public class Customer
    {
        public int CustomerNumber { get; }
        public string Name { get; }
        public string ContactDetails { get; }

        public Customer(int customerNumber, string name, string contactDetails = "")
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            CustomerNumber = customerNumber;
            Name = name;
            ContactDetails = contactDetails ?? "";
        }
        public virtual bool IsStaff => false;
        public override string ToString() => $"{Name} ({CustomerNumber})";
    }
}
