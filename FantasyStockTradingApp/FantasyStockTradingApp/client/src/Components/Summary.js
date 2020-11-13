import React, { useState } from 'react';
import { Tabs, Tab} from 'react-bootstrap';
import Search from './SummayComponents/Search';
import TopHoldings from './SummayComponents/TopHoldings';
import MyHoldings from './SummayComponents/MyHoldings';

function Summary() {
    const [key, setKey] = useState('search');

    return (
        <div>
            <Tabs
                id="controlled-tab-example"
                activeKey={key}
                onSelect={(k) => setKey(k)}
            >
                <Tab eventKey="search" title="Search">
                    <Search />
                </Tab>
                <Tab eventKey="topholdings" title="Top Holdings">
                    <TopHoldings />
                </Tab>
                <Tab eventKey="myholdings" title="My Holdings">
                    <MyHoldings />
                </Tab>
            </Tabs>
        </div>
    )
}

export default Summary;