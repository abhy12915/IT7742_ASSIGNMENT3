using System;

namespace BankAccountApp.Models
{
    public class Transaction
    {
        public DateTime Timestamp { get; } = DateTime.Now;
        public string Type { get; }              
        public decimal Amount { get; }          
        public string Status { get; }            
        public decimal FeeApplied { get; }       
        public decimal NewBalance { get; }       

        public Transaction(string type, decimal amount, string status, decimal feeApplied, decimal newBalance)
        {
            Type = type;
            Amount = Math.Round(amount, 2);
            Status = status;
            FeeApplied = Math.Round(feeApplied, 2);
            NewBalance = Math.Round(newBalance, 2);
        }

        public override string ToString()
        {
            
            return $"{Type}; Amount: ${Amount:F2}; Status: {Status}; Fee: ${FeeApplied:F2}; Final Balance: ${NewBalance:F2}";
        }
    }
}
