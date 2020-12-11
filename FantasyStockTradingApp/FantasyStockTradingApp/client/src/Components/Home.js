import React, { useState } from 'react';
import Login from './Login';
import NewUser from './NewUser';
import { useAuth } from "../Context/AuthContext";
import { useUser } from "../Context/UserContext";

function Home() {
    const [login, setLogin] = useState(true);
    const { authTokens } = useAuth();
    const { user } = useUser(); 

    function handleNewUserOrLogin(bool) {
        setLogin(bool);
    }

    return (
        <div>{!authTokens ? (
            login ?
            <Login login = { handleNewUserOrLogin } />
                :
            <NewUser login={ handleNewUserOrLogin } />
        ) : (
                (user !== undefined) ? (
                    <div>
                        <h1 className="home-header">Welcome {user.firstName}</h1>
                        <div>
                            <h3 className="home-sub-header">Summary</h3>
                            <p className="home-paragraph">
                                In the summary page you can buy and sell stocks. You are given
                                a starting total of 100,000 dollars to buy and sell stocks with. 
                                Lets see if you got what it takes to become a wall street tycoon!
                            </p>
                        </div>

                        <div>
                            <h3 className="home-sub-header">Report</h3>
                            <p className="home-paragraph">
                                To view a how your stocks are doing on a graphical interface,
                                take a look at the report page. It also has options to Search stocks,
                                or view the most active stocks as well.
                            </p>
                        </div>

                    </div>
                ) : <div></div>
            )
        }
        </div>
    )
}

export default Home;