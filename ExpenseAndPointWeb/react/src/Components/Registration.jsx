import { React, useState } from 'react';
import '../Styles/Authorization.css';
import { RegisterUserUrl } from "../Urls/UrlList";
import axios from 'axios';

const Authorization = () => {
    const [showError, setShowError] = useState(false);
    const [user, setUser] = useState({
        name: '',
        password: '',
        confirmedPassword: ''
    });
    const [errorMessage, setErrorMessage] = useState(null);

    const Register = (event) => {
        event.preventDefault();

        try {
            if (user.name === "" || user.password === "") {
                throw new Error("Введите имя пользователя и пароль");
            }

            if (user.password !== user.confirmedPassword) {
                throw new Error("Пароли не совпадают");
            }

            axios.post(RegisterUserUrl, user)
                .then(res => {
                    window.location.replace('http://localhost:5173/auth')
                })
                .catch(function (error) {
                    if (error.response) {
                        setShowError(true);
                        setErrorMessage(error.response.data.detail);
                    }
                });

        } catch (error) {
            setShowError(true);
            setErrorMessage(error.message);
        }
    }

    const ErrorMessage = () => {
        return (
            <div className="err">
                {errorMessage}
            </div>
        )
    }

    return (
        <>
            {showError ? <ErrorMessage /> : null}

            <div className="formdiv">
                <form>
                    <input className="input" type="text" placeholder="Логин"
                        value={user.name} onChange={event => setUser({ ...user, name: event.target.value })}></input>
                    <input className="input" type="password" placeholder="Пароль"
                        value={user.password} onChange={event => setUser({ ...user, password: event.target.value })}></input>
                    <input className="input" type="password" placeholder="Подтвердите пароль"
                        value={user.confirmedPassword} onChange={event => setUser({ ...user, confirmedPassword: event.target.value })}></input>
                </form>
                <button className="button"
                    onClick={ Register }>Зарегистрироваться</button>
            </div>
        </>
    );
};

export default Authorization;