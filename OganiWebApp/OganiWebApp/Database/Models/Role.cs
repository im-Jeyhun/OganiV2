namespace OganiWebApp.Database.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<User>? Users { get; set; }
    }
}
