using ExpenseAndPoint.Data;
using ExpenseAndPointServer.Authentication;
using ExpenseAndPointServer.Models.Users;
using ExpenseAndPointServer.Services;
using ExpenseAndPointServer.Services.Cryptographer;
using ExpenseAndPointServer.Services.PasswordChecker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ExpenseAndPointServer.Controllers
{
    /// <summary>
    /// Контроллер работы с аутенфикацией
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        /// <summary>
        /// Сервис для работы с пользователями
        /// </summary>
        private readonly UserService _userservice;

        /// <summary>
        /// Конструктор создания контроллера
        /// </summary>
        /// <param name="context">Контекст для работы с БД</param>
        /// <param name="cryptographer">Сервис шифрования пароля</param>
        /// <param name="passwordChecker">Сервис проверки надежности пароля</param>
        public AuthenticationController(AppDbContext context, ICryptographer cryptographer, IPasswordChecker passwordChecker)
        {
            _userservice = new UserService(context, cryptographer, passwordChecker);
        }

        /// <summary>
        /// Аутенфикация пользователя
        /// </summary>
        /// <param name="userRequest">Модель пользователя, приходящего с веб</param>
        /// <returns>JWT токен, идентификатор пользователя</returns>
        [HttpPost]
        public async Task<ActionResult<User>> AuthUser(UserRequest userRequest)
        {
            List<Claim> identity;
            try
            {
                identity = await GetIdentity(userRequest);
            } catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            var now = DateTime.UtcNow;

            // Создаем JWT-токен
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDINCE,
                notBefore: now,
                claims: identity,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                userId = identity[0].Value.ToString(),
                userName = identity[1].Value.ToString()
            };
            return Ok(response);
        }

        /// <summary>
        /// Идентификация пользователя
        /// </summary>
        /// <param name="userRequest">Модель пользователя, приходящего с веб</param>
        /// <returns>Список Claim</returns>
        private async Task<List<Claim>> GetIdentity(UserRequest userRequest)
        {
            await _userservice.IsUsernameAndPasswordCorrect(userRequest.Name, userRequest.Password);
            var user = await _userservice.GetUserByName(userRequest.Name);
            var claims = new List<Claim> { 
                new Claim(nameof(user.Id), $"{user.Id}"),
                new Claim(nameof(user.Name), $"{user.Name}")
            };
            return claims;
        }
    }
}
