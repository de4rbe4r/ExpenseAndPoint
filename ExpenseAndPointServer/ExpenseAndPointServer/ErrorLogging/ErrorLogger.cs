namespace ExpenseAndPointServer.ErrorLogging
{
    /// <summary>
    /// Логировщик ошибок
    /// </summary>
    public class ErrorLogger
    {
        /// <summary>
        /// Путь хранения файла
        /// </summary>
        private const string path = "_logs.txt";

        /// <summary>
        /// Логирование ошибки
        /// </summary>
        /// <param name="methodName">Название метода</param>
        /// <param name="request">Текст запроса</param>
        /// <param name="errorMessage">Текст ошибки</param>
        public async void LogError(string methodName, string request, string errorMessage)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, append: true))
                {
                    sw.WriteLine("------------------------------");
                    sw.WriteLine("Дата и время: " + DateTime.Now);
                    sw.WriteLine("Вызываемый метод: " + methodName);
                    sw.WriteLine("Передаваемый запрос: " + request);
                    sw.WriteLine("Текст ошибки: " + errorMessage);
                }
            } catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
