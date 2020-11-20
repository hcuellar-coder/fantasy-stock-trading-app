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
            <div id="summary-balance-div">
                <span id="summary-balance">Balance {account.balance}</span>
                <span id="summary-portfolio-balance">Portfolio {account.portfolio_Balance}</span>
            </div>
            <Tabs
                id="summary-report-tabs"
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