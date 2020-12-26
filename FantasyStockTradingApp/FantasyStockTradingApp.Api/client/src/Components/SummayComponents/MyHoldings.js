import React, { useState } from 'react'
import { Button, Card, CardDeck, Container } from 'react-bootstrap';
import { useHoldings } from "../../Context/HoldingsContext";
import TransactionModal from './TransactionModal';

function MyHoldings() {
    const [showModal, setShowModal] = useState(false);
    const [isBuying, setIsBuying] = useState(true);
    const [stockData, setStockData] = useState({});
    const { holdings } = useHoldings();

    function handleBuyButton(holding) {
        setIsBuying(true);
        setStockData({
            symbol: holding.symbol,
            changePercent: holding.changePercentage,
            change: holding.change,
            companyName: holding.companyName,
            latestPrice: holding.latestCostPerStock,
            stockCount: holding.stockCount
        });
        setShowModal(true);
    }

    function handleSellButton(holding) {
        setIsBuying(false);
        setStockData({
            symbol: holding.symbol,
            changePercent: holding.changePercentage,
            change: holding.change,
            companyName: holding.companyName,
            latestPrice: holding.latestCostPerStock,
            stockCount: holding.stockCount
        });
        setShowModal(true);
    }

    function handleClose() {
        setShowModal(false);
    }

    return (
        <div id="summary-myHoldings-div">
            <TransactionModal show={showModal} handleClose={handleClose} isBuying={isBuying} stockData={stockData} />
            {(holdings === undefined || holdings.length === 0)
                ?
                <Container>
                    <Card className="no-stocks-card">
                        <Card.Title className="no-stocks-card-title"><h3>No holdings to display.</h3></Card.Title>
                    </Card>
                </Container>
                : < CardDeck > {
                    holdings
                        .sort((a ,b ) => a.id - b.id)
                        .map(
                        (holding, index) =>
                        <Card className="cards-responsive" key={index}>
                            <Card.Header>
                                        <h3>{holding.symbol}</h3> 
                                        <span>Quantity: {holding.stockCount}</span>
                            </Card.Header>
                            <Card.Body>
                                <Card.Title>{holding.companyName}</Card.Title>
                                <Card.Text className="card-details">
                                            <span> Price: {(holding.latestCostPerStock).toFixed(2)} </span>
                                            <span> Change: {(holding.change).toFixed(2)} </span>
                                </Card.Text>
                                <div className="card-buttons">
                                            <Button className="card-button" onClick={() => { handleBuyButton(holding) }}>Buy</Button>
                                            <Button className="card-button" onClick={() => { handleSellButton(holding) }}>Sell</Button>
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