import React, { useState } from 'react';
import { Form, Button, Container, NavLink } from 'react-bootstrap';
import { dbAccess } from './API';

function Login(props) {
    const [validated, setValidated] = useState(false);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    async function getUser(email, password) {
        try {
            const response = await dbAccess.get('/login?', {
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

   async function handleSubmit(e) {
        e.preventDefault();
       const form = e.target;
       console.log('form =', form);
        if (form.checkValidity() !== false) {
            await getUser(email, password).then((response) => {
                console.log(response);
            });
            console.log('data is valid');
        }
        setValidated(true);
    }

    function handleNewUser(e) {
        props.login(false);
        console.log('handle new user information');
    }

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
                            placeholder="Password"
                            onChange={handlePasswordChange}
                            value={password}
                        />
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