using Calcori.Server.Data;
using Calcori.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Calcori.Server.Controllers
{
    [ApiController]
    [Route("UserMeals/{userMealId}/[controller]")]
    public class MealItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MealItemsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealItem>>> GetMealItems(int userMealId)
        {
            var mealItems = await _context.MealItems
                .Where(m => m.UserMealId == userMealId)
                .ToListAsync();

            if (mealItems == null)
                return NotFound();

            return mealItems;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MealItem>> GetMealItem(int userMealId, int id)
        {
            var mealItem = await _context.MealItems
                .FirstOrDefaultAsync(m => m.Id == id && m.UserMealId == userMealId);

            if (mealItem == null)
                return NotFound();

            return mealItem;
        }

        [HttpPost]
        public async Task<ActionResult<MealItem>> CreateMealItem(int userMealId, MealItem mealItem)
        {
            if (mealItem.UserMealId != userMealId)
                return BadRequest("userMealId mismatch");

            var userMeal = await _context.UserMeals.FindAsync(userMealId);
            if (userMeal == null)
                return NotFound("UserMeal not found");

            _context.MealItems.Add(mealItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMealItem), new { id = mealItem.Id }, mealItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMealItem(int userMealId, int id, MealItem mealItem)
        {
            if (id != mealItem.Id || mealItem.UserMealId != userMealId)
                return BadRequest("ID or userMealId mismatch");

            _context.Entry(mealItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleleMealItem(int userMealId, int id)
        {
            var mealItem = await _context.MealItems
                .FirstOrDefaultAsync(m => m.Id == id && m.UserMealId == userMealId);
            
            if (mealItem == null)
                return NotFound();
            
            _context.MealItems.Remove(mealItem);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
