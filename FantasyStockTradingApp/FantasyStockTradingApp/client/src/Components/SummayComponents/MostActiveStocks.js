import React, { useState, useEffect } from 'react'
import { Button, Card, CardDeck  } from 'react-bootstrap';
import { iexApi } from '../API';
import TransactionModal from './TransactionModal';

function MostActiveStocks() {
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

    function get_mostActive() {
        try {
            const response = iexApi.get('/get_mostactive');
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function handleClose() {
        setShowModal(false);
    }

    useEffect(() => {
        get_mostActive().then((response) => {
            sessionStorage.setItem('MostActiveStocks', JSON.stringify(response.data));
            setMostActiveHoldings(response.data);
        })
    },[]);

    return (
        <div id="summary-myHoldings-div">
            <TransactionModal show={showModal} handleClose={handleClose} isBuying={isBuying} stockData={stockData} />
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
            }
        </div>
    )
}

export default MostActiveStocks;