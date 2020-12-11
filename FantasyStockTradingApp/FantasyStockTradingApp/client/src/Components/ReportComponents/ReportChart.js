import React from 'react'
import Chart from 'react-google-charts';

function ReportChart(props) {

    return (
        <div>
            <Chart
                width='100%'
                height='400px'
                chartType="LineChart"
                loader={<div>Loading Chart</div>}
                data={[
                    ['Dates', 'Price'],
                    ...props.chartData.map(d => [d.date, d.close])
                ]}
                options={{
                    title: `${props.symbol} - 1 Month History`,
                    titleTextStyle: { fontSize: 18 },
                    colors: ['#e76f51'],
                    chartArea: {
                        width: '70%',
                    },
                    hAxis: {
                        title: 'Dates',
                        titleTextStyle: {
                            italic: false
                        }
                    },
                    vAxis: {
                        title: 'Prices (USD)',
                        titleTextStyle: {
                            italic: false
                        }
                    },
                }}
                legendToggle
            />
        </div>
        )
}

export default ReportChart;