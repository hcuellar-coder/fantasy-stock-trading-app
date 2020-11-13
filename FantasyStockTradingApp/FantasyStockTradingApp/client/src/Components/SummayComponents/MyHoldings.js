import React, { useState, useEffect } from 'react'
import { Form, Button, Card, CardDeck } from 'react-bootstrap';
import { useHoldings } from "../../Context/HoldingsContext";
import TransactionModal from './TransactionModal';

function MyHoldings() {
    const [showModal, setShowModal] = useState(false);
    const [isBuying, setIsBuying] = useState(true);
    const [stockData, setStockData] = useState([]);
    /*const [holdings, setHoldings] = useState([]);*/
    const { holdings } = useHoldings();



    function handleBuyButton() {
        console.log('Buying');
        setIsBuying(true);
        setShowModal(true);
    }

    function handleSellButton() {
        console.log('Selling');
        setIsBuying(false);
        setShowModal(true);
    }

    function handleClose() {
        setShowModal(false);
    }

/*    useEffect(() => {
        console.log('holdings =', holdings);
        if (holdings !== null && holdings !== '')
        {
            setHoldings(holdings);
        }
    }, [holdings])*/

    useEffect(() => {
        console.log(holdings);
    }, [holdings])


    return (
        <div id="summary-myHoldings-div">
            <TransactionModal show={showModal} handleClose={handleClose} isBuying={isBuying} stockData={stockData} />
            <h1>My Holdings</h1>
            {holdings === undefined
                ? <div></div>
                : < CardDeck > {
                        holdings.map(
                        (holding, index) =>
                        <Card key={index}>
                            <Card.Img variant="top" src="holder.js/100px160" />
                            <Card.Body>
                                <Card.Title>{/*holding.companyName*/}</Card.Title>
                                <Card.Text>
                                    <span>Price {holding.latest_cost_per_stock} | Change {holding.latest_cost_per_stock}  | % Changes 1.2</span>
                                </Card.Text>
                                <div className="card-buttons">
                                    <Button className="card-button" onClick={handleBuyButton}>Buy</Button>
                                    <Button className="card-button" onClick={handleSellButton}>Sell</Button>
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