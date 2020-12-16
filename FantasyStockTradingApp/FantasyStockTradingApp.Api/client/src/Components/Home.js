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
                                In the summary page you can buy stocks by searching for a specific stock, or viewing the most active stocks.
                                You can even buy or sell stocks that you already have in your portfolio. You are initially given a total of 
                                100,000 dollars to buy stocks with, and then see if you have what it takes to become a wall street tycoon!
                            </p>
                        </div>

                        <div>
                            <h3 className="home-sub-header">Report</h3>
                            <p className="home-paragraph">
                                To view a 1 month history of stock prices, your holdings and most active stocks
                                take a look at the report page. With this information you can make informed decisions
                                on what stocks to buy and maybe pick up on some trends. It also has and option to view 
                                your portfolio break down! So go on and take a quick gander over at the reports page.
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