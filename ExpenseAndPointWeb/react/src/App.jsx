import React from 'react';
import './App.css';
import { Routes, Route } from "react-router-dom";
import Authorization from './Components/Authorization';
import Registration from './Components/Registration';
import Month from './Components/NavMenuTabs/Month';
import Period from './Components/NavMenuTabs/Period';
import Tools from './Components/NavMenuTabs/Tools';
import 'bootstrap/dist/css/bootstrap.min.css';
import Cookies from 'universal-cookie';
import Day from './Components/NavMenuTabs/Day';



function App() {
    const cookies = new Cookies();

    return (
        <div className="App">

            {(
                (cookies.get('access_token') === undefined || cookies.get('access_token') === '')) ?
                (
                    (window.location.href !== 'http://localhost:5173/auth' && window.location.href !== 'http://localhost:5173/register') ?
                    window.location.replace('http://localhost:5173/auth') : null
                ) : 
                (
                    (window.location.href === 'http://localhost:5173/auth' || window.location.href === 'http://localhost:5173/register') ?
                        window.location.replace('http://localhost:5173/') : null
                )
                }
            <Routes>
                <Route path='/auth' element={<Authorization />} />
                <Route path='/register' element={<Registration />} />
                <Route path='/' element={<Day />} />
                <Route path='/month' element={<Month />} />
                <Route path='/period' element={<Period/>} />
                <Route path='/tools' element={<Tools/>} />
            </Routes>
        </div>
    );
}

export default App;
