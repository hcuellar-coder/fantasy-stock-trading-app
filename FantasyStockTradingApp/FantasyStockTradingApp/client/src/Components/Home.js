import React, { useState } from 'react'
import Login from './Login';
import NewUser from './NewUser';
import { useAuth } from "../Context/Auth";

function Home() {
    const [login, setLogin] = useState(true);
    const { authTokens } = useAuth();

    function handleNewUserOrLogin(bool) {
        setLogin(bool);
    }

    return (
        <div>{!authTokens ? (
            login ?
            <Login login = { handleNewUserOrLogin } />
                :
            <NewUser login={handleNewUserOrLogin} />
        ) : (
                <div></div>
            )
        }
        </div>
    )
}

export default Home;