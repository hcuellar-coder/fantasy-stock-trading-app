import React, { useState, useEffect } from 'react'
import { Button, Card, CardDeck, Form  } from 'react-bootstrap';
import { iexApi } from '../API';
import TransactionModal from './TransactionModal';

function MostActiveStocks() {
    const [isError, setIsError] = useState(false);
    const [error, setError] = useState('');
    const [showModal, setShowModal] = useState(false);
    const [isBuying, setIsBuying] = useState(true);
    const [stockData, setStockData] = useState({});
    const [mostActiveHoldings, setMostActiveHoldings] = useState(() => {
        if (sessionStorage.getItem('MostActiveStocks')) {
            return JSON.parse(sessionStorage.getItem('MostActiveStocks'));
        } else {
            return [];
        }
    });

    function handleBuyButton(stock) {
        setIsBuying(true); 
        setStockData(stock);
        setShowModal(true);
    }

    async function getMostActive() {
        try {
            const response = await iexApi.get('/get_mostactive');
            return response;
        } catch (error) {
            return error.response;
        }
    }

    function handleClose() {
        setShowModal(false);
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
            <TransactionModal show={showModal} handleClose={handleClose} isBuying={isBuying} stockData={stockData} />
            {mostActiveHoldings === undefined
                ?
                <div></div>
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
                                        <Card.Text className="card-details">
                                                <span> Price: {stock.latestPrice} </span>
                                                <span> Change: {stock.change} </span>
                                        </Card.Text>
                                        <div className="card-buttons">
                                            <Button className="card-button" onClick={() => { handleBuyButton(stock) }}>Buy</Button>
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