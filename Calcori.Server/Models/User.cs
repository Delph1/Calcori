namespace Calcori.Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; } = null;
    }
}
