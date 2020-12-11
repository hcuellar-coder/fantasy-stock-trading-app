import React, { useState } from 'react';
import { Nav, Navbar, NavItem } from 'react-bootstrap';
import { NavLink, Redirect} from 'react-router-dom';
import { useAuth } from "../Context/AuthContext";
import { useUser } from "../Context/UserContext";
import { useAccount } from "../Context/AccountContext";
import { useHoldings } from "../Context/HoldingsContext";

function NavBar(props) {
    const [expanded, setExpanded] = useState(false);
    const { authTokens, setAuthTokens } = useAuth();
    const { setUser } = useUser();
    const { setAccount } = useAccount();
    const { setHoldings} = useHoldings();

    function handleLogOut() {
        setAuthTokens('');
        setUser('');
        setAccount('');
        setHoldings('');
        return < Redirect to = '/' />;
    }

    return (
        <Navbar id="nav-bar" expanded={expanded} expand="sm" sticky="top">
            <Navbar.Brand>Stock Trading Application</Navbar.Brand>
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
                        <a className='Navlink' onClick={handleLogOut} hidden={!authTokens} >Logout</a>
                    </NavItem>
                </Nav>
            </Navbar.Collapse>
        </Navbar>
    )
}

export default NavBar;