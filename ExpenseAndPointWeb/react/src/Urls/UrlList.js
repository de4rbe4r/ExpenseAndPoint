const BASE_URL = 'https://localhost:7184';
const USERS = '/api/users'
const CATEGORIES = '/api/categories'
const EXPENSE = '/api/expense';
const EXPENSEHISTORY = '/api/expensehistory'

// Authentication
export const urlAuth = BASE_URL + '/api/Authentication'

// Users
export const isUsernameAndPasswordCorrectUrl = BASE_URL + USERS + '/IsUsernameAndPasswordCorrect'
export const RegisterUserUrl = BASE_URL + USERS;
export const EditUserNameUrl = BASE_URL + USERS + '/EditUserName/';
export const EditUserPasswordUrl = BASE_URL + USERS + '/EditUserPassword/';

// Categories
export const AddCategoryUrl = BASE_URL + CATEGORIES;
export const GetCategoryByIdUrl = BASE_URL + CATEGORIES + '/ByUserId/';
export const EditCategoryUrl = BASE_URL + CATEGORIES + '/';
export const DeleteCategoryUrl = BASE_URL + CATEGORIES + '/';

// Expenses
export const AddExpensesUrl = BASE_URL + EXPENSE;
export const GetExpensesListByUserIdAndDateUrl = BASE_URL + EXPENSE + '/ByUserIdAndDate';
export const GetExpensesListByUserIdAndPeriodUrl = BASE_URL + EXPENSE + '/ByUserIdAndPeriod';
export const EditExpenseUrl = BASE_URL + EXPENSE + '/';
export const DeleteExpenseUrl = BASE_URL + EXPENSE + '/';

// ExpensesHistory
export const GetExpensesHistoryListByUserId = BASE_URL + EXPENSEHISTORY + '/ByUserId/';
