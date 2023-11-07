import React from 'react';
import './App.css';
import { Routes, Route } from "react-router-dom";
import Authorization from './Components/Authorization';

function App() {
    return (
        <div>
            <Routes>
                <Route path='/auth' element={<Authorization />} />
            </Routes>
        </div>
    );
}

export default App;
