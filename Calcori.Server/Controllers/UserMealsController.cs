using Calcori.Server.Data;
using Calcori.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Calcori.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserMealsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserMealsController(AppDbContext context)
        {
            _context = context;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if ( int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("Invalid or missing token.");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserMeal>>> GetUserMeals()
        {
            return await _context.UserMeals
                .Where(u => u.UserId == GetCurrentUserId())
                .Include(u => u.MealItems)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserMeal>> GetUserMeal(int id)
        {
            var userMeal = await _context.UserMeals
                .Include(u => u.MealItems)
                .FirstOrDefaultAsync(u => u.Id == id && u.UserId == GetCurrentUserId());
            if (userMeal == null)
            {
                return NotFound();
            }

            return userMeal;
        }

        [HttpPost]
        public async Task<ActionResult<UserMeal>> CreateUserMeal(UserMeal userMeal)
        {
            int currentUserId = GetCurrentUserId();
            userMeal.UserId = currentUserId;

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
            int currentUserId = GetCurrentUserId();

            var userMeal = await _context.UserMeals
                .FirstOrDefaultAsync(u => u.Id == id && u.UserId == currentUserId);

            if (userMeal == null)
                return NotFound();

            _context.UserMeals.Remove(userMeal);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
