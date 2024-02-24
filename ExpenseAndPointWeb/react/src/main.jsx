import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import './index.css';
import { BrowserRouter } from 'react-router-dom';
import NavMenu from './Components/NavMenuTabs/NavMenu';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
    <React.StrictMode>
        <BrowserRouter>
            {(window.location.href !== 'http://localhost:5173/auth' && window.location.href !== 'http://localhost:5173/register') ? <NavMenu /> : null}
            <App />
        </BrowserRouter>
    </React.StrictMode>
);
