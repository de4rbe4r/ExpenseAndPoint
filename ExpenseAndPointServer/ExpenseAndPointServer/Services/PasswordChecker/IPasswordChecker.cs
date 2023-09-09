namespace ExpenseAndPointServer.Services.PasswordChecker
{
    public interface IPasswordChecker
    {
        public bool IsStrengthPassword(string password);
    }
}
