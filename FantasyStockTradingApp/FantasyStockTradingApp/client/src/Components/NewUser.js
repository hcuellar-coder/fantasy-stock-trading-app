import React, { useState } from 'react';
import { Form, Button, Container, NavLink } from 'react-bootstrap';

function NewUser(props) {
    const [validated, setValidated] = useState(false);

    function handleSubmit(e) {
        e.preventDefault();
        const form = e.target;
        if (form.checkValidity() !== false) {
            console.log('data is valid');
        }
        setValidated(true);
    }

    function handleAlreadyExistingUser(e) {
        props.login(true);
        console.log('handle already existing user');
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
                            placeholder="First Name"
                        />
                    </Form.Group>
                </Form.Row>
                <Form.Row>
                    <Form.Group>
                        <Form.Control
                            required
                            type='text'
                            placeholder="Last Name"
                        />
                    </Form.Group>
                </Form.Row>
                <Form.Row>
                    <Form.Group>
                        <Form.Control
                            required
                            type='password'
                            placeholder="Password"
                        />
                    </Form.Group>
                </Form.Row>
                <Form.Row>
                    <Form.Group>
                        <Form.Control
                            required
                            type='password'
                            placeholder="Confirm Password"
                        />
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