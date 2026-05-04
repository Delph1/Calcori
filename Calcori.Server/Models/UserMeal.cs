namespace Calcori.Server.Models
{
    public class UserMeal
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime Date { get; set; }
        public int TotalCalories { get; set; }
        public int UserId { get; set; }
        public ICollection<MealItem> MealItems { get; set; } = new List<MealItem>();
    }
}
