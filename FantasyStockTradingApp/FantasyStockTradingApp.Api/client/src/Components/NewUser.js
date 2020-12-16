import React, { useState, useEffect } from 'react';
import { Form, Button, Container, NavLink } from 'react-bootstrap';
import { api, tokenApi } from './API';
import { useAuth } from '../Context/AuthContext';
import { useUser } from '../Context/UserContext';
import { useAccount } from '../Context/AccountContext';
import { useHoldings } from '../Context/HoldingsContext';

function NewUser(props) {
    const [validated, setValidated] = useState(false);
    const [isError, setIsError] = useState(false);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setconfirmPassword] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const { setAuthTokens } = useAuth();
    const { setUser } = useUser();
    const { setAccount} = useAccount();
    const { setHoldings } = useHoldings();

    function newUser() {
        try {
            const response = api.post('/new_user', {
                Email: email.toLowerCase(),
                Password: password,
                FirstName: firstName,
                LastName: lastName
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
        } catch (error) {
            console.error(error);
        }
    }

    function isValidUser() {
        try {
            const response = api.get('/is_valid_user?', {
                params: {
                    Email: email.toLowerCase(),
                }
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function newAccout(userId) {
        try {
            const response = api.post('/new_account', {
                UserId: userId,
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function getAccount(userID) {
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


    function handleEmailChange(e) {
        e.preventDefault();
        setEmail(e.target.value);
    }

    function handlePasswordChange(e) {
        e.preventDefault();
        setPassword(e.target.value);
    }

    function handleConfirmPasswordChange(e) {
        e.preventDefault();
        setconfirmPassword(e.target.value);
    }

    function handleFirstNameChange(e) {
        e.preventDefault();
        setFirstName(e.target.value);
    }

    function handleLastNameChange(e) {
        e.preventDefault();
        setLastName(e.target.value);
    }

    function handleSubmit(e) {
        e.preventDefault();
        const form = e.target;
        if (password === confirmPassword) {
            if (form.checkValidity() !== false) {
                isValidUser().then((isValidUserResponse) => {
                    if (isValidUserResponse.data) {
                        alert('User already exists! Login instead!');
                    } else {
                        newUser().then((newUserResponse) => {
                            if (newUserResponse.status === 200) {

                                getUser().then((getUserResponse) => {
                                    if (getUserResponse.status === 200) {
                                        setUser(getUserResponse.data[0]);

                                        newAccout(getUserResponse.data[0].id).then((newAccountResponse) => {
                                            if (newAccountResponse.status === 200) {

                                                getAccount(getUserResponse.data[0].id).then((getAccountResponse) => {
                                                    if (getAccountResponse.status === 200) {
                                                        setAccount(getAccountResponse.data[0]);
                                                    }

                                                    setHoldings('');
                                                    getToken().then((getTokenResponse) => {
                                                        setAuthTokens(getTokenResponse.data);
                                                    });
                                                })
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
                    }
                })
            }              
            setValidated(true);
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
                            isInvalid={password !== confirmPassword}
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
                            isInvalid={password!==confirmPassword}
                        />
                        <Form.Control.Feedback type="invalid">
                            Passwords do not to match
                        </Form.Control.Feedback>
                    </Form.Group>
                </Form.Row>
                <div id='login-buttons-div'>
                    <Button id='join-user-button' type="submit">Join</Button>
                    <div id='login-new-user-div'>
                        Have an account?
                        <NavLink id="already-existing-user-button" onClick={handleAlreadyExistingUser}>Log in</NavLink>
                    </div>
                </div>
            </Form>
        </Container>
    )
}

export default NewUser;