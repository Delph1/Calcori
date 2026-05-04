using System.Text.Json.Serialization;

namespace Calcori.Server.Models
{
    public class MealItem
    {
        public int Id { get; set; }
        public int UserMealId { get; set; }
        public required string FoodName { get; set; }
        public int CaloriesPer100g { get; set; }
        public double Weight { get; set; }
        public int TotalCalories => (int)(CaloriesPer100g * (Weight / 100.0));
        public UserMeal? UserMeal { get; set; }
    }
}
