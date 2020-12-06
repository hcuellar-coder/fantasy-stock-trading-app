import React from 'react'
import { Button, Card, CardDeck } from 'react-bootstrap';
import { iexApi } from '../API';
import { useHoldings } from "../../Context/HoldingsContext";

function MyHoldings(props) {
    const { holdings } = useHoldings();

    function handleViewButton(symbol) {
        get_history(symbol).then((response) => {
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

    return (
        <div id="summary-myHoldings-div">
            {(holdings === undefined || holdings.length === 0)
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
                                <Card.Title>{holding.company_Name}</Card.Title>
                                <Card.Text>
                                            <span>Total: {(holding.latest_Cost_per_Stock * holding.stock_Count).toFixed(2)}</span>
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