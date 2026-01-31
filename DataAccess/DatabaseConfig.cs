namespace StudentManagementSystem.DataAccess
{
    public static class DatabaseConfig
    {
        public static string ConnectionString
        {
            get
            {
                return "Server=localhost;Port=3306;Database=StudentManagementDB;User=root;Password=Harishwar@123;";
            }
        }
    }
}