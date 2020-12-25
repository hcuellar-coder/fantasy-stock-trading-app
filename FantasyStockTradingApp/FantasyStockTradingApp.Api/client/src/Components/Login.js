import React, { useState, useEffect } from 'react';
import { Form, Button, Container, NavLink, Image, Card } from 'react-bootstrap';
import { api, tokenApi } from './API';
import { useAuth } from '../Context/AuthContext';
import { useUser } from '../Context/UserContext';
import { useAccount } from '../Context/AccountContext';
import { useHoldings } from '../Context/HoldingsContext';
import loginLoading from '../loaders/Money-1.1s-200px.svg'

function Login(props) {
    const [error, setError] = useState('');
    const [isError, setIsError] = useState(false);
    const [validated, setValidated] = useState(false);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const { setAuthTokens } = useAuth();
    const { setUser } = useUser();
    const { setAccount } = useAccount();
    const { holdings, setHoldings } = useHoldings();

    async function isValidUser() {
        try {
            const response = await api.get('/is_valid_user?', {
                params: {
                    Email: email.toLowerCase(),
                    Password: password
                }
            });
            return response;
        } catch (error) {
            console.error(error);
            return error.response;
        }
    }

    async function getUser() {
        try {
            const response = await api.get('/get_user?', {
                params: {
                    Email: email.toLowerCase(),
                }
            });
            return response;
        } catch(error) {
            console.error(error);
            return error.response;
        }
    }

    async function getAccount(userID) {
        try {
            const response = await api.get('/get_account?', {
                params: {
                    UserId: userID
                }
            });
            return response;
        } catch (error) {
            console.error(error);
            return error.response;
        }
    }

    async function getHoldings(accountID) {
        try {
            const response = await api.get('/get_holdings?', {
                params: {
                    AccountId: accountID
                }
            });
            return response;
        } catch (error) {
            console.error(error);
            return error.response;
        }
    }

    async function getToken() {
        try {
            const response = await tokenApi.get('/get_token?', {
                params: {
                    Email: email.toLowerCase(),
                    Password: password
                }
            });
            return response;
        } catch (error) {
            console.error(error);
            return error.response;
        }
    } 

    async function update_holdings() {
        try {
            const response = await api.post('/update_holdings', {
                Holdings: holdings
            });
            return response;
        } catch (error) {
            console.error(error);
            return error.response;
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

    async function handleSubmit(e) {
        setValidated(true);
        setIsLoading(true);
        e.preventDefault();
       const form = e.target;
        if (form.checkValidity() !== false) {
            await isValidUser().then(async (isValidUserResponse) => {
                if (isValidUserResponse.data === true) {
                    await getUser().then(async (getUserResponse) => {
                        console.log('getUserResponse = ', getUserResponse);
                        if (getUserResponse.status === 200 && getUserResponse.data.length !== 0) {
                            setUser(getUserResponse.data[0]);

                            await getAccount(getUserResponse.data[0].id).then(async (getAccountResponse) => {
                                console.log('getAccountResponse = ', getAccountResponse);
                                if (getAccountResponse.status === 200 && getAccountResponse.data.length !== 0) {
                                    setAccount(getAccountResponse.data[0]);

                                    await getHoldings(getAccountResponse.data[0].id).then(async (getHoldingsResponse) => {
                                        console.log('getHoldingsResponse = ', getHoldingsResponse);
                                        if (getHoldingsResponse.status === 200) {
                                            setHoldings(getHoldingsResponse.data);

                                            await getToken().then(async (getTokenResponse) => {
                                                console.log('getTokenResponse = ', getTokenResponse);
                                                if (getTokenResponse.status === 200) {
                                                    setAuthTokens(getTokenResponse.data);
                                                } else {
                                                    setError(getTokenResponse.data.Message);
                                                    setIsError(true);
                                                }
                                            });
                                        } else {
                                            setError(getHoldingsResponse.data.Message);
                                            setIsError(true);
                                        }
                                    });
                                } else {
                                    setError(getAccountResponse.data.Message);
                                    setIsError(true);
                                }
                            });
                        } else {
                            setError(getUserResponse.data.Message);
                            setIsError(true);
                        }
                    });
                } else {
                    setError('Sorry, your password was incorrect. ' 
                            + ' Please double - check your password');
                    setIsError(true);
                    setIsLoading(false);
                }
            });
        }
    }

    function handleNewUser(e) {
        props.login(false);
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
            update_holdings();
        }
    },[holdings])

    return (
        <div>
            <Container>
                <Card id="login-signup-card">
                    <Card.Title id="login-signup-card-title"><h3>Welcome to Fantasy Stock Trader!</h3></Card.Title>
                    <Card.Body id="login-signup-card-body">
                        <h5>Ever wondered what it'd be like to manage a stock portfolio?</h5> Buying and selling stocks
                        can be a daunting endevor, especially with your money on the line! Well thats where the 
                        fantasy stock trader comes in to play. Here you can buy and sell stocks, even research the latest 
                        stock trends, all with fantasy money!
                    </Card.Body>
                </Card>
            </Container>
            {isLoading ?
                <Container id="login-loading-container" fluid><Image className="media-gif" src={loginLoading}/></Container>
                :
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
                                <Form.Text className="error-text">
                                    {error}
                                </Form.Text>
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
                }
            </div>
    )
}

export default Login;