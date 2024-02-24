using ExpenseAndPoint.Data;
using ExpenseAndPointServer.ErrorLogging;
using ExpenseAndPointServer.Models.Users;
using ExpenseAndPointServer.Services;
using ExpenseAndPointServer.Services.Cryptographer;
using ExpenseAndPointServer.Services.PasswordChecker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace ExpenseAndPointServer.Controllers
{
    /// <summary>
    /// Контроллер для работы с пользователями
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// Сервис для работы с моделями пользователей
        /// </summary>
        private UserService userService;

        /// <summary>
        /// Логгирование ошибок
        /// </summary>
        private readonly ErrorLogger _errorLogger = new ErrorLogger();

        /// <summary>
        /// Конструктор создания контроллера
        /// </summary>
        /// <param name="context">Контекст для работы с БД</param>
        /// <param name="cryptographer">Сервис шифрования пароля</param>
        /// <param name="passwordChecker">Сервис проверки надежности пароля</param>
        public UsersController(AppDbContext context, ICryptographer cryptographer, IPasswordChecker passwordChecker)
        {
            userService = new UserService(context, cryptographer, passwordChecker);
        }

        /// <summary>
        /// Получение списка пользователей.
        /// </summary>
        /// <returns>Список пользователей</returns>
        /// <response code="201">Список пользователей</response>
        /// <response code="204">Список пользователей пуст</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var userList = await userService.GetUsers();
            var userDtoList = userList.Select(u => u.ToUserOutputDtoMap());
            if (userDtoList.IsNullOrEmpty())
            {
                return NoContent();
            }
            else
            {
                return Ok(userDtoList);
            }
        }

        /// <summary>
        /// Получение пользователя по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns>Пользователь с указанным идентификатором</returns>
        /// <response code="200">Найденный пользователь</response>
        /// <response code="204">Пользователь с указанным идентификатором не найден</response>
        /// <response code="500">Ошибка при поиске пользователя</response> 
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult<UserResponse>> GetUserById(int id)
        {
            try
            {
                var result = await userService.GetUserById(id);
                if (result == null)
                {
                    return NoContent();
                }
                else
                {
                    return Ok(result.ToUserOutputDtoMap());
                }
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    "Id: " + id,
                    ex.Message);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Получение пользователя по имени.
        /// </summary>
        /// <param name="name">Идентификатор пользователя</param>
        /// <returns>Пользователь с указанным идентификатором</returns>
        /// <response code="200">Найденный пользователь</response>
        /// <response code="204">Пользователь с указанным именем не найден</response>
        /// <response code="500">Пользователь с указанным идентификатором не найден</response> 
        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUserByName(string name)
        {
            try
            {
                var result = await userService.GetUserByName(name);
                if (result == null)
                {
                    return NoContent();
                }
                else
                {
                    return Ok(result.ToUserOutputDtoMap());
                }
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    "Name: " + name,
                    ex.Message);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Добавление пользователя
        /// </summary>
        /// <param name="userInputDto">Класс пользователя приходящий с веб</param>
        /// <returns>Созданный пользователь</returns>
        /// <response code="200">Созданный пользоваель</response>
        /// <response code="500">Ошибка при создании пользователя</response> 
        
        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserResponse>> PostUser(UserRequest userInputDto)
        {
            try
            {
                var user = await userService.AddUser(userInputDto.ToUserMap());
                return Ok(user.ToUserOutputDtoMap());
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    JsonSerializer.Serialize(userInputDto),
                    ex.Message);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <response code="200">Пользователь удален</response> 
        /// <response code="500">Ошибка при удалении пользователя</response> 
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await userService.DeleteUserById(id);
                return Ok();
            } catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    "Id: " + id,
                    ex.Message);
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Изменение имени пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="userInputDto">Класс пользователя приходящий с веб</param>
        /// <returns>Изменный пользователь</returns>
        /// <response code="200">Изменный пользователь</response> 
        /// <response code="500">Ошибка при изменении имени пользователя</response> 
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("EditUserName/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult<UserResponse>> PutUserName(int id, UserRequest userInputDto)
        {
            User editedUser;
            try
            {
               editedUser  = await userService.EditUserName(id, userInputDto.ToUserMap());
            } catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    "Id: " + id + ", " + JsonSerializer.Serialize(userInputDto),
                    ex.Message);
                return Problem(ex.Message);
            }
            return Ok(editedUser.ToUserOutputDtoMap());
        }

        /// <summary>
        /// Изменение пароля пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="userInputDto">Класс пользователя приходящий с веб</param>
        /// <returns>Изменный пользователь</returns>
        /// <response code="200">Изменный пользователь</response> 
        /// <response code="500">Ошибка при изменении пароля пользователя</response> 
        
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPut("EditUserPassword/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult<UserResponse>> PutUserPassword(int id, UserRequest userInputDto)
        {
            User editedUser;
            try
            {
                editedUser = await userService.EditUserPassword(id, userInputDto);
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    JsonSerializer.Serialize(userInputDto),
                    ex.Message);
                return Problem(ex.Message);
            }
            return Ok(editedUser.ToUserOutputDtoMap());
        }

        /// <summary>
        /// Проверка имени пользователя и пароля
        /// </summary>
        /// <param name="userRequest">Класс пользователя приходящий с веб</param>
        /// <response code="200">Имя пользователя и пароль корректны</response>
        /// <response code="500">Неверное имя пользователя или пароль</response> 

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("IsUsernameAndPasswordCorrect")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> IsUsernameAndPasswordCorrect(UserRequest userRequest)
        {
            try
            {
                await userService.IsUsernameAndPasswordCorrect(userRequest.Name, userRequest.Password);
                return Ok();
            } catch (Exception ex)
            {
                _errorLogger.LogError(System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    JsonSerializer.Serialize(userRequest),
                    ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
