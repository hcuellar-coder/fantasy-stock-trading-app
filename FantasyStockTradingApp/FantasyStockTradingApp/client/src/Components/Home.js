import React, { useState } from 'react'
import Login from './Login';
import NewUser from './NewUser';

function Home() {
    const [login, setLogin] = useState(true);

    function handleNewUserOrLogin(bool) {
        setLogin(bool);
    }

    return (
        <div>
            {login ?
                <Login login={handleNewUserOrLogin} />
                :
                <NewUser login={handleNewUserOrLogin} />
            }
        </div>
    )
}

export default Home;