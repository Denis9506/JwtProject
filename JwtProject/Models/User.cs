namespace JwtProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }  
    }
}
public enum Role
{
    user,
    admin
}