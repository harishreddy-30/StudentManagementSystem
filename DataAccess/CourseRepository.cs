using MySql.Data.MySqlClient;
using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace StudentManagementSystem.DataAccess
{
    public class CourseRepository
    {
        private readonly string _connectionString;

        public CourseRepository()
        {
            _connectionString = DatabaseConfig.ConnectionString;
        }

        // CREATE - Add new course
        public int AddCourse(Course course)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Courses 
                                (CourseCode, CourseName, Description, Credits, IsActive) 
                                VALUES 
                                (@CourseCode, @CourseName, @Description, @Credits, @IsActive);
                                SELECT LAST_INSERT_ID();";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CourseCode", course.CourseCode);
                command.Parameters.AddWithValue("@CourseName", course.CourseName);
                command.Parameters.AddWithValue("@Description", course.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Credits", course.Credits);
                command.Parameters.AddWithValue("@IsActive", course.IsActive);

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        // READ - Get all courses
        public List<Course> GetAllCourses()
        {
            List<Course> courses = new List<Course>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Courses ORDER BY CourseId DESC";
                MySqlCommand command = new MySqlCommand(query, connection);

                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Course course = new Course();
                    course.CourseId = reader.GetInt32("CourseId");
                    course.CourseCode = reader.GetString("CourseCode");
                    course.CourseName = reader.GetString("CourseName");
                    
                    if (!reader.IsDBNull(reader.GetOrdinal("Description")))
                    {
                        course.Description = reader.GetString("Description");
                    }
                    
                    course.Credits = reader.GetInt32("Credits");
                    course.IsActive = reader.GetBoolean("IsActive");
                    course.CreatedAt = reader.GetDateTime("CreatedAt");

                    courses.Add(course);
                }
            }

            return courses;
        }

        // READ - Get course by ID
        public Course GetCourseById(int courseId)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Courses WHERE CourseId = @CourseId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CourseId", courseId);

                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Course course = new Course();
                    course.CourseId = reader.GetInt32("CourseId");
                    course.CourseCode = reader.GetString("CourseCode");
                    course.CourseName = reader.GetString("CourseName");
                    
                    if (!reader.IsDBNull(reader.GetOrdinal("Description")))
                    {
                        course.Description = reader.GetString("Description");
                    }
                    
                    course.Credits = reader.GetInt32("Credits");
                    course.IsActive = reader.GetBoolean("IsActive");
                    course.CreatedAt = reader.GetDateTime("CreatedAt");

                    return course;
                }
            }

            return null;
        }

        // UPDATE - Update course
        public bool UpdateCourse(Course course)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = @"UPDATE Courses 
                                SET CourseCode = @CourseCode, 
                                    CourseName = @CourseName, 
                                    Description = @Description, 
                                    Credits = @Credits, 
                                    IsActive = @IsActive 
                                WHERE CourseId = @CourseId";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CourseId", course.CourseId);
                command.Parameters.AddWithValue("@CourseCode", course.CourseCode);
                command.Parameters.AddWithValue("@CourseName", course.CourseName);
                command.Parameters.AddWithValue("@Description", course.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Credits", course.Credits);
                command.Parameters.AddWithValue("@IsActive", course.IsActive);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        // DELETE - Delete course
        public bool DeleteCourse(int courseId)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = "DELETE FROM Courses WHERE CourseId = @CourseId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CourseId", courseId);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}