import React, { useState } from 'react';
import { Form, Button, Container, NavLink } from 'react-bootstrap';
import axios from 'axios';

function Login(props) {
    const [validated, setValidated] = useState(false);

    async function getUser(email, password) {
        try {
            const response = await axios.get('/login?', {
                params: {
                    email: email,
                    password: password
                }
            });
            console.log(response);
        } catch(error) {
            console.error(error);
        }
    }

    function handleSubmit(e) {
        e.preventDefault();
        const form = e.target;
        if (form.checkValidity() !== false) {
            await getUser(form.email, form.password).then(async() => (response) {
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
            <Form class="login-form" noValidate validated={validated} onSubmit={handleSubmit}>
                <Form.Row class="login-row">
                    <Form.Group>
                        <Form.Control
                            required
                            type='text'
                            placeholder="Email"
                        />
                    </Form.Group>
                </Form.Row>
                <Form.Row class="login-row">
                    <Form.Group>
                        <Form.Control
                            required
                            type='text'
                            placeholder="Password"
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