using Microsoft.Data.SqlClient;
using PedrosCantinaLib.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace PedrosCantinaLib.Repository
{
    public class EmployeeRepository
    {
        // instansfelter 

        private List<Employee> _employee;

        private DBConnection _dbConnection = new DBConnection();

        // properties

        public List<Employee> Employee
        {
            get { return _employee; }
            set { _employee = value; }
        }

        public DBConnection DBConnection
        { 
            get { return _dbConnection; }
            set { _dbConnection = value; }
        }

        // constructor
        public EmployeeRepository()
        {
            _employee = new List<Employee>();
            _dbConnection = new DBConnection();
        }

        public EmployeeRepository(List<Employee> employee, DBConnection dBConnection)
        {
            Employee = employee;
            DBConnection = dBConnection;
        }

        // toString
        public override string ToString()
        {
            // return list i ToString
            string result = "";
            foreach (Employee e in _employee) 
            {
                result = result + e + ", ";
            }
            return result;
        }

        // ------ CRUD -----

        // CREATE
        public Employee Create(Employee employee)
        {
            _dbConnection.ConnectToDB();

            string sql = "INSERT INTO Employee (Name, Phone, Email, Address, IsLeader) " +
                "VALUES (@Name, @Phone, @Email, @Address, @IsLeader) ";

            SqlCommand cmd = new SqlCommand(sql, _dbConnection.Connection);

            cmd.Parameters.AddWithValue("@name", employee.Name);
            cmd.Parameters.AddWithValue("@phone", employee.Phone);
            cmd.Parameters.AddWithValue("@email", employee.Email);
            cmd.Parameters.AddWithValue("@address", employee.Address);
            cmd.Parameters.AddWithValue("@isLeader", employee.IsLeader);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                throw new ArgumentException("Error. No rows were affected.");
            }

            _dbConnection.DisconnectFromDB();

            return employee;
        }

        // READ
        public List<Employee> ReadAll()
        {
            List<Employee> list = new List<Employee>();

            _dbConnection.ConnectToDB();

            string sql = "SELECT * FROM Employee";
            SqlCommand cmd = new SqlCommand(sql, _dbConnection.Connection);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = (int)reader["EmployeeId"];
                string name = (string)reader["Name"];
                string phone = (string)reader["Phone"];
                string email = reader["Email"] as string;
                string address = reader["Address"] as string;
                bool isLeader = (bool)reader["IsLeader"];

                list.Add(new Employee(id, name, phone, email, address, isLeader));
            }

            _dbConnection.DisconnectFromDB();

            return list;
        }

        // GET BY ID

        public Employee GetById(int employeeId)
        {
            _dbConnection.ConnectToDB();

            string sql = "SELECT * FROM Employee " +
                "WHERE EmployeeId = @EmployeeId";

            SqlCommand cmd = new SqlCommand(sql, _dbConnection.Connection);
            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

            SqlDataReader reader = cmd.ExecuteReader();

            Employee employeeFound = null;

            if (reader.Read())
            {
                int id = (int)reader["EmployeeId"];
                string name = (string)reader["Name"];
                string phone = (string)reader["Phone"];
                string email = (string)reader["Email"];
                string address = (string)reader["Address"];
                bool isLeader = (bool)reader["IsLeader"];

                employeeFound = new Employee(id, name, phone, email, address, isLeader);
            } 
            else
            {
                _dbConnection.DisconnectFromDB();

                throw new KeyNotFoundException($"Error. No employee found with ID: [{employeeId}]");
            }

            _dbConnection.DisconnectFromDB();

            return employeeFound;

        }
        
        // UPDATE
        public Employee Update(int employeeId, Employee updatedEmployee)
        {
            _dbConnection.ConnectToDB();

            string sql = "UPDATE Employee " + 
                "SET Name = @Name, Phone = @Phone, Email = @Email, Address = @Address, IsLeader = @IsLeader " +
                "WHERE EmployeeId = @EmployeeId ";

            SqlCommand cmd = new SqlCommand(sql, _dbConnection.Connection);

            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
            cmd.Parameters.AddWithValue("@Name", updatedEmployee.Name);
            cmd.Parameters.AddWithValue("@Phone", updatedEmployee.Phone);
            cmd.Parameters.AddWithValue("@Email", updatedEmployee.Email);
            cmd.Parameters.AddWithValue("@Address", updatedEmployee.Address);
            cmd.Parameters.AddWithValue("@IsLeader", updatedEmployee.IsLeader);

            int rowsAffected = cmd.ExecuteNonQuery();

            // disconnect before update

            _dbConnection.DisconnectFromDB();
            
            if (rowsAffected == 1)
            {
                return GetById(employeeId);
            }

            throw new KeyNotFoundException($"Error. No rows affected, employee with ID [{employeeId}] does not exist.");

        }

        // DELETE

        public Employee Delete(int employeeId)
        {
            Employee DeleteEmployee = GetById(employeeId);

            _dbConnection.ConnectToDB();

            string sql ="DELETE FROM Employee " +
                "WHERE EmployeeId = @EmployeeId";

            SqlCommand cmd = new SqlCommand(sql, _dbConnection.Connection);
            
            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

            int rowsAffected = cmd.ExecuteNonQuery();

            _dbConnection.DisconnectFromDB();

            if (rowsAffected != 1) 
            {
                throw new KeyNotFoundException($"Error. No employee with ID [{employeeId}] found and cannot be deleted");
            }

            return DeleteEmployee;

        }
        
    }
}
