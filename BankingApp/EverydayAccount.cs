using System;

namespace BankAccountApp.Models
{
    public class EverydayAccount : Account
    {
        public EverydayAccount(int id, Customer owner, decimal initialBalance = 0m)
            : base(id, owner, initialBalance)
        {
            InterestRate = 0m;
            OverdraftLimit = 0m;
            FailedWithdrawalFee = 0m;
        }
        public override Transaction Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal must be positive.");

            Transaction transaction;

            if (amount <= Balance)
            {
                Balance -= amount;
                transaction = new Transaction("Withdraw", amount, "Success", 0m, Balance);
            }
            else
            {
                transaction = new Transaction("Withdraw", amount, "Failed", 0m, Balance);
            }

            SaveTransaction(transaction);
            return transaction;
        }

        
        public override Transaction ApplyInterest()
        {
            var transaction = new Transaction("Interest", 0m, "None", 0m, Balance);
            SaveTransaction(transaction);
            return transaction;
        }
    }
}
