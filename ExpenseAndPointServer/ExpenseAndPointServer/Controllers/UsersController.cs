using ExpenseAndPoint.Data;
using ExpenseAndPointServer.Models;
using ExpenseAndPointServer.Services;
using ExpenseAndPointServer.Services.Cryptographer;
using ExpenseAndPointServer.Services.PasswordChecker;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUser()
        {
            var userList = await userService.GetUsers();
            var userDtoList = userList.Select(u => u.ToUserDtoMap());
            if (userDtoList.IsNullOrEmpty())
            {
                return NotFound();
            } else 
            {
                return Ok(userDtoList);
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
                    throw new Exception("Пользователя с данным Id не существует");
                }
                else
                {
                    return Ok(result.ToUserDtoMap());
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
            try
            {
                var result = await userService.GetUserByName(name);
                if (result == null)
                {
                    throw new Exception("Пользователя с данным именем не существует"); 
                }
                else
                {
                    return Ok(result.ToUserDtoMap());
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser(UserDto userDto)
        {
            try
            {
                await userService.AddUser(userDto.ToUserMap());
                return Ok(userDto.Name);
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
        [HttpPut("EditUserName/{id}")]
        public async Task<ActionResult<UserDto>> PutUserName(int id, UserDto userDto)
        {
            User editedUser;
            try
            {
               editedUser  = await userService.EditUserName(id, userDto.ToUserMap());
            } catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return Ok(editedUser.ToUserDtoMap());
        }
        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("EditUserPassword/{id}")]
        public async Task<ActionResult<UserDto>> PutUserPassword(int id, UserDto userDto)
        {
            User editedUser;
            try
            {
                editedUser = await userService.EditUserPassword(id, userDto.ToUserMap());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return Ok(editedUser.ToUserDtoMap());
        }
    }
}
