const BASE_URL = 'https://localhost:7184';
const USERS = '/api/users'

// Authentication
export const urlAuth = BASE_URL + '/api/Authentication'

// Users
export const isUsernameAndPasswordCorrectUrl = BASE_URL + USERS + '/IsUsernameAndPasswordCorrect'
export const RegisterUser = BASE_URL + USERS;