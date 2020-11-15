import React, { useState } from 'react';
import { Tabs, Tab} from 'react-bootstrap';
import Search from './SummayComponents/Search';
import MostActiveStocks from './SummayComponents/MostActiveStocks';
import MyHoldings from './SummayComponents/MyHoldings';
import { useUser } from "../Context/UserContext";
import { useAccount } from "../Context/AccountContext";

function Summary() {
    const [key, setKey] = useState('search');
    const { user } = useUser();
    const { account } = useAccount();

    return (
        <div>
            <span>Balance {account.balance}</span>
            <span>Portfolio {account.portfolio_Balance}</span>
            <Tabs
                id="controlled-tab-example"
                activeKey={key}
                onSelect={(k) => setKey(k)}
            >
                <Tab eventKey="search" title="Search">
                    <Search />
                </Tab>
                <Tab eventKey="mostactivestocks" title="Most Active">
                    <MostActiveStocks />
                </Tab>
                <Tab eventKey="myholdings" title="My Holdings">
                    <MyHoldings />
                </Tab>
            </Tabs>
        </div>
    )
}

export default Summary;