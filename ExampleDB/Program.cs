using System;
using System.Data;
using IBM.Data.Db2;
using System.Data.SqlClient;
using System.Data.Common;

class Program
{
    static void Main()
    {
        // TODO: add actual connection
        string sqlServerConnectionString = "";
        string db2ConnectionString = "";

        CreateSqlServerDatabase(sqlServerConnectionString);
        CreateDb2Database(db2ConnectionString);

        Console.WriteLine("Databases created and populated successfully.");
    }

    static void CreateSqlServerDatabase(string connectionString)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EmployeeDB') CREATE DATABASE EmployeeDB;", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        string employeeDbConnStr = connectionString.Replace("Database=master", "Database=EmployeeDB");

        using (SqlConnection conn = new SqlConnection(employeeDbConnStr))
        {
            conn.Open();

            string createTableQuery = @"
                IF OBJECT_ID('Employees', 'U') IS NULL
                CREATE TABLE Employees (
                    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
                    FirstName NVARCHAR(50),
                    LastName NVARCHAR(50),
                    Age INT,
                    DepartmentID INT
                );";
            using (SqlCommand cmd = new SqlCommand(createTableQuery, conn))
            {
                cmd.ExecuteNonQuery();
            }

            Random random = new Random();
            string[] firstNames = { "John", "Jane", "Alex", "Emily", "Michael", "Sarah", "Chris", "Jessica", "Daniel", "Laura" };
            string[] lastNames = { "Smith", "Johnson", "Brown", "Williams", "Jones", "Miller", "Davis", "Garcia", "Rodriguez", "Martinez" };

            for (int i = 0; i < 100; i++)
            {
                string firstName = firstNames[random.Next(firstNames.Length)];
                string lastName = lastNames[random.Next(lastNames.Length)];
                int age = random.Next(22, 60);
                int departmentId = random.Next(1, 6); // Departments 1-5

                string insertQuery = $"INSERT INTO Employees (FirstName, LastName, Age, DepartmentID) VALUES ('{firstName}', '{lastName}', {age}, {departmentId})";
                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    static void CreateDb2Database(string connectionString)
    {
        using (DB2Connection conn = new DB2Connection(connectionString))
        {
            conn.Open();

            string createTableQuery = @"
                CREATE TABLE Departments (
                    DepartmentID INT PRIMARY KEY,
                    DepartmentName VARCHAR(100)
                )";
            using (DB2Command cmd = new DB2Command(createTableQuery, conn))
            {
                try { cmd.ExecuteNonQuery(); } catch (Exception) { /* Ignore if already exists */ }
            }

            string[] departments = { "HR", "Finance", "Engineering", "Marketing", "Sales" };
            for (int i = 0; i < departments.Length; i++)
            {
                string insertQuery = $"INSERT INTO Departments (DepartmentID, DepartmentName) VALUES ({i + 1}, '{departments[i]}')";
                using (DB2Command cmd = new DB2Command(insertQuery, conn))
                {
                    try { cmd.ExecuteNonQuery(); } catch (Exception) { /* Ignore duplicate errors */ }
                }
            }
        }
    }
}
