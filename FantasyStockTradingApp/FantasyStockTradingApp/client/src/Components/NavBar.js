import React, { useState } from 'react';
import { Nav, Navbar, NavItem } from 'react-bootstrap';
import { NavLink } from 'react-router-dom';

function NavBar(props) {
    const [expanded, setExpanded] = useState(false);

    return (
        <Navbar id="nav-bar" expanded={expanded} expand="sm" sticky="top">
            <Navbar.Brand href='/'>Fantasy Stock Trading Application</Navbar.Brand>
            <Navbar.Toggle aria-controls="responsive-navbar-nav" onClick={() => setExpanded(expanded ? false : "expanded")} />
            <Navbar.Collapse id="responsive-navbar-nav">
                <Nav className="ml-auto" onClick={() => { setExpanded(false) }}>
                    <NavItem>
                        <NavLink className='Navlink' to='/' exact={true}>Home</NavLink>
                    </NavItem>
                    <NavItem>
                        <NavLink className='Navlink' to='/summary' hidden={true} >Summary</NavLink>
                    </NavItem>
                    <NavItem>
                        <NavLink className='Navlink' to='/report' hidden={true} >Report</NavLink>
                    </NavItem>
                </Nav>
            </Navbar.Collapse>
        </Navbar>
    )
}

export default NavBar;