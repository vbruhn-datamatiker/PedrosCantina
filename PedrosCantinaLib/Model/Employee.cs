using System;
using System.Collections.Generic;
using System.Text;

namespace PedrosCantinaLib.Model
{
    public class Employee
    {
        // properties
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool IsLeader { get; set; } = false;

        // constructor
        public Employee(int employeeId, string name, string phone, string email, string address, bool isLeader)
        {
            EmployeeId = employeeId;
            Name = name;
            Phone = phone;
            Email = email;
            Address = address;
            IsLeader = isLeader;
        }

        // toString method()

        public override string ToString()
        {
            return $"Employee ID: [{EmployeeId}] \n" +
                $"Navn: {Name}\n" +
                $"Telefon: {Phone}\n" +
                $"Email: {Email}\n" +
                $"Address: {Address}\n" +
                $"Er leder?: {IsLeader}\n";
        }


    }
}
