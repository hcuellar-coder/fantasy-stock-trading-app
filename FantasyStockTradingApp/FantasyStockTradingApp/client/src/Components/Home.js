import React, { useState, useEffect } from 'react'
import Login from './Login';
import NewUser from './NewUser';
import { useAuth } from "../Context/AuthContext";
import { useUser } from "../Context/UserContext";

function Home() {
    const [login, setLogin] = useState(true);
    const { authTokens } = useAuth();
    const { userAccount } = useUser();
    const [user, setUser] = useState([]);
    const [account, setAccount] = useState([]);

    function handleNewUserOrLogin(bool) {
        setLogin(bool);
    }


    useEffect(() => {
        if (userAccount !== null) {
            console.log(userAccount);
            setUser(userAccount.user[0]);
            setAccount(userAccount.account[0]);
        }
    }, [userAccount]);

    return (
        <div>{!authTokens ? (
            login ?
            <Login login = { handleNewUserOrLogin } />
                :
            <NewUser login={ handleNewUserOrLogin } />
        ) : (
                <div>
                    <h1>Welcome {user.first_name}</h1>
                    <span>Balance {account.balance}</span>
                    <span>Portfolio {account.portfolio_Balance}</span>

                    <div>
                        <h3>Summary</h3>
                        <p>
                            In the summary you can buy and sell stocks, you are given
                            a total of 100,000 dollars to buy and sell stocks with. Lets
                            see if you got what it takes to become a wall street tycoon.
                        </p>
                    </div>

                    <div>
                        <h3>Report</h3>
                        <p>
                            To view how your stocks are doing on a graphical interface,
                            take a look at the report page. It also has options to Search stocks,
                            or view the most active stocks as well.
                        </p>
                    </div>

                </div>
            )
        }
        </div>
    )
}

export default Home;