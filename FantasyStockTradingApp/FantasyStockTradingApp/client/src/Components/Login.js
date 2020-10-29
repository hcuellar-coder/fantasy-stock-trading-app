import React, { useState } from 'react';
import { Form, Button, Container, NavLink  } from 'react-bootstrap';

function Login(props) {
    const [validated, setValidated] = useState(false);

    function handleSubmit(e) {
        e.preventDefault();
        const form = e.target;
        if (form.checkValidity() !== false) {
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
            <Form noValidate validated={validated} onSubmit={handleSubmit}>
                <Form.Row>
                    <Form.Group>
                        <Form.Control
                            required
                            type='text'
                            placeholder="Email"
                        />
                    </Form.Group>
                </Form.Row>
                <Form.Row>
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