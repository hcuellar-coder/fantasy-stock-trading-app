import React, { useState } from 'react';
import { Tabs, Tab} from 'react-bootstrap';
import Search from './SummayComponents/Search';
import MostActiveStocks from './SummayComponents/MostActiveStocks';
import MyHoldings from './SummayComponents/MyHoldings';
import { useAccount } from "../Context/AccountContext";

function Summary() {
    const [key, setKey] = useState('search');
    const { account } = useAccount();

    return (
        <div>
            <div id="summary-balance-div">
                <span id="summary-balance">Balance {(account.balance).toFixed(2)}</span>
                <span id="summary-portfolio-balance">Portfolio {(account.portfolioBalance).toFixed(2)}</span>
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