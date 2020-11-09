import React, { useState, useEffect } from 'react';
import { Redirect } from 'react-router-dom';
import { Form, Button, Container, NavLink } from 'react-bootstrap';
import { api } from './API';
import { useAuth } from '../Context/Auth';

function Login(props) {
    const [isError, setIsError] = useState(false);
    const [validated, setValidated] = useState(false);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const { setAuthTokens } = useAuth();

    function getUser() {
        try {
            const response = api.get('/get_user?', {
                params: {
                    email: email,
                    password: password
                }
            });
            return response;
        } catch(error) {
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

    function handleSubmit(e) {
        e.preventDefault();
       const form = e.target;
       console.log('form =', form);
        if (form.checkValidity() !== false) {
             getUser().then((response) => {
                if (response.status === 200 && response.data.length !== 0) {
                    setAuthTokens(response.data);
                } else {
                    setIsError(true);
                }
                console.log(response);
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
    },[isError])

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