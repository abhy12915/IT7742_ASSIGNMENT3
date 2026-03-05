using System;
using System.Collections.Generic;

namespace BankAccountApp.Models
{
    public abstract class Account
    {
        public int Id { get; }

        public Customer Owner { get; }

        public decimal Balance { get; protected set; }

        public decimal InterestRate { get; protected set; } = 0m;

        public decimal OverdraftLimit { get; protected set; } = 0m;

        public decimal FailedWithdrawalFee { get; protected set; } = 0m;

        public Transaction LastTransaction { get; protected set; }

        protected readonly List<Transaction> _history = new List<Transaction>();

        public IReadOnlyList<Transaction> History => _history.AsReadOnly();

        // Constructor
        protected Account(int id, Customer owner, decimal initialBalance = 0m)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner), "Account owner cannot be null.");

            if (initialBalance < 0)
                throw new ArgumentException("Initial balance cannot be negative.", nameof(initialBalance));

            Id = id;
            Owner = owner;
            Balance = Math.Round(initialBalance, 2);
        }

        // Deposit Method
        public virtual Transaction Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit must be greater than zero.", nameof(amount));

            Balance = Math.Round(Balance + amount, 2);

            var tx = new Transaction("Deposit", amount, "Success", 0m, Balance);

            SaveTransaction(tx);

            return tx;
        }

        // Withdraw Method
        public virtual Transaction Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal must be greater than zero.", nameof(amount));

            decimal allowedAmount = Balance + OverdraftLimit;

            if (amount <= allowedAmount)
            {
                Balance = Math.Round(Balance - amount, 2);

                var tx = new Transaction("Withdraw", amount, "Success", 0m, Balance);

                SaveTransaction(tx);

                return tx;
            }
            else
            {
                decimal fee = GetEffectiveFee();

                Balance = Math.Round(Balance - fee, 2);

                var tx = new Transaction("Withdraw", amount, "Failed", fee, Balance);

                SaveTransaction(tx);

                return tx;
            }
        }

        // Calculate Fee
        protected virtual decimal GetEffectiveFee()
        {
            if (FailedWithdrawalFee <= 0)
                return 0m;

            if (Owner.IsStaff)
            {
                if (Owner is StaffCustomer staff)
                    return Math.Round(FailedWithdrawalFee * staff.FeeMultiplier, 2);

                return Math.Round(FailedWithdrawalFee * 0.5m, 2);
            }

            return Math.Round(FailedWithdrawalFee, 2);
        }

        // Apply Interest (can be overridden in derived classes)
        public virtual Transaction ApplyInterest()
        {
            if (InterestRate <= 0)
                return new Transaction("Interest", 0m, "No Interest", 0m, Balance);

            decimal interest = Math.Round(Balance * InterestRate, 2);
            Balance = Math.Round(Balance + interest, 2);

            var tx = new Transaction("Interest", interest, "Success", 0m, Balance);

            SaveTransaction(tx);

            return tx;
        }

        // Account Summary
        public virtual string GetSummary()
        {
            string summary = $"Account Type: {GetType().Name}\n" +
                             $"ID: {Id}\n" +
                             $"Owner: {Owner.Name}\n" +
                             $"Balance: ${Balance:F2}";

            if (LastTransaction != null)
                summary += $"\nLast Transaction: {LastTransaction}";
            else
                summary += "\nNo transactions yet.";

            return summary;
        }

        // Save Transaction
        protected void SaveTransaction(Transaction tx)
        {
            LastTransaction = tx;
            _history.Add(tx);
        }
    }
}