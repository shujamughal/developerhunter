namespace Jobverse.Models
{
    public class Employer
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Password { get; set; }

        public Employer()
        {
            Email = "company@gmail.com";
        }
    }
}
