using Microsoft.AspNetCore.Mvc;
using ExpenseAndPoint.Data;
using ExpenseAndPointServer.Models;
using ExpenseAndPointServer.Services;
using ExpenseAndPointServer.Services.Cryptographer;
using ExpenseAndPointServer.Services.PasswordChecker;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseAndPointServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private UserService userService;

        public UsersController(AppDbContext context, ICryptographer cryptographer, IPasswordChecker passwordChecker)
        {
            _context = context;
            userService = new UserService(context, cryptographer, passwordChecker);
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            var result = await userService.GetUsers();
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            } else 
            {
                return Ok(result);
            }
        }

        // GET: api/Users/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var result = await userService.GetUserById(id);
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // GET: api/Users/Name
        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserByName(string name)
        {
            var result = await userService.GetUserByName(name);
            if (result.IsNullOrEmpty())
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                await userService.AddUser(user);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await userService.DeleteUserById(id);
            return NoContent();
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser(int id, User user)
        {
            User editedUser;
            try
            {
                editedUser = await userService.EditUser(id, user);
            } catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return Ok(editedUser);
        }
    }
}
