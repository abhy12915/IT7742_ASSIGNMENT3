using System;

namespace BankAccountApp.Models
{
    public class InvestmentAccount : Account
    {
        public InvestmentAccount(int id, Customer owner, decimal initialBalance = 0m, decimal interestRate = 0m, decimal failedFee = 10m)
            : base(id, owner, initialBalance)
        {
            InterestRate = interestRate;           // e.g., 2.5 means 2.5%
            OverdraftLimit = 0m;                   // no overdraft allowed
            FailedWithdrawalFee = failedFee;
        }

        // Withdraw only allowed if sufficient balance (no overdraft).
        public override Transaction Withdraw(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Withdrawal must be positive.", nameof(amount));

            if (amount <= Balance)
            {
                Balance = Math.Round(Balance - amount, 2);
                var tx = new Transaction("Withdraw", amount, "Success", 0m, Balance);
                // derived can call protected method
                var save = typeof(Account).GetMethod("SaveTransaction", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                save.Invoke(this, new object[] { tx });
                return tx;
            }
            else
            {
                // failed: apply fee (discounted for staff if applicable)
                decimal fee = GetEffectiveFee();
                Balance = Math.Round(Balance - fee, 2);
                var tx = new Transaction("Withdraw", amount, "Failed", fee, Balance);
                var save = typeof(Account).GetMethod("SaveTransaction", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                save.Invoke(this, new object[] { tx });
                return tx;
            }
        }

        // Apply interest to entire balance (variable interest)
        public override Transaction ApplyInterest()
        {
            if (InterestRate <= 0m)
            {
                var noneTx = new Transaction("Interest", 0m, "None", 0m, Balance);
                var save = typeof(Account).GetMethod("SaveTransaction", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                save.Invoke(this, new object[] { noneTx });
                return noneTx;
            }

            decimal interest = Math.Round(Balance * (InterestRate / 100m), 2);
            Balance = Math.Round(Balance + interest, 2);
            var tx = new Transaction("Interest", interest, "Success", 0m, Balance);
            var save2 = typeof(Account).GetMethod("SaveTransaction", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            save2.Invoke(this, new object[] { tx });
            return tx;
        }
    }
}
