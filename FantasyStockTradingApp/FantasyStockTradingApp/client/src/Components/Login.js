import React, { useState, useEffect } from 'react';
import { Redirect } from 'react-router-dom';
import { Form, Button, Container, NavLink } from 'react-bootstrap';
import { api, tokenApi, iexApi } from './API';
import { useAuth } from '../Context/AuthContext';
import { useUser } from '../Context/UserContext';
import { useAccount } from '../Context/AccountContext';
import { useHoldings } from '../Context/HoldingsContext';

function Login(props) {
    const [isError, setIsError] = useState(false);
    const [validated, setValidated] = useState(false);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const { setAuthTokens } = useAuth();
    const { setUser } = useUser();
    const { account, setAccount } = useAccount();
    const { holdings, setHoldings } = useHoldings();

    function isValidUser() {
        try {
            const response = api.get('/is_valid_user?', {
                params: {
                    Email: email.toLowerCase(),
                    Password: password
                }
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function getUser() {
        try {
            const response = api.get('/get_user?', {
                params: {
                    Email: email.toLowerCase(),
                }
            });
            return response;
        } catch(error) {
            console.error(error);
        }
    }

    function getAccount(userID) {
        console.log('userId', userID);
        try {
            const response = api.get('/get_account?', {
                params: {
                    UserId: userID
                }
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function getHoldings(accountID) {
        console.log('accountID', accountID);
        try {
            const response = api.get('/get_holdings?', {
                params: {
                    AccountId: accountID
                }
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function getToken() {
        try {
            const response = tokenApi.get('/get_token?', {
                params: {
                    Email: email.toLowerCase(),
                    Password: password
                }
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    } 

    function get_quote(symbol) {
        try {
            const response = iexApi.get('/get_quote', {
                params: {
                    Symbol: symbol
                }
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function update_holding(holding, stockCount) {
        console.log('holding = ', holding);
        console.log('account = ', account);
        console.log('stockCount = ', stockCount);
        try {
            const response = api.post('/update_holding', {
                AccountId: account.id,
                CompanyName: holding.companyName,
                Symbol: holding.symbol,
                StockCount: stockCount,
                LatestCostPerStock: holding.latestPrice,
                Change: holding.change,
                ChangePercentage: holding.changePercent,
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function updateHoldings() {
        for (let holding of holdings) {
            console.log('holding =', holding);
            get_quote(holding.symbol).then((getQuoteResponse) => {
                if (getQuoteResponse.status === 200) {
                    console.log('getQuoteResponse.data = ', getQuoteResponse.data);
                    let stockCount = searchHoldings(holding.symbol);
                    update_holding(getQuoteResponse.data, stockCount).then((updateHoldingResponse) => {
                        console.log(updateHoldingResponse);
                    });
                }
            });
        }
    }

    function searchHoldings(symbol) {
        console.log('symbol =', symbol);
        console.log('holdings =', holdings);
        for (let holding of holdings) {
            console.log('holding.symbol =', holding.symbol);
            if (holding.symbol === symbol) {
                console.log('holding.stock_Count = ', holding.stock_Count);
                return holding.stock_Count;
            }
        }
    }

    function handleEmailChange(e) {
        e.preventDefault();
        setEmail(e.target.value);
    }

    function handlePasswordChange(e) {
        e.preventDefault();
        setPassword(e.target.value);
    }

    function handleSubmit(e) {
        e.preventDefault();
       const form = e.target;
        if (form.checkValidity() !== false) {
            isValidUser().then((isValidUserResponse) => {
                console.log('isValidUser =', isValidUserResponse);
                if (isValidUserResponse.data) {
                    getUser().then((getUserResponse) => {
                        if (getUserResponse.status === 200 && getUserResponse.data.length !== 0) {
                            setUser(getUserResponse.data[0]);
                            console.log('getUserResponse.data =', getUserResponse.data);
                            console.log('getUserResponse.data[0] =', getUserResponse.data[0]);

                            getAccount(getUserResponse.data[0].id).then((getAccountResponse) => {
                                if (getAccountResponse.status === 200 && getAccountResponse.data.length !== 0) {
                                    console.log('getAccountResponse = ', getAccountResponse);
                                    console.log('getAccountResponse[0] = ',getAccountResponse[0]);
                                    setAccount(getAccountResponse.data[0]);

                                    getHoldings(getAccountResponse.data[0].id).then((getHoldingsResponse) => {
                                        if (getHoldingsResponse.status === 200) {
                                            setHoldings(getHoldingsResponse.data);

                                            getToken().then((getTokenResponse) => {
                                                if (getTokenResponse.status === 200) {
                                                    setAuthTokens(getTokenResponse.data);
                                                }
                                            });
                                        }
                                    });
                                }
                            });
                        }
                    });
                } else {
                    setIsError(true);
                }
            }).catch(e => {
                setIsError(true);
            });
            console.log('data is valid');
        }
        setValidated(true);
    }

    function handleNewUser(e) {
        props.login(false);
        console.log('handle new user information');
    }

    useEffect(() => {
        if (isError) {
            setEmail('');
            setPassword('');
            setIsError(false);
        }
    }, [isError])

    useEffect(() => {
        if (holdings) {
            updateHoldings();
        }
    },[holdings])

    return (
        <Container id="login-container">
            <Form className="login-form" noValidate validated={validated} onSubmit={handleSubmit}>
                <Form.Row className="login-row">
                    <Form.Group>
                        <Form.Control
                            required
                            type='text'
                            placeholder="Email"
                            onChange={handleEmailChange}
                            value={email}
                        />
                    </Form.Group>
                </Form.Row>
                <Form.Row className="login-row">
                    <Form.Group>
                        <Form.Control
                            required
                            type='password'
                            placeholder="Password"
                            onChange={handlePasswordChange}
                            value={password}
                        />
                        <Form.Control.Feedback type="invalid">
                            Incorrect username or password.
                        </Form.Control.Feedback>
                    </Form.Group>
                </Form.Row>
                <div id='login-buttons-div'>
                    <Button id="login-user-button" type="submit">Login</Button>
                    <div id='new-user-login-div'>
                        Don't have an account?
                        <NavLink id="new-user-button" onClick={handleNewUser}>Sign up</NavLink>
                    </div>
                </div>
            </Form>
        </Container>
    )
}

export default Login;