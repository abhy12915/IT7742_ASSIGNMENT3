using System;

namespace MVC
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string LastName { get; set; }

        public Customer(int id, string name,string LastName)
        {
            Id = id;
            Name = name;
            LastName = LastName;
        }
    }
}