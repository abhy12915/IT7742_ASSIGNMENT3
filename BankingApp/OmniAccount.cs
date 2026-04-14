using System;

namespace BankAccountApp.Models
{
    public class OmniAccount : Account
    {
        private const decimal InterestThreshold = 1000m;

        public OmniAccount(int id, Customer owner, decimal initialBalance = 0m, decimal interestRate = 0m, decimal overdraftLimit = 0m, decimal failedFee = 10m)
            : base(id, owner, initialBalance)
        {
            InterestRate = interestRate;
            OverdraftLimit = overdraftLimit;
            FailedWithdrawalFee = failedFee;
        }

        // Withdraw: allowed up to balance + overdraft
        public override Transaction Withdraw(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Withdrawal must be positive.", nameof(amount));

            decimal allowed = Balance + OverdraftLimit;
            if (amount <= allowed)
            {
                Balance = Math.Round(Balance - amount, 2);
                var tx = new Transaction("Withdraw", amount, "Success", 0m, Balance);
                var save = typeof(Account).GetMethod("SaveTransaction", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                save.Invoke(this, new object[] { tx });
                return tx;
            }
            else
            {
                // Failed: charge fee (effective fee may be discounted)
                decimal fee = GetEffectiveFee();
                Balance = Math.Round(Balance - fee, 2);
                var tx = new Transaction("Withdraw", amount, "Failed", fee, Balance);
                var save = typeof(Account).GetMethod("SaveTransaction", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                save.Invoke(this, new object[] { tx });
                return tx;
            }
        }

        // Interest applied only on amount exceeding $1000
        public override Transaction ApplyInterest()
        {
            if (InterestRate <= 0m)
            {
                var noneTx = new Transaction("Interest", 0m, "None", 0m, Balance);
                var save = typeof(Account).GetMethod("SaveTransaction", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                save.Invoke(this, new object[] { noneTx });
                return noneTx;
            }

            decimal baseAmount = Math.Max(0m, Balance - InterestThreshold);
            if (baseAmount <= 0m)
            {
                var noneTx = new Transaction("Interest", 0m, "None", 0m, Balance);
                var save = typeof(Account).GetMethod("SaveTransaction", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                save.Invoke(this, new object[] { noneTx });
                return noneTx;
            }

            decimal interest = Math.Round(baseAmount * (InterestRate / 100m), 2);
            Balance = Math.Round(Balance + interest, 2);
            var tx = new Transaction("Interest", interest, "Success", 0m, Balance);
            var save2 = typeof(Account).GetMethod("SaveTransaction", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            save2.Invoke(this, new object[] { tx });
            return tx;
        }
    }
}
