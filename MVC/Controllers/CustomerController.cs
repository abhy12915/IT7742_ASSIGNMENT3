using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC
{
    public class CustomerController
    {
        private List<Customer> customers = new List<Customer>();

        public void AddCustomer(Customer customer)
        {
            if (customer == null)
                throw new Exception("Customer cannot be null.");

            customers.Add(customer);
        }

        public void UpdateCustomer(int id, string newName, string newLastName)
        {
            Customer customer = customers.FirstOrDefault(c => c.Id == id);

            if (customer == null)
                throw new Exception("Customer not found.");

            customer.Name = newName;
            customer.LastName = newLastName;
        }

        public void DeleteCustomer(int id)
        {
            Customer customer = customers.FirstOrDefault(c => c.Id == id);

            if (customer == null)
                throw new Exception("Customer not found.");

            customers.Remove(customer);
        }

        public List<Customer> GetAllCustomers()
        {
            return customers;
        }
    }
}