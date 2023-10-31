# Расходы и точка

Расходы и точка - это веб-приложение для учета личных расходов. С помощью данного приложения вы можете следить за своими финансами и контролировать свои расходы.

---
# Раздел для разработчика
Данное приложение написано на языке C# (backend) и ReactJS (frontend)

## Используемые модели 
### Расход
- Id - уникальный идентификатор: int, PK
- Amount - сумма: float, NOT NULL
- DateTime - дата и время: DateTime, default(DateTime.Now), NOT NULL
- CategoryId - идентификатор категории: int, FK Category(Id)
- UserId - идентификатор пользователя: int, FK User(Id)
- Category - категория
- User - пользователь

### Категория
- Id - уникальный идентификтор: int, PK
- Title - название категории: string, NOT NULL
- Expenses - список расходов: ICollection<Expense>

### Пользователь
- Id - уникальный идентификатор: int, PK
- Name - имя пользователя: sting, minlenght(4), должно начинаться с буквы и не должно содержать специальных знаков
- Password - пароль: string, minlenght(8), должен содержать строчные и заглавные буквы и специальные символы
- Expenses - список расходов: ICollection<Expense>

## База данных
- MS SQl
### Реализованные особенности для моделей
- Каскадное удаление расходов при удалении пользователя
- Запрет удаления категории при наличии расходов

- СДЕЛАТЬ! Хранение истории добавлений, изменений, удалений расходов

## Реализованные особенности на стороне backend
- Передача данных DTO
- Использование маппинга данных (предпочтительнее ручной)
- Документирование Swagger
- Логирование ошибок в файл

- СДЕЛАТЬ! Использование JWT токенов
