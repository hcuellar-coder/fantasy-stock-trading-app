import React, { useState, useEffect } from 'react'
import { Button, Card, CardDeck } from 'react-bootstrap';
import { iexApi } from '../API';

function MostActiveStocks(props) {
    const [mostActiveHoldings, setMostActiveHoldings] = useState(() => {
        if (sessionStorage.getItem('MostActiveStocks')) {
            return JSON.parse(sessionStorage.getItem('MostActiveStocks'));
        } else {
            return [];
        }
    });

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

    function get_mostActive() {
        try {
            const response = iexApi.get('/get_mostactive');
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    useEffect(() => {
        get_mostActive().then((response) => {
            sessionStorage.setItem('MostActiveStocks', JSON.stringify(response.data));
            setMostActiveHoldings(response.data);
        })
    },[]);

    return (
        <div id="summary-myHoldings-div">
            {mostActiveHoldings === undefined
                ? <div></div>
                : < CardDeck > {
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
            }
        </div>
    )
}

export default MostActiveStocks;