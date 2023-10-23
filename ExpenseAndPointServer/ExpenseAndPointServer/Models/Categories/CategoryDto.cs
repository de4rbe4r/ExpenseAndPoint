namespace ExpenseAndPointServer.Models.Categories
{
    /// <summary>
    /// Класс категории для отправления на веб
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// Идентификатор категории
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Преобразование класса CategoryDro в Category
        /// </summary>
        /// <returns>Класс Category для взаимодействия с БД</returns>
        public Category ToCategoryMap()
        {
            return new Category
            {
                Id = Id,
                UserId = UserId,
                Title = Title
            };
        }
    }
}
