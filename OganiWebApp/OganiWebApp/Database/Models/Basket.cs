namespace OganiWebApp.Database.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<BasketProduct>? BasketProducts { get; set; }
    }
}
