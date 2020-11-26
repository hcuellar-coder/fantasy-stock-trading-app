import React from 'react'
import { useHoldings } from "../../Context/HoldingsContext";
import Chart from 'react-google-charts';

function MyPortfolio(props) {
    const { holdings } = useHoldings();


    return (
        <div id="myPortfolio-div">
            <Chart
                width='100%'
                height='100%'
                chartType="PieChart"
                loader={<div>Loading Chart</div>}
                data={[
                    ['Symbol', 'Price'],
                    ...holdings.map(d => [d.symbol, (d.latest_Cost_per_Stock * d.stock_Count)])
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
            <Chart
                width='100%'
                height='100%'
                chartType="BarChart"
                loader={<div>Loading Chart</div>}
                data={[
                    ['Symbol', 'Price'],
                    ...holdings.map(d => [d.symbol, (d.latest_Cost_per_Stock * d.stock_Count)])
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
        </div>
    )
}

export default MyPortfolio;