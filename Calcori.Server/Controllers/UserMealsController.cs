using Calcori.Server.Data;
using Calcori.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Calcori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserMealsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserMealsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserMeal>>> GetUserMeals()
        {
            return await _context.UserMeals
                .Include(u => u.MealItems)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserMeal>> GetUserMeal(int id)
        {
            var userMeal = await _context.UserMeals
                .Include(u => u.MealItems)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (userMeal == null)
            {
                return NotFound();
            }

            return userMeal;
        }

        [HttpPost]
        public async Task<ActionResult<UserMeal>> CreateUserMeal(UserMeal userMeal)
        {
            _context.UserMeals.Add(userMeal);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserMeal), new { id = userMeal.Id }, userMeal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserMeal(int id, UserMeal userMeal)
        {
            if  (id != userMeal.Id)
                return BadRequest();

            _context.Entry(userMeal).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserMeal(int id)
        {
            var userMeal = await _context.UserMeals.FindAsync(id);
            if (userMeal == null)
                return NotFound();

            _context.UserMeals.Remove(userMeal);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
