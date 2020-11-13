import React from 'react';
import { api} from '../API';
import { useUser } from '../../Context/UserContext';

export default function RetrieveUserData(email) {

    function getUser() {
        try {
            const response = api.get('/get_user?', {
                params: {
                    email: email,
                }
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function getAccount(userID) {
        console.log('userId', userID);
        try {
            const response = api.get('/get_account?', {
                params: {
                    user_id: userID
                }
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function getHoldings(accountID) {
        console.log('accountID', accountID);
        try {
            const response = api.get('/get_holdings?', {
                params: {
                    account_id: accountID
                }
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    const { setUserAccount } = useUser();
    let userAccount = {};
    getUser().then((getUserResponse) => {
        if (getUserResponse.status === 200 && getUserResponse.data.length !== 0) {
            console.log('login response = ', getUserResponse.data);
            userAccount = { ...userAccount, user: getUserResponse.data[0] };
            getAccount(getUserResponse.data[0].id).then((getAccountResponse) => {
                if (getAccountResponse.status === 200 && getAccountResponse.data.length !== 0) {
                    userAccount = { ...userAccount, account: getAccountResponse.data[0] };
                    getHoldings(getAccountResponse.data[0].id).then((getHoldingsResponse) => {
                        userAccount = { ...userAccount, holdings: getHoldingsResponse.data };
                        setUserAccount(userAccount);
                    });
                }
            })

        } 
    }).catch(e => {
        console.log(e);
    });
    console.log('data is valid');
}