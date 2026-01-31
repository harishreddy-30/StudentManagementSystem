using MySql.Data.MySqlClient;
using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace StudentManagementSystem.DataAccess
{
    public class StudentRepository
    {
        private readonly string _connectionString;

        // Constructor - runs when you create a new StudentRepository object
        public StudentRepository()
        {
            _connectionString = DatabaseConfig.ConnectionString;
        }

        // CREATE - Add a new student to database
        public int AddStudent(Student student)
        {
            // using statement ensures connection is closed automatically
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                // SQL query with parameters (@FirstName, @LastName, etc.)
                string query = @"INSERT INTO Students 
                                (FirstName, LastName, Email, Phone, DateOfBirth, EnrollmentDate, IsActive) 
                                VALUES 
                                (@FirstName, @LastName, @Email, @Phone, @DateOfBirth, @EnrollmentDate, @IsActive);
                                SELECT LAST_INSERT_ID();";

                // Create command object
                MySqlCommand command = new MySqlCommand(query, connection);
                
                // Add parameters to prevent SQL injection
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@Phone", student.Phone ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                command.Parameters.AddWithValue("@EnrollmentDate", student.EnrollmentDate);
                command.Parameters.AddWithValue("@IsActive", student.IsActive);

                // Open connection
                connection.Open();
                
                // ExecuteScalar returns the new StudentId
                int newStudentId = Convert.ToInt32(command.ExecuteScalar());
                
                return newStudentId;
            }
        }

        // READ - Get all students from database
        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Students ORDER BY StudentId DESC";
                MySqlCommand command = new MySqlCommand(query, connection);

                connection.Open();
                
                // ExecuteReader returns rows of data
                MySqlDataReader reader = command.ExecuteReader();

                // Loop through each row
                while (reader.Read())
                {
                    Student student = new Student();
                    student.StudentId = reader.GetInt32("StudentId");
                    student.FirstName = reader.GetString("FirstName");
                    student.LastName = reader.GetString("LastName");
                    student.Email = reader.GetString("Email");
                    
                    // Check if Phone is null in database
                    if (!reader.IsDBNull(reader.GetOrdinal("Phone")))
                    {
                        student.Phone = reader.GetString("Phone");
                    }
                    
                    student.DateOfBirth = reader.GetDateTime("DateOfBirth");
                    student.EnrollmentDate = reader.GetDateTime("EnrollmentDate");
                    student.IsActive = reader.GetBoolean("IsActive");
                    student.CreatedAt = reader.GetDateTime("CreatedAt");

                    students.Add(student);
                }
            }

            return students;
        }

        // READ - Get single student by ID
        public Student GetStudentById(int studentId)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Students WHERE StudentId = @StudentId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@StudentId", studentId);

                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Student student = new Student();
                    student.StudentId = reader.GetInt32("StudentId");
                    student.FirstName = reader.GetString("FirstName");
                    student.LastName = reader.GetString("LastName");
                    student.Email = reader.GetString("Email");
                    
                    if (!reader.IsDBNull(reader.GetOrdinal("Phone")))
                    {
                        student.Phone = reader.GetString("Phone");
                    }
                    
                    student.DateOfBirth = reader.GetDateTime("DateOfBirth");
                    student.EnrollmentDate = reader.GetDateTime("EnrollmentDate");
                    student.IsActive = reader.GetBoolean("IsActive");
                    student.CreatedAt = reader.GetDateTime("CreatedAt");

                    return student;
                }
            }

            return null;
        }

        // UPDATE - Update existing student
        public bool UpdateStudent(Student student)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = @"UPDATE Students 
                                SET FirstName = @FirstName, 
                                    LastName = @LastName, 
                                    Email = @Email, 
                                    Phone = @Phone, 
                                    DateOfBirth = @DateOfBirth, 
                                    EnrollmentDate = @EnrollmentDate, 
                                    IsActive = @IsActive 
                                WHERE StudentId = @StudentId";

                MySqlCommand command = new MySqlCommand(query, connection);
                
                command.Parameters.AddWithValue("@StudentId", student.StudentId);
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@Phone", student.Phone ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                command.Parameters.AddWithValue("@EnrollmentDate", student.EnrollmentDate);
                command.Parameters.AddWithValue("@IsActive", student.IsActive);

                connection.Open();
                
                // ExecuteNonQuery returns number of rows affected
                int rowsAffected = command.ExecuteNonQuery();
                
                return rowsAffected > 0;
            }
        }

        // DELETE - Delete student
        public bool DeleteStudent(int studentId)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "DELETE FROM Students WHERE StudentId = @StudentId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@StudentId", studentId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                
                return rowsAffected > 0;
            }
        }
    }
}