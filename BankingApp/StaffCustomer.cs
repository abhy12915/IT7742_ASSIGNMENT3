using System;

namespace BankAccountApp.Models
{
    public class StaffCustomer : Customer
    {
        // Staff get a default 50% fee multiplier (i.e., pay 50% of normal fee)
        public decimal FeeMultiplier { get; } = 0.5m;

        public StaffCustomer(int customerNumber, string name, string contactDetails)
            : base(customerNumber, name, contactDetails) { }

        public override bool IsStaff => true;

        public override string ToString()
        {
            return base.ToString() + " [Staff]";
        }
    }
}
