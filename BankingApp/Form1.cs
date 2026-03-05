using BankAccountApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankingApp
{
    public partial class Form1 : Form
    {
        private Account currentAccount;
        private Customer defaultCustomer;

        public Form1()
        {
            InitializeComponent();

            cmbAccountType.Items.Add("Everyday Account");
            cmbAccountType.Items.Add("Omni Account");
            cmbAccountType.Items.Add("Investment Account");

            // Default customer
            defaultCustomer = new Customer(101, "Abinash", "1234567");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (currentAccount == null) return;

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("Enter a valid amount.");
                return;
            }

            var tx = currentAccount.Deposit(amount);
            lstOutput.Items.Add(tx.ToString());
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            lstOutput.Items.Clear();

            string choice = cmbAccountType.SelectedItem?.ToString();
            if (choice == null)
            {
                MessageBox.Show("Please select an account type.");
                return;
            }

            switch (choice)
            {
                case "Everyday Account":
                    currentAccount = new EverydayAccount(1001, defaultCustomer, 500);
                    break;

                case "Omni Account":
                    currentAccount = new OmniAccount(1002, defaultCustomer, 800, 0.04m, 100, 10);
                    break;

                case "Investment Account":
                    currentAccount = new InvestmentAccount(1003, defaultCustomer, 1000, 0.05m, 15);
                    break;
            }

            lstOutput.Items.Add("Account Created:");
            lstOutput.Items.Add(currentAccount.GetSummary());
        
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            if (currentAccount == null) return;

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("Enter a valid amount.");
                return;
            }

            var tx = currentAccount.Withdraw(amount);
            lstOutput.Items.Add(tx.ToString());
        }

        private void btnSummary_Click(object sender, EventArgs e)
        {
            if (currentAccount == null) return;
            lstOutput.Items.Add(currentAccount.GetSummary());
        }

        private void btnInterest_Click(object sender, EventArgs e)
        {
            if (currentAccount == null) return;

            var tx = currentAccount.ApplyInterest();
            if (tx != null && tx.Type == "Interest" && tx.Status == "Success")
                lstOutput.Items.Add(tx.ToString());
            else
                lstOutput.Items.Add("This account does not support interest.");
        }

        private void lstOutput_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
