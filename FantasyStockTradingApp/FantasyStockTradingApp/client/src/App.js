import React, { useState } from 'react';
import PrivateRoute from './PrivateRoute';
import Home from './Components/Home';
import Summary from './Components/Summary';
import Report from './Components/Report';
import Layout from './Components/Layout';
import { Route } from 'react-router';
import { AuthContext } from './Context/AuthContext';
import { UserContext } from './Context/UserContext';
import './App.css';


function App() {
    const existingTokens = JSON.parse(sessionStorage.getItem('tokens'));
    const existingUser = JSON.parse(sessionStorage.getItem('userAccount'));
    const [authTokens, setAuthTokens] = useState(existingTokens);
    const [userAccount, setUserAccount] = useState(existingUser);

    const setTokens = (data) => {
        sessionStorage.setItem('tokens', JSON.stringify(data));
        setAuthTokens(data);
    }

    const setUser = (data) => {
        let userData = JSON.stringify(data);
        console.log('userData = ', userData);
        console.log('in setUser, data =', data);
        sessionStorage.setItem('userAccount', userData);
        setUserAccount(data);
    }

    return (
        <AuthContext.Provider value={{ authTokens, setAuthTokens: setTokens }}>
            <UserContext.Provider value={{ userAccount, setUserAccount: setUser }}>
                <div>
                  <Layout >
                    <Route exact path='/' component={Home} />
                    <PrivateRoute path='/summary' component={Summary} />
                    <PrivateRoute path='/report' component={Report} />
                  </Layout>
                </div>
            </UserContext.Provider>
        </AuthContext.Provider>
    )
}

export default App;
