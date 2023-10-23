namespace ExpenseAndPointServer.Services.Cryptographer
{
    /// <summary>
    /// Интерфейс шифровщика пароля
    /// </summary>
    public interface ICryptographer
    {
        /// <summary>
        /// Зашифровать пароль
        /// </summary>
        /// <param name="str">Пароль</param>
        /// <returns>Зашифрованный пароль</returns>
        public string Encrypt(string str);
    }
}
