import React, { useState, useEffect } from 'react'
import { Form, Button, Card, CardDeck } from 'react-bootstrap';
import { useHoldings } from "../../Context/HoldingsContext";
import TransactionModal from './TransactionModal';

function MyHoldings() {
    const [showModal, setShowModal] = useState(false);
    const [isBuying, setIsBuying] = useState(true);
    const [stockData, setStockData] = useState({});
    const { holdings } = useHoldings();

    function handleBuyButton(holding) {
        console.log('Buying');
        console.log(holding);
        setIsBuying(true);
        setStockData({
            symbol: holding.symbol,
            changePercent: holding.change_Percentage,
            change: holding.change,
            companyName: holding.company_Name,
            latestPrice: holding.latest_Cost_per_Stock,
            stockCount: holding.stock_count
        });
        setShowModal(true);
    }

    function handleSellButton(holding) {
        console.log('Selling');
        setIsBuying(false);
        setStockData({
            symbol: holding.symbol,
            changePercent: holding.change_Percentage,
            change: holding.change,
            companyName: holding.company_Name,
            latestPrice: holding.latest_Cost_per_Stock,
            stockCount: holding.stock_Count
        });
        setShowModal(true);
    }

    function handleClose() {
        setShowModal(false);
    }

    /*function searchHoldings() {
        for (let i = 0; i < holdings.length; i++) {
            if (holdings[i].symbol === props.stockData.symbol) {
                setCurrentHoldingStock(holdings[i].stock_Count);
            }
        }
    }*/

    /*useEffect(() => {
        if (stockData !== null && stockData.length !== 0) {
            console.log(stockData);
            *//*setShowModal(true);*//*
        }
    }, [stockData])*/

    return (
        <div id="summary-myHoldings-div">
            <TransactionModal show={showModal} handleClose={handleClose} isBuying={isBuying} stockData={stockData} />
            {holdings === undefined
                ? <div></div>
                : < CardDeck > {
                        holdings.map(
                        (holding, index) =>
                        <Card className="cards-myholdings" key={index}>
                            <Card.Header>
                                        <h3>{holding.symbol}</h3> 
                                        <span>Quantity: {holding.stock_Count}</span>
                            </Card.Header>
                            <Card.Body>
                                <Card.Title>{holding.companyName}</Card.Title>
                                <Card.Text>
                                            <span>Price {holding.latest_cost_per_stock} | Change {holding.change}  | % Changes {holding.change_Percentage}</span>
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