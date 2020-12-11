import React from 'react'
import { useHoldings } from "../../Context/HoldingsContext";
import Chart from 'react-google-charts';

function MyPortfolio() {
    const { holdings } = useHoldings();

    return (
        <div id="myPortfolio-div">
            {(holdings === undefined || holdings.length === 0)
                ? <div></div>
                :
                <div>
                    <Chart
                        width='100%'
                        height='350px'
                        chartType="PieChart"
                        loader={<div>Loading Chart</div>}
                        data={[
                            ['Symbol', 'Price'],
                            ...holdings.map(d => [d.symbol, (d.latestCostPerStock * d.stockCount)])
                        ]}
                        options={{
                            title: `My Portfolio`,
                            titleTextStyle: { fontSize: 18 },
                            chartArea: {
                                width: '70%',
                                height: '85%'
                            },
                        }}
                        legendToggle
                    />
                    <br />
                    <Chart
                        width='100%'
                        height='350px'
                        chartType="BarChart"
                        loader={<div>Loading Chart</div>}
                        data={[
                            ['Symbol', 'Portfolio Balance'],
                            ...holdings.map(d => [d.symbol, (d.latestCostPerStock * d.stockCount)])
                        ]}
                        options={{
                            title: `My Portfolio`,
                            titleTextStyle: { fontSize: 18 },
                            chartArea: {
                                width: '70%',
                                height: '85%'
                            },
                            hAxis: {
                                title: 'Total Asset Cost',
                                minValue: 0,
                            }
                        }}
                        legendToggle
                    />
                </div>
            }
        </div>
    )
}

export default MyPortfolio;