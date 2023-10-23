namespace ExpenseAndPointServer.Services.PasswordChecker
{
    /// <summary>
    /// Интерфейс класса проверки пароля
    /// </summary>
    public interface IPasswordChecker
    {
        /// <summary>
        /// Проверить надежность пароля
        /// </summary>
        /// <param name="password">Пароль</param>
        public void CheckStrengthPassword(string password);
    }
}
