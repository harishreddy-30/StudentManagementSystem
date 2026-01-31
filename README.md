# Student Management System

A student management system built with C#, ADO.NET, and MySQL to manage students, courses, and enrollments.

## Features

- **Student Management** - Add, view, update, and delete students
- **Course Management** - Add and view courses
- **Enrollment Management** - Enroll students in courses and update grades

## Technologies Used

- C# (.NET 9.0)
- MySQL
- ADO.NET

## Project Structure
```
StudentManagementSystem/
├── Models/
│   ├── Student.cs
│   ├── Course.cs
│   └── Enrollment.cs
├── DataAccess/
│   ├── DatabaseConfig.cs
│   ├── StudentRepository.cs
│   ├── CourseRepository.cs
│   └── EnrollmentRepository.cs
└── Program.cs
```

## Database Schema

**Students Table**
- StudentId, FirstName, LastName, Email, Phone, DateOfBirth, EnrollmentDate, IsActive, CreatedAt

**Courses Table**
- CourseId, CourseCode, CourseName, Description, Credits, IsActive, CreatedAt

**Enrollments Table**
- EnrollmentId, StudentId, CourseId, EnrollmentDate, Grade, Status, CreatedAt
