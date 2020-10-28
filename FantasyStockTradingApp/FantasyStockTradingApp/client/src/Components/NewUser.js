import React, { useState } from 'react';
import { Form, Button, Container } from 'react-bootstrap';

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
                            placeholder="Password"
                        />
                    </Form.Group>
                </Form.Row>
                <Form.Row>
                    <Form.Group>
                        <Form.Control
                            required
                            type='text'
                            placeholder="Confirm Password"
                        />
                    </Form.Group>
                </Form.Row>
                <div id='login-buttons-div'>
                    <Button id="already-existing-user-button" onClick={handleAlreadyExistingUser}>Already Have an account?</Button>
                    <Button id="join-user-button" type="submit">Join</Button>
                </div>
            </Form>
        </Container>
    )
}

export default NewUser;