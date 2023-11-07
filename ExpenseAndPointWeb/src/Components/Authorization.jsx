import React from 'react';
import '../Styles/Authorization.css';

const Authorization = () => {

    return (
        <div class="container">
            <form>
                <input type="text" placeholder="Логин"></input>
                <input type="password" placeholder="Пароль"></input>
            </form>
        </div>
    );
};

export default Authorization;