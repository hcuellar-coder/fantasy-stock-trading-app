import React, { useState } from 'react';
import PrivateRoute from './PrivateRoute';
import Home from './Components/Home';
import Summary from './Components/Summary';
import Report from './Components/Report';
import Layout from './Components/Layout';
import { Route } from 'react-router';
import { AuthContext } from './Context/AuthContext';
import { UserContext } from './Context/UserContext';
import { AccountContext } from './Context/AccountContext';
import { HoldingsContext } from './Context/HoldingsContext';
import './App.css';


function App() {
    const existingTokens = JSON.parse(sessionStorage.getItem('tokens'));
    const existingUser = JSON.parse(sessionStorage.getItem('user'));
    const existingAccount = JSON.parse(sessionStorage.getItem('account'));
    const existingHoldings = JSON.parse(sessionStorage.getItem('holdings'));
    const [authTokens, setAuthTokens] = useState(existingTokens);
    const [user, setUser] = useState(existingUser);
    const [account, setAccount] = useState(existingAccount);
    const [holdings, setHoldings] = useState(existingHoldings);

    const setTokens = (data) => {
        sessionStorage.setItem('tokens', JSON.stringify(data));
        setAuthTokens(data);
    }

    const setUserContext = (data) => {
        sessionStorage.setItem('user', JSON.stringify(data));
        setUser(data);
    }
    const setAcountContext = (data) => {
        sessionStorage.setItem('account', JSON.stringify(data));
        setAccount(data);
    }
    const setHoldingsContext = (data) => {
        sessionStorage.setItem('holdings', JSON.stringify(data));
        setHoldings(data);
    }

    return (
        <AuthContext.Provider value={{ authTokens, setAuthTokens: setTokens }}>
            <UserContext.Provider value={{ user, setUser: setUserContext }}>
                <AccountContext.Provider value={{ account, setAccount: setAcountContext }}>
                    <HoldingsContext.Provider value={{ holdings, setHoldings: setHoldingsContext }}>
                        <div>
                          <Layout >
                            <Route exact path='/' component={Home} />
                            <PrivateRoute path='/summary' component={Summary} />
                            <PrivateRoute path='/report' component={Report} />
                          </Layout>
                        </div>
                        </HoldingsContext.Provider>
                    </AccountContext.Provider>
            </UserContext.Provider>
        </AuthContext.Provider>
    )
}

export default App;
