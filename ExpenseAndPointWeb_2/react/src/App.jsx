import React from 'react';
import './App.css';
import { Routes, Route } from "react-router-dom";
import Authorization from './Components/Authorization';
import Registration from './Components/Registration';
import NavMenu from './Components/NavMenu';
import 'bootstrap/dist/css/bootstrap.min.css';
import Cookies from 'universal-cookie';


function App() {
    const cookies = new Cookies();

    return (
        <div>
            <Routes>
                <Route path='/auth' element={<Authorization />} />
                <Route path='/register' element={<Registration />} />
            </Routes>
            {(window.location.href !== 'http://localhost:5173/auth' && window.location.href !== 'http://localhost:5173/register') ? <NavMenu /> : null}
            {(
                (cookies.get('access_token') === "") && (window.location.href !== 'http://localhost:5173/auth' && window.location.href !== 'http://localhost:5173/register')
            ) ? window.location.replace('http://localhost:5173/auth') : null}
            {(
                (cookies.get('access_token') !== "") && (window.location.href === 'http://localhost:5173/auth' || window.location.href === 'http://localhost:5173/register')
            ) ? window.location.replace('http://localhost:5173/') : null}
        </div>
    );
}

export default App;
