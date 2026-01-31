namespace StudentManagementSystem.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string? Grade { get; set; }
        public string Status { get; set; } = "Enrolled";
        public DateTime CreatedAt { get; set; }

        // Navigation properties (for display purposes)
        public string? StudentName { get; set; }
        public string? CourseName { get; set; }
    }
}