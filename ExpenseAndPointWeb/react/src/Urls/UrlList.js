const BASE_URL = 'https://localhost:7184';
const USERS = '/api/users'
const CATEGORIES = '/api/categories'
const EXPENSE = '/api/expense';

// Authentication
export const urlAuth = BASE_URL + '/api/Authentication'

// Users
export const isUsernameAndPasswordCorrectUrl = BASE_URL + USERS + '/IsUsernameAndPasswordCorrect'
export const RegisterUserUrl = BASE_URL + USERS;

// Categories
export const AddCategoryUrl = BASE_URL + CATEGORIES;
export const GetCategoryByIdUrl = BASE_URL + CATEGORIES + '/ByUserId/';

// Expenses
export const AddExpensesUrl = BASE_URL + EXPENSE;
export const GetExpensesListByUserIdAndDateUrl = BASE_URL + EXPENSE + '/ByUserIdAndDate';
export const EditExpenseUrl = BASE_URL + EXPENSE + '/';
export const DeleteExpenseUrl = BASE_URL + EXPENSE + '/';