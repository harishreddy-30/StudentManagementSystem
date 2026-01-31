using StudentManagementSystem.DataAccess;
using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace StudentManagementSystem
{
    class Program
    {
        static StudentRepository studentRepo = new StudentRepository();
        static CourseRepository courseRepo = new CourseRepository();
        static EnrollmentRepository enrollmentRepo = new EnrollmentRepository();

        static void Main(string[] args)
        {
            Console.WriteLine("===========================================");
            Console.WriteLine("   STUDENT MANAGEMENT SYSTEM - ADO.NET");
            Console.WriteLine("===========================================\n");

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n--- MAIN MENU ---");
                Console.WriteLine("1. Student Management");
                Console.WriteLine("2. Course Management");
                Console.WriteLine("3. Enrollment Management");
                Console.WriteLine("4. Exit");
                Console.Write("\nEnter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StudentMenu();
                        break;
                    case "2":
                        CourseMenu();
                        break;
                    case "3":
                        EnrollmentMenu();
                        break;
                    case "4":
                        exit = true;
                        Console.WriteLine("\nThank you for using Student Management System!");
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice! Please try again.");
                        break;
                }
            }
        }

        // STUDENT MENU
        static void StudentMenu()
        {
            bool back = false;

            while (!back)
            {
                Console.WriteLine("\n--- STUDENT MANAGEMENT ---");
                Console.WriteLine("1. View All Students");
                Console.WriteLine("2. Add New Student");
                Console.WriteLine("3. Update Student");
                Console.WriteLine("4. Delete Student");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("\nEnter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllStudents();
                        break;
                    case "2":
                        AddNewStudent();
                        break;
                    case "3":
                        UpdateStudent();
                        break;
                    case "4":
                        DeleteStudent();
                        break;
                    case "5":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice!");
                        break;
                }
            }
        }

        // VIEW ALL STUDENTS
        static void ViewAllStudents()
        {
            Console.WriteLine("\n--- ALL STUDENTS ---");
            var students = studentRepo.GetAllStudents();

            if (students.Count == 0)
            {
                Console.WriteLine("No students found.");
                return;
            }

            Console.WriteLine("\n{0,-5} {1,-20} {2,-30} {3,-15} {4,-10}", 
                "ID", "Full Name", "Email", "Phone", "Active");
            Console.WriteLine(new string('-', 85));

            foreach (var student in students)
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-30} {3,-15} {4,-10}", 
                    student.StudentId, 
                    student.FullName, 
                    student.Email, 
                    student.Phone ?? "N/A", 
                    student.IsActive ? "Yes" : "No");
            }
        }

        // ADD NEW STUDENT
        static void AddNewStudent()
        {
            Console.WriteLine("\n--- ADD NEW STUDENT ---");

            Student student = new Student();

            Console.Write("First Name: ");
            student.FirstName = Console.ReadLine();

            Console.Write("Last Name: ");
            student.LastName = Console.ReadLine();

            Console.Write("Email: ");
            student.Email = Console.ReadLine();

            Console.Write("Phone (optional, press Enter to skip): ");
            string phone = Console.ReadLine();
            student.Phone = string.IsNullOrWhiteSpace(phone) ? null : phone;

            Console.Write("Date of Birth (yyyy-mm-dd): ");
            student.DateOfBirth = DateTime.Parse(Console.ReadLine());

            student.EnrollmentDate = DateTime.Now;
            student.IsActive = true;

            int newId = studentRepo.AddStudent(student);
            Console.WriteLine($"\n✓ Student added successfully! Student ID: {newId}");
        }

        // UPDATE STUDENT
        static void UpdateStudent()
        {
            Console.WriteLine("\n--- UPDATE STUDENT ---");
            Console.Write("Enter Student ID to update: ");
            int id = int.Parse(Console.ReadLine());

            Student student = studentRepo.GetStudentById(id);

            if (student == null)
            {
                Console.WriteLine("Student not found!");
                return;
            }

            Console.WriteLine($"\nCurrent Name: {student.FullName}");
            Console.Write("New First Name (press Enter to keep current): ");
            string firstName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(firstName))
                student.FirstName = firstName;

            Console.Write("New Last Name (press Enter to keep current): ");
            string lastName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(lastName))
                student.LastName = lastName;

            Console.Write("New Email (press Enter to keep current): ");
            string email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email))
                student.Email = email;

            bool success = studentRepo.UpdateStudent(student);
            
            if (success)
                Console.WriteLine("\n✓ Student updated successfully!");
            else
                Console.WriteLine("\n✗ Update failed!");
        }

        // DELETE STUDENT
        static void DeleteStudent()
        {
            Console.WriteLine("\n--- DELETE STUDENT ---");
            Console.Write("Enter Student ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            Student student = studentRepo.GetStudentById(id);

            if (student == null)
            {
                Console.WriteLine("Student not found!");
                return;
            }

            Console.WriteLine($"\nAre you sure you want to delete {student.FullName}? (yes/no): ");
            string confirm = Console.ReadLine();

            if (confirm.ToLower() == "yes")
            {
                bool success = studentRepo.DeleteStudent(id);
                
                if (success)
                    Console.WriteLine("\n✓ Student deleted successfully!");
                else
                    Console.WriteLine("\n✗ Delete failed!");
            }
            else
            {
                Console.WriteLine("\nDelete cancelled.");
            }
        }

        // COURSE MENU
        static void CourseMenu()
        {
            bool back = false;

            while (!back)
            {
                Console.WriteLine("\n--- COURSE MANAGEMENT ---");
                Console.WriteLine("1. View All Courses");
                Console.WriteLine("2. Add New Course");
                Console.WriteLine("3. Back to Main Menu");
                Console.Write("\nEnter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllCourses();
                        break;
                    case "2":
                        AddNewCourse();
                        break;
                    case "3":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice!");
                        break;
                }
            }
        }

        // VIEW ALL COURSES
        static void ViewAllCourses()
        {
            Console.WriteLine("\n--- ALL COURSES ---");
            var courses = courseRepo.GetAllCourses();

            if (courses.Count == 0)
            {
                Console.WriteLine("No courses found.");
                return;
            }

            Console.WriteLine("\n{0,-5} {1,-10} {2,-40} {3,-8}", 
                "ID", "Code", "Course Name", "Credits");
            Console.WriteLine(new string('-', 70));

            foreach (var course in courses)
            {
                Console.WriteLine("{0,-5} {1,-10} {2,-40} {3,-8}", 
                    course.CourseId, 
                    course.CourseCode, 
                    course.CourseName, 
                    course.Credits);
            }
        }

        // ADD NEW COURSE
        static void AddNewCourse()
        {
            Console.WriteLine("\n--- ADD NEW COURSE ---");

            Course course = new Course();

            Console.Write("Course Code (e.g., CS101): ");
            course.CourseCode = Console.ReadLine();

            Console.Write("Course Name: ");
            course.CourseName = Console.ReadLine();

            Console.Write("Description (optional, press Enter to skip): ");
            string description = Console.ReadLine();
            course.Description = string.IsNullOrWhiteSpace(description) ? null : description;

            Console.Write("Credits: ");
            course.Credits = int.Parse(Console.ReadLine());

            course.IsActive = true;

            int newId = courseRepo.AddCourse(course);
            Console.WriteLine($"\n✓ Course added successfully! Course ID: {newId}");
        }

        // ENROLLMENT MENU
        static void EnrollmentMenu()
        {
            bool back = false;

            while (!back)
            {
                Console.WriteLine("\n--- ENROLLMENT MANAGEMENT ---");
                Console.WriteLine("1. View All Enrollments");
                Console.WriteLine("2. Enroll Student in Course");
                Console.WriteLine("3. Update Grade");
                Console.WriteLine("4. Back to Main Menu");
                Console.Write("\nEnter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllEnrollments();
                        break;
                    case "2":
                        EnrollStudent();
                        break;
                    case "3":
                        UpdateGrade();
                        break;
                    case "4":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice!");
                        break;
                }
            }
        }

        // VIEW ALL ENROLLMENTS
        static void ViewAllEnrollments()
        {
            Console.WriteLine("\n--- ALL ENROLLMENTS ---");
            var enrollments = enrollmentRepo.GetAllEnrollments();

            if (enrollments.Count == 0)
            {
                Console.WriteLine("No enrollments found.");
                return;
            }

            Console.WriteLine("\n{0,-5} {1,-20} {2,-30} {3,-10} {4,-12}", 
                "ID", "Student", "Course", "Grade", "Status");
            Console.WriteLine(new string('-', 80));

            foreach (var enrollment in enrollments)
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-30} {3,-10} {4,-12}", 
                    enrollment.EnrollmentId, 
                    enrollment.StudentName, 
                    enrollment.CourseName, 
                    enrollment.Grade ?? "N/A", 
                    enrollment.Status);
            }
        }

        // ENROLL STUDENT
        static void EnrollStudent()
        {
            Console.WriteLine("\n--- ENROLL STUDENT IN COURSE ---");

            Enrollment enrollment = new Enrollment();

            Console.Write("Student ID: ");
            enrollment.StudentId = int.Parse(Console.ReadLine());

            Console.Write("Course ID: ");
            enrollment.CourseId = int.Parse(Console.ReadLine());

            enrollment.EnrollmentDate = DateTime.Now;
            enrollment.Status = "Enrolled";

            int newId = enrollmentRepo.AddEnrollment(enrollment);
            Console.WriteLine($"\n✓ Student enrolled successfully! Enrollment ID: {newId}");
        }

        // UPDATE GRADE
        static void UpdateGrade()
        {
            Console.WriteLine("\n--- UPDATE GRADE ---");
            Console.Write("Enter Enrollment ID: ");
            int id = int.Parse(Console.ReadLine());

            // For simplicity, we'll create a basic enrollment object
            Enrollment enrollment = new Enrollment();
            enrollment.EnrollmentId = id;

            Console.Write("Enter Grade (A, B, C, D, F): ");
            enrollment.Grade = Console.ReadLine();

            Console.Write("Enter Status (Enrolled/Completed/Dropped): ");
            enrollment.Status = Console.ReadLine();

            bool success = enrollmentRepo.UpdateEnrollment(enrollment);
            
            if (success)
                Console.WriteLine("\n✓ Grade updated successfully!");
            else
                Console.WriteLine("\n✗ Update failed!");
        }
    }
}