import React, { useState, useEffect } from 'react';
import { Form, Button, Container, NavLink } from 'react-bootstrap';
import { api } from './API';
import { useAuth } from '../Context/AuthContext';
import { useUser } from '../Context/UserContext';

function NewUser(props) {
    const [validated, setValidated] = useState(false);
    const [isError, setIsError] = useState(false);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setconfirmPassword] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const { setAuthTokens } = useAuth();
    const { setUserAccount } = useUser();

    function newUser() {
        /*
         Things to do: password needs checking
         check if email is correct
         check if user already exists
         */

        try {
            const response = api.post('/new_user', {
                email: email,
                password: password,
                first_name: firstName,
                last_name: lastName
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function doesUserExist() {
        try {
            const response = api.get('/get_user?', {
                params: {
                    email: email,
                }
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function newAccoutBalance(userID) {
        try {
            const response = api.post('/new_account', {
                user_id: userID,
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
        let userAccount = {};
        let token = {};
        if (password === confirmPassword) {
            if (form.checkValidity() !== false) {
                doesUserExist().then((response) => {
                    if (response.status === 200 && response.data.length !== 0) {
                        console.log('user already exists! Login instead!');
                    } else {
                        newUser().then((response) => {
                            userAccount = { ...userAccount, user : response.data };
                            token = {...token , token : response.data };
                            if (response.status === 200) {
                                newAccoutBalance(response.data.id).then((response) => {
                                    console.log(response);
                                    userAccount = { ...userAccount, account : response.data };
                                    setUserAccount(userAccount);
                                    setAuthTokens(token);
                                });
                                
                            } else {
                                setIsError(true);
                            }
                            console.log(response);
                        }).catch(e => {
                            setIsError(true);
                        });
                    }
                })
            }              
            console.log('data is valid');
            setValidated(true);
        }
    }

    function handleAlreadyExistingUser(e) {
        props.login(true);
        console.log('handle already existing user');
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