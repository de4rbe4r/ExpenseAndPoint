using System.Text.RegularExpressions;

namespace ExpenseAndPointServer.Services.PasswordChecker
{
    public class SimplePasswordChecker : IPasswordChecker
    {
        public bool IsStrengthPassword(string password)
        {
            // В регулярных выражениях
            // \d - соответствие на любую десятичнуюю цифру
            // + - встречается один и более раз
            if (Regex.Match(password, @"\d+", RegexOptions.ECMAScript).Success
                && (Regex.Match(password, @"[a-z]", RegexOptions.ECMAScript).Success || Regex.Match(password, @"[а-я]", RegexOptions.ECMAScript).Success)
                && (Regex.Match(password, @"[A-Z]", RegexOptions.ECMAScript).Success || Regex.Match(password, @"[А-Я]", RegexOptions.ECMAScript).Success)
                && Regex.Match(password, @".[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript).Success
                && password.Length >= 8
                ) return true;
            else return false;
        }
    }
}
