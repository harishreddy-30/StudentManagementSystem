using MySql.Data.MySqlClient;
using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace StudentManagementSystem.DataAccess
{
    public class EnrollmentRepository
    {
        private readonly string _connectionString;

        public EnrollmentRepository()
        {
            _connectionString = DatabaseConfig.ConnectionString;
        }

        // CREATE - Add enrollment
        public int AddEnrollment(Enrollment enrollment)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Enrollments 
                                (StudentId, CourseId, EnrollmentDate, Grade, Status) 
                                VALUES 
                                (@StudentId, @CourseId, @EnrollmentDate, @Grade, @Status);
                                SELECT LAST_INSERT_ID();";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@StudentId", enrollment.StudentId);
                command.Parameters.AddWithValue("@CourseId", enrollment.CourseId);
                command.Parameters.AddWithValue("@EnrollmentDate", enrollment.EnrollmentDate);
                command.Parameters.AddWithValue("@Grade", enrollment.Grade ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Status", enrollment.Status);

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        // READ - Get all enrollments with student and course names
        public List<Enrollment> GetAllEnrollments()
        {
            List<Enrollment> enrollments = new List<Enrollment>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                // JOIN query to get student name and course name
                string query = @"SELECT e.*, 
                                CONCAT(s.FirstName, ' ', s.LastName) as StudentName,
                                c.CourseName
                                FROM Enrollments e
                                JOIN Students s ON e.StudentId = s.StudentId
                                JOIN Courses c ON e.CourseId = c.CourseId
                                ORDER BY e.EnrollmentId DESC";

                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Enrollment enrollment = new Enrollment();
                    enrollment.EnrollmentId = reader.GetInt32("EnrollmentId");
                    enrollment.StudentId = reader.GetInt32("StudentId");
                    enrollment.CourseId = reader.GetInt32("CourseId");
                    enrollment.EnrollmentDate = reader.GetDateTime("EnrollmentDate");
                    
                    if (!reader.IsDBNull(reader.GetOrdinal("Grade")))
                    {
                        enrollment.Grade = reader.GetString("Grade");
                    }
                    
                    enrollment.Status = reader.GetString("Status");
                    enrollment.CreatedAt = reader.GetDateTime("CreatedAt");
                    
                    // Fill navigation properties
                    enrollment.StudentName = reader.GetString("StudentName");
                    enrollment.CourseName = reader.GetString("CourseName");

                    enrollments.Add(enrollment);
                }
            }

            return enrollments;
        }

        // READ - Get enrollments by student ID
        public List<Enrollment> GetEnrollmentsByStudentId(int studentId)
        {
            List<Enrollment> enrollments = new List<Enrollment>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = @"SELECT e.*, c.CourseName
                                FROM Enrollments e
                                JOIN Courses c ON e.CourseId = c.CourseId
                                WHERE e.StudentId = @StudentId
                                ORDER BY e.EnrollmentDate DESC";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@StudentId", studentId);

                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Enrollment enrollment = new Enrollment();
                    enrollment.EnrollmentId = reader.GetInt32("EnrollmentId");
                    enrollment.StudentId = reader.GetInt32("StudentId");
                    enrollment.CourseId = reader.GetInt32("CourseId");
                    enrollment.EnrollmentDate = reader.GetDateTime("EnrollmentDate");
                    
                    if (!reader.IsDBNull(reader.GetOrdinal("Grade")))
                    {
                        enrollment.Grade = reader.GetString("Grade");
                    }
                    
                    enrollment.Status = reader.GetString("Status");
                    enrollment.CreatedAt = reader.GetDateTime("CreatedAt");
                    enrollment.CourseName = reader.GetString("CourseName");

                    enrollments.Add(enrollment);
                }
            }

            return enrollments;
        }

        // UPDATE - Update enrollment (grade and status)
        public bool UpdateEnrollment(Enrollment enrollment)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = @"UPDATE Enrollments 
                                SET Grade = @Grade, 
                                    Status = @Status 
                                WHERE EnrollmentId = @EnrollmentId";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@EnrollmentId", enrollment.EnrollmentId);
                command.Parameters.AddWithValue("@Grade", enrollment.Grade ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Status", enrollment.Status);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        // DELETE - Delete enrollment
        public bool DeleteEnrollment(int enrollmentId)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "DELETE FROM Enrollments WHERE EnrollmentId = @EnrollmentId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@EnrollmentId", enrollmentId);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}