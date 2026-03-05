using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BankAccountApp.Models;

namespace BankAccountApp.Tests
{
    [TestClass]
    public class AccountTests
    {
        // Create a simple test customer
        private Customer CreateCustomer()
        {
            return new Customer(1, "John");
        }

        // Because Account is abstract, we create a test subclass
        private class TestAccount : Account
        {
            public TestAccount(int id, Customer owner, decimal balance)
                : base(id, owner, balance)
            {
            }
        }

        [TestMethod]
        public void Deposit_ShouldIncreaseBalance()
        {
            var customer = CreateCustomer();
            var account = new TestAccount(1, customer, 100m);

            account.Deposit(50m);

            Assert.AreEqual(150m, account.Balance);
        }

        [TestMethod]
        public void Withdraw_ShouldDecreaseBalance_WhenEnoughFunds()
        {
            var customer = CreateCustomer();
            var account = new TestAccount(1, customer, 200m);

            account.Withdraw(50m);

            Assert.AreEqual(150m, account.Balance);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Deposit_ShouldThrowException_WhenAmountIsZero()
        {
            var customer = CreateCustomer();
            var account = new TestAccount(1, customer, 100m);

            account.Deposit(0m);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Withdraw_ShouldThrowException_WhenAmountIsZero()
        {
            var customer = CreateCustomer();
            var account = new TestAccount(1, customer, 100m);

            account.Withdraw(0m);
        }
    }
}