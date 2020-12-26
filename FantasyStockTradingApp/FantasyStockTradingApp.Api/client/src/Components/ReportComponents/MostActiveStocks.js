import React, { useState, useEffect } from 'react'
import { Button, Card, CardDeck, Form } from 'react-bootstrap';
import { iexApi } from '../API';

function MostActiveStocks(props) {
    const [isError, setIsError] = useState(false);
    const [error, setError] = useState('');
    const [mostActiveHoldings, setMostActiveHoldings] = useState(() => {
        if (sessionStorage.getItem('MostActiveStocks')) {
            return JSON.parse(sessionStorage.getItem('MostActiveStocks'));
        } else {
            return [];
        }
    });

    function handleViewButton(symbol) {
        getHistory(symbol).then((getHistoryResponse) => {
            if (getHistoryResponse.status === 200) {
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

    async function getMostActive() {
        try {
            const response = await iexApi.get('/get_mostactive');
            return response;
        } catch (error) {
            return error.response;
        }
    }

    useEffect(() => {
        if (mostActiveHoldings.length === 0) {
            getMostActive().then((getMostActiveResponse) => {
                if (getMostActiveResponse.status === 200) {
                    sessionStorage.setItem('MostActiveStocks', JSON.stringify(getMostActiveResponse.data));
                    setMostActiveHoldings(getMostActiveResponse.data);
                } else {
                    setError(getMostActiveResponse.data.Message);
                    setIsError(true);
                }
            })
        }
    },[]);

    return (
        <div id="summary-myHoldings-div">
            {mostActiveHoldings === undefined
                ? <div></div>
                :
                <div>
                    {
                    !isError ? <div></div> :
                        <Form.Text className="error-text">
                            {error}
                        </Form.Text>
                    }
                    <CardDeck> {
                        mostActiveHoldings.map(
                            (stock, index) =>
                                <Card className="cards-responsive" key={index}>
                                    <Card.Header>
                                        <h3>{stock.symbol}</h3>
                                    </Card.Header>
                                    <Card.Body>
                                        <Card.Title>{stock.companyName}</Card.Title>
                                        <div className="card-buttons">
                                            <Button className="card-button" onClick={() => { handleViewButton(stock.symbol) }}>View</Button>
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

export default MostActiveStocks;