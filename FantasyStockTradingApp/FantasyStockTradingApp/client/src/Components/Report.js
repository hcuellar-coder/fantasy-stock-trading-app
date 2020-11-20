import React, { useState, useEffect } from 'react';
import { Tabs, Tab } from 'react-bootstrap';
import MyHoldings from './ReportComponents/MyHoldings';
import Search from './ReportComponents/Search';
import MostActiveStocks from './ReportComponents/MostActiveStocks';
import ReportChart from './ReportComponents/ReportChart';

function Report() {
    const [key, setKey] = useState('search');
    const [chartData, setChartData] = useState([]);
    const [symbol, setSymbol] = useState([]);

    function handleChartData(propChartData, propSymbol) {
        setChartData(propChartData);
        setSymbol(propSymbol);
    }

    useEffect(() => {
        console.log(chartData);
    }, [chartData]);

    return (
        <div>
            <div id="chart-div">
                {chartData.length !== 0  ?
                    <ReportChart chartData={chartData} symbol={symbol} />
                    :
                    <div></div>
                    }
            </div>
            <Tabs
                id="summary-report-tabs"
                activeKey={key}
                onSelect={(k) => setKey(k)}
            >
                <Tab eventKey="search" title="Search">
                    <Search handleChartData={handleChartData} />
                </Tab>
                <Tab eventKey="mostactivestocks" title="Most Active">
                    <MostActiveStocks handleChartData={handleChartData}/>
                </Tab>
                <Tab eventKey="myholdings" title="My Holdings">
                    <MyHoldings handleChartData={handleChartData}/>
                </Tab>
            </Tabs>
        </div>
    )
}

export default Report;