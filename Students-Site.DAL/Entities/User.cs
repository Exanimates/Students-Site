namespace Students_Site.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }

        public Role Role { get; set; }

        public Student  Student { get; set; }
        public Teacher Teacher { get; set; }
    }
}