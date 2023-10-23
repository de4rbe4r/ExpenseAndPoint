using System.Text.RegularExpressions;

namespace ExpenseAndPointServer.Services.PasswordChecker
{
    /// <summary>
    /// Класс простой проверки пароля
    /// </summary>
    public class SimplePasswordChecker : IPasswordChecker
    {
        /// <summary>
        /// Проверить надежность пароля
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <exception cref="Exception">Ошибка, связанная с ненадежностью пароля</exception>
        public void CheckStrengthPassword(string password)
        {
            // В регулярных выражениях
            // \d - соответствие на любую десятичнуюю цифру
            // + - встречается один и более раз
            if (Regex.Match(password, @"\d+", RegexOptions.ECMAScript).Success
                && (Regex.Match(password, @"[a-z]", RegexOptions.ECMAScript).Success || Regex.Match(password, @"[а-я]", RegexOptions.ECMAScript).Success)
                && (Regex.Match(password, @"[A-Z]", RegexOptions.ECMAScript).Success || Regex.Match(password, @"[А-Я]", RegexOptions.ECMAScript).Success)
                && Regex.Match(password, @".[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript).Success
                && password.Length >= 8
                ) return;
            throw new Exception("Пароль должен содержать буквы верхнего и нижнего регистра," +
               "хотя бы одну цифру и один специальный символ");
        }
    }
}
