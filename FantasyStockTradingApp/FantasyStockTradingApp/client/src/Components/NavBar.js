import React, { useState, useEffect } from 'react';
import { Nav, Navbar, NavItem } from 'react-bootstrap';
import { NavLink, Redirect} from 'react-router-dom';
import { useAuth } from "../Context/AuthContext";
import { useUser } from "../Context/UserContext";

function NavBar(props) {
    const [expanded, setExpanded] = useState(false);
    const { authTokens, setAuthTokens } = useAuth();
    const { userAccount, setUserAccount } = useUser();

    function handleLogOut() {
        setAuthTokens('');
        setUserAccount('');
        return < Redirect to = '/' />;
    }

    useEffect(() => {
        console.log(userAccount);
    }, [userAccount])

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
                        <NavLink className='Navlink' to='/summary'>Summary</NavLink>
                    </NavItem>
                    <NavItem>
                        <NavLink className='Navlink' to='/report' >Report</NavLink>
                    </NavItem>
                    <NavItem>
                        <a className='Navlink' onClick={handleLogOut} hidden={!authTokens} >Log Out</a>
                    </NavItem>
                </Nav>
            </Navbar.Collapse>
        </Navbar>
    )
}

export default NavBar;