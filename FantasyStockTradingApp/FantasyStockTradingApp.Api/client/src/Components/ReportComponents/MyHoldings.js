import React, { useState } from 'react'
import { Button, Card, CardDeck, Container, Form } from 'react-bootstrap';
import { iexApi } from '../API';
import { useHoldings } from "../../Context/HoldingsContext";

function MyHoldings(props) {
    const { holdings } = useHoldings();
    const [isError, setIsError] = useState(false);
    const [error, setError] = useState('');

   async function handleViewButton(symbol) {
       await getHistory(symbol)
        .then(async (getHistoryResponse) => {
            if (getHistoryResponse.status === 200) {
                setIsError(false);
                props.handleChartData(getHistoryResponse.data, symbol);
            } else {
                setError(getHistoryResponse.data.Message);
                setIsError(true);
            }
        });
    }

    async function getHistory(symbol) {
        try {
            const response = await iexApi.get('/get_history', {
                params: {
                    symbol: symbol
                }
            });
            return response;
        } catch (error) {
            return error.response;
        }
    }

    return (
        <div id="summary-myHoldings-div">
            {(holdings === undefined || holdings.length === 0)
                ? 
                <Container>
                    <Card className="no-stocks-card">
                        <Card.Title className="no-stocks-card-title"><h3>No holdings to display.</h3></Card.Title>
                    </Card>
                </Container>
                :
                <div>
                    {
                        !isError ? <div></div> :
                            <Form.Text className="error-text">
                                {error}
                        </Form.Text>
                    }
                    <CardDeck> {
                        holdings
                            .sort((a, b) => a.id - b.id)
                            .map(
                            (holding, index) =>
                            <Card className="cards-responsive" key={index}>
                                <Card.Header>
                                            <h3>{holding.symbol}</h3> 
                                            <span>Quantity: {holding.stockCount}</span>
                                </Card.Header>
                                <Card.Body>
                                    <Card.Title>{holding.company_Name}</Card.Title>
                                    <Card.Text>
                                                <span>Total: {(holding.latestCostPerStock * holding.stockCount).toFixed(2)}</span>
                                    </Card.Text>
                                    <div className="card-buttons">
                                            <Button className="card-button" onClick={() => { handleViewButton(holding.symbol) }}>View</Button>
                                    </div>
                                </Card.Body>
                            </Card>
                        )} 
                    </CardDeck>
                </div>
            }
        </div>
    )
}

export default MyHoldings;