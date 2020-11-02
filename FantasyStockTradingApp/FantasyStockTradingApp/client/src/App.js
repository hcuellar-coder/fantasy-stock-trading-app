import React, { useState } from 'react';
import PrivateRoute from './PrivateRoute';
import Home from './Components/Home';
import Summary from './Components/Summary';
import Report from './Components/Report';
import Layout from './Components/Layout';
import { Route } from 'react-router';
import { AuthContext } from './Context/Auth';
import './App.css';


function App() {
    const existingTokens = JSON.parse(localStorage.getItem('tokens'));
    const [authTokens, setAuthTokens] = useState(existingTokens);

    const setTokens = (data) => {
        localStorage.setItem('tokens', JSON.stringify(data));
        setAuthTokens(data);
    }

    return (
        <AuthContext.Provider value={{ authTokens, setAuthTokens: setTokens }}>
            <div>
              <Layout >
                <Route exact path='/' component={Home} />
                <PrivateRoute path='/summary' component={Summary} />
                <PrivateRoute path='/report' component={Report} />
              </Layout>
            </div>
        </AuthContext.Provider>
    )
}

export default App;
