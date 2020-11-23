import React, { useState, useEffect } from 'react'
import { Form, Button, Card, CardDeck } from 'react-bootstrap';
import { api, iexApi } from '../API';
import { useHoldings } from "../../Context/HoldingsContext";

function MyHoldings(props) {
    const [history, setHistory] = useState({});
    const { holdings } = useHoldings();

    function handleViewButton(symbol) {
        console.log('viewing');
        console.log(symbol);
        get_history(symbol).then((response) => {
            console.log(response.data);
            props.handleChartData(response.data, symbol);
        });
    }

    function get_history(symbol) {
        try {
            const response = iexApi.get('/get_history', {
                params: {
                    symbol: symbol
                }
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    useEffect(() => {
        console.log(history);
    }, [history])

    return (
        <div id="summary-myHoldings-div">
            {holdings === undefined
                ? <div></div>
                : < CardDeck > {
                    holdings
                        .sort((a, b) => a.id - b.id)
                        .map(
                        (holding, index) =>
                        <Card className="cards-responsive" key={index}>
                            <Card.Header>
                                        <h3>{holding.symbol}</h3> 
                                        <span>Quantity: {holding.stock_Count}</span>
                            </Card.Header>
                            <Card.Body>
                                <Card.Title>{holding.companyName}</Card.Title>
                                <Card.Text>
                                            <span>Price {holding.latest_Cost_per_Stock} | Change {holding.change}  | % Changes {holding.change_Percentage}</span>
                                </Card.Text>
                                <div className="card-buttons">
                                        <Button className="card-button" onClick={() => { handleViewButton(holding.symbol) }}>View</Button>
                                </div>
                            </Card.Body>
                        </Card>
                    )} 
                </CardDeck>
            }
        </div>
    )
}

export default MyHoldings;