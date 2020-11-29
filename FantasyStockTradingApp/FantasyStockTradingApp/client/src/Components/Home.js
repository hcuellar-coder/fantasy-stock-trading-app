import React, { useState, useEffect } from 'react'
import Login from './Login';
import NewUser from './NewUser';
import { useAuth } from "../Context/AuthContext";
import { useUser } from "../Context/UserContext";
import { useAccount } from "../Context/AccountContext";

function Home() {
    const [login, setLogin] = useState(true);
    const { authTokens } = useAuth();
    const { user } = useUser();
    const { account } = useAccount();

    function handleNewUserOrLogin(bool) {
        setLogin(bool);
    }

    useEffect(() => {
        console.log(account);
    }, [account])

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
                                In the summary you can buy and sell stocks, you are given
                                a total of 100,000 dollars to buy and sell stocks with. Lets
                                see if you got what it takes to become a wall street tycoon.
                            </p>
                        </div>

                        <div>
                            <h3 className="home-sub-header">Report</h3>
                            <p className="home-paragraph">
                                To view how your stocks are doing on a graphical interface,
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