# Расходы и точка

Расходы и точка - это веб-приложение для учета личных расходов. С помощью данного приложения вы можете следить за своими финансами и контролировать свои расходы.

---
# Общая информация о приложении
## Регистрация и авторизация
Для получения доступа к приложению пользователю необходимо пройти этап регистрации и авторизации. Если пользователь не будет авторизован, то основные страницы приложения, в том числе домашная страница, будет недоступна. Неавторизованному пользователю доступны 2 страницы: авторизация и регистрация.
<div align="center"><img src="https://github.com/de4rbe4r/ExpenseAndPoint/blob/main/Images/01.PNG" width="700"/></div>

### Регистрация
Для регистрации пользователя необходимо ввести имя пользователя и пароль. При успешной регистрации пользователь будет перенаправлен на страницу входа.
<div align="center"><img src="https://github.com/de4rbe4r/ExpenseAndPoint/blob/main/Images/02.PNG" width="700"/></div>

Возможные ошибки при нажатии на кнопку зарегистрироваться:
#### Логин и пароль не введен
<div align="center"><img src="https://github.com/de4rbe4r/ExpenseAndPoint/blob/main/Images/03.PNG" width="700"/></div>

#### Введенные пароли не совпадают
<div align="center"><img src="https://github.com/de4rbe4r/ExpenseAndPoint/blob/main/Images/04.PNG" width="700"/></div>

#### Пользователь с указанным именем уже зарегестрирован
<div align="center"><img src="https://github.com/de4rbe4r/ExpenseAndPoint/blob/main/Images/05.PNG" width="700"/></div>

#### Введенный пароль не соответствует требования безопасности
<div align="center"><img src="https://github.com/de4rbe4r/ExpenseAndPoint/blob/main/Images/06.PNG" width="700"/></div>

### Авторизация
Для входа в приложение необходимо ввести логин и пароль зарегистрированного пользователя.
<div align="center"><img src="https://github.com/de4rbe4r/ExpenseAndPoint/blob/main/Images/07.PNG" width="700"/></div>

При успешной авторизации пользователь будет перенаправлен на главную страницу приложения. Информация получения в результате регистрации (JWT токен, имя пользователя, Id пользователя) записываются и хранятся в Cookies. Если зарегистрированный пользователь попытается перейти к страницам авторизации или регистрации, то он будет перенаправлен на домашнюю страницу.

Возможные ситуации при нажатии на кнопку войти:
#### Введено неверное имя пользователя или пароль
<div align="center"><img src="https://github.com/de4rbe4r/ExpenseAndPoint/blob/main/Images/08.PNG" width="700"/></div>

## Основной функционал приложения
Приложение состоит из вкладок:
- День: информация о суточных расходах
- Месяц: информация о расходах с начала месяца
- Период: информация о расходах за указанный период
- Настройки: изменение имени пользователя, пароля, работа с категориями.
- Кнопка выхода из приложения: при нажатии на кнопку данные в Cookies будут удалены.

На вкладках день, месяц, период находятся кнопки, с помощью которых происходит добавление расхода или категории.
<div align="center"><img src="https://github.com/de4rbe4r/ExpenseAndPoint/blob/main/Images/11.PNG" width="700"/></div>

### Добавить категорию
При нажатии на кнопку "Добавить категорию" отобразится модальное окно с вводом названия категории.
<div align="center"><img src="https://github.com/de4rbe4r/ExpenseAndPoint/blob/main/Images/12.PNG" width="700"/></div>

При возникновении какой-либо ошибки отобразится модальное окно с текстом ошибки.
<div align="center"><img src="https://github.com/de4rbe4r/ExpenseAndPoint/blob/main/Images/13.PNG" width="700"/></div>

При успешном добавлении категории отобразится модальное окно.
<div align="center"><img src="https://github.com/de4rbe4r/ExpenseAndPoint/blob/main/Images/14.PNG" width="700"/></div>

### Добавить расход
При нажатии на кнопку "Добавить расход" отобразится модальное окно для ввода параметров. На данной форме представлен выбор категории из выпадающего списка, выбор даты и времеи расхода (имеется возможность добавления расхода за прошедшее время).


### Вкладка день
<div align="center"><img src="https://github.com/de4rbe4r/ExpenseAndPoint/blob/main/Images/09.PNG" width="700"/></div>

На данной вкладке представлен список расходов, который позволяет изменять и удалять информация о расходе. Для этого необходимо нажать ПКМ по интересующему расходу и выбрать действие.
<div align="center"><img src="https://github.com/de4rbe4r/ExpenseAndPoint/blob/main/Images/08.PNG" width="700"/></div>

Lkz 
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

### История изменения расходов 
- Id - уникальный идентификатор: int, РК
- DateCreated - дата создания записи: DateTime, NOT NULL
- UserId - идентификатор польователя: int, FK User(Id)
- ActionType - тип действия: int, NOT NULL
- NewAmount - новая сумма расходов: float, NOT NULL
- NewDateTime - новая датаи и время: DateTime, NOT NULL
- NewCategoryTitle - новое название категории: string, NOT NULL
- OldAmount - старая сумма расходов: float
- OldDateTime - старая дата и время: DateTime
- OldCategoryTitle - старое название категории: string

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
- Хранение истории добавления, изменения, удаления расходов

## Реализованные особенности на стороне backend
- Передача данных DTO
- Использование маппинга данных (предпочтительнее ручной)
- Документирование Swagger
- Логирование ошибок в файл

- СДЕЛАТЬ! Использование JWT токенов
