import React, { useState, useEffect } from 'react';
import { Form, Button, Container, NavLink, Card } from 'react-bootstrap';
import { api, tokenApi } from './API';
import { useAuth } from '../Context/AuthContext';
import { useUser } from '../Context/UserContext';
import { useAccount } from '../Context/AccountContext';
import { useHoldings } from '../Context/HoldingsContext';

function NewUser(props) {
    const [validated, setValidated] = useState(false);
    const [isError, setIsError] = useState(false);
    const [error, setError] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setconfirmPassword] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const { setAuthTokens } = useAuth();
    const { setUser } = useUser();
    const { setAccount} = useAccount();
    const { setHoldings } = useHoldings();



     async function newUser() {
        try {
            const response = await api.post('/new_user', {
                Email: email.toLowerCase(),
                Password: password,
                FirstName: firstName,
                LastName: lastName
            });
            return response;
        } catch (error) {
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
        } catch (error) {
            return error.response;
        }
    }

    async function isValidUser() {
        try {
            const response = await api.get('/is_valid_user?', {
                params: {
                    Email: email.toLowerCase(),
                }
            });
            return response;
        } catch (error) {
            return error.response;
        }
    }

    async function newAccount(userId) {
        try {
            const response = await api.post('/new_account', {
                UserId: userId,
            });
            return response;
        } catch (error) {
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
        checkPasswordsMatch(e.target.value, confirmPassword);
    }

    function handleConfirmPasswordChange(e) {
        e.preventDefault();
        setconfirmPassword(e.target.value);
        checkPasswordsMatch(password, e.target.value);
    }

    function handleFirstNameChange(e) {
        e.preventDefault();
        setFirstName(e.target.value);
    }

    function handleLastNameChange(e) {
        e.preventDefault();
        setLastName(e.target.value);
    }

    function checkPasswordsMatch(password, confirmPassword) {
        if (password !== confirmPassword) {
            setError('Passwords do not match');
        } else {
            setError('');
        }
    }

    async function handleSubmit(e) {
        setValidated(true);
        e.preventDefault();
        const form = e.target;
        if (password === confirmPassword) {
            if (form.checkValidity() !== false) {
                await isValidUser().then(async (isValidUserResponse) => {
                    console.log(isValidUserResponse);
                    console.log(isValidUserResponse.data);
                    if (isValidUserResponse.data) {
                        alert('User already exists! Login instead!');
                    } else {
                        await newUser().then(async (newUserResponse) => {
                            if (newUserResponse.status === 200) {

                                await getUser().then(async (getUserResponse) => {
                                    if (getUserResponse.status === 200) {
                                        setUser(getUserResponse.data[0]);

                                        await newAccount(getUserResponse.data[0].id).then(async (newAccountResponse) => {
                                            if (newAccountResponse.status === 200) {

                                                await getAccount(getUserResponse.data[0].id).then(async (getAccountResponse) => {
                                                    if (getAccountResponse.status === 200) {
                                                        setAccount(getAccountResponse.data[0]);
                                                        setHoldings('');

                                                        await getToken().then(async (getTokenResponse) => {
                                                            if (getTokenResponse.status === 200) {
                                                                setAuthTokens(getTokenResponse.data);
                                                            } else {
                                                                setError(getTokenResponse.data.Message);
                                                                setIsError(true);
                                                            }
                                                        });
                                                    } else {
                                                        setError(getAccountResponse.data.Message);
                                                        setIsError(true);
                                                    }
                                                });
                                            } else {
                                                setError(newAccountResponse.data.Message);
                                                setIsError(true);
                                            }
                                        });
                                    } else {
                                        setError(getUserResponse.data.Message);
                                        setIsError(true);
                                    }
                                });
                            } else {
                                setError(newUserResponse.data.Message);
                                setIsError(true);
                            }
                        });
                    }
                });
            }              
        }
    }

    function handleAlreadyExistingUser(e) {
        props.login(true);
    }

    useEffect(() => {
        if (isError) {
            setPassword('');
            setconfirmPassword('');
            setIsError(false);
        }
    }, [isError])

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
                            <Form.Text className="new-user-form-text">
                               ex: john.smith@example.com
                            </Form.Text>
                        </Form.Group>
                    </Form.Row>
                    <Form.Row className="login-row">
                        <Form.Group>
                            <Form.Control
                                required
                                type='text'
                                placeholder="First Name"
                                onChange={handleFirstNameChange}
                                value={firstName}
                            />
                        </Form.Group>
                    </Form.Row>
                    <Form.Row className="login-row">
                        <Form.Group>
                            <Form.Control
                                required
                                type='text'
                                placeholder="Last Name"
                                onChange={handleLastNameChange}
                                value={lastName}
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
                        </Form.Group>
                    </Form.Row>
                    <Form.Row className="login-row">
                        <Form.Group>
                            <Form.Control
                                required
                                type='password'
                                placeholder="Confirm Password"
                                onChange={handleConfirmPasswordChange}
                                value={confirmPassword}
                            /> 
                        </Form.Group>
                    </Form.Row>
                    <Form.Text className="error-text">
                        { error }
                    </Form.Text>
                    <div id='login-buttons-div'>
                        <Button id='join-user-button' type="submit">Join</Button>
                        <div id='login-new-user-div'>
                            Have an account?
                            <NavLink id="already-existing-user-button" onClick={handleAlreadyExistingUser}>Log in</NavLink>
                        </div>
                    </div>
                </Form>
            </Container>
        </div>
    )
}

export default NewUser;