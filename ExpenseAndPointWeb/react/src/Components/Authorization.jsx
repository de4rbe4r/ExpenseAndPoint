﻿import { React, useState } from 'react';
import '../Styles/Authorization.css';
import { urlAuth } from "../Urls/UrlList";
import axios from 'axios';
import Cookies from 'universal-cookie';

const Authorization = () => {
    const [showError, setShowError] = useState(false);
    const cookies = new Cookies();
    const [user, setUser] = useState({
        name: '',
        password: ''
    });
    const [errorMessage, setErrorMessage] = useState(null);

    const cookiesLive = 604800; // Неделя

    const SignIn = (event) => {
        event.preventDefault();

        try {
            if (user.name === "" || user.password === "") {
                throw new Error("Введите имя пользователя и пароль");
            }

            axios.post(urlAuth, user)
                .then(res => {
                    cookies.set('access_token', res.data.access_token, { path: '/', maxAge: cookiesLive });
                    cookies.set('userId', res.data.userId, { path: '/', maxAge: cookiesLive });

                    cookies.set('userName', res.data.userName, { path: '/', maxAge: cookiesLive });
                    window.location.replace('http://localhost:5173')
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
                {errorMessage }
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
            </form>
                <button className="button"
                    onClick={SignIn }>Войти</button>
                <a className="a" href="http://localhost:5173/register">Регистрация</a>
            </div>
        </>
    );
};

export default Authorization;