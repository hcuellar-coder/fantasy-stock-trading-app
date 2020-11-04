import React, { useState, useEffect } from 'react'
import { Form, Button, Card, CardDeck } from 'react-bootstrap';
import { api } from '../API';

function Search() {
    const [isError, setIsError] = useState(false);
    const [search, setSearch] = useState('');
    const [searchValid, setSearchValid] = useState(false);
    const [validated, setValidated] = useState(false);
    const [stockData, setStockData] = useState([]);



    function searchSymbol() {
        try {
            const response = api.get('/get_quote', {
                params: {
                    symbol: search
                }
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function handleChange(e) {
        e.preventDefault();
        setSearch(e.target.value);
    }

    function handleSubmit(e) {
        e.preventDefault();
        const form = e.target;
        if (form.checkValidity() !== false) {
            searchSymbol().then((response) => {
                if (response.status === 200 && response.data.length !== 0) {
                    console.log(response.data);
                    setStockData([...stockData, response.data]);
                    setSearchValid(true);
                } else {
                    setIsError(true);
                }
                console.log(response);
            }).catch(e => {
                setIsError(true);
            });
            console.log('data is valid');
        }
        setValidated(true);
    }


    function handleBuyButton() {

    }

    function handleSellButton() {

    }

    useEffect(() => {
        console.log(stockData);
    },[stockData])

    return (
        <div>
            <Form className="search-form" noValidate validated={validated} onSubmit={handleSubmit}>
                <Form.Group>
                    <Form.Label className="search-label">Search Symbol</Form.Label>
                    <Form.Control
                        className="search-form-control"
                        type="text"
                        placeholder="Enter Symbol"
                        required
                        onChange={handleChange}
                        value={search}
                    />
                    <Form.Text className="text-muted">
                        Example: AAPL = apple | GE = General Electric | F = Ford 
                    </Form.Text>
                </Form.Group>
                    <Button variant="primary" type="submit">
                        Submit
                    </Button>
            </Form >
            {searchValid ?
                <CardDeck>
                    <Card>
                        <Card.Img variant="top" src="holder.js/100px160" />
                        <Card.Body>
                            <Card.Title>{stockData[0].companyName}</Card.Title>
                            <Card.Text>
                                <span>Price ${stockData[0].latestPrice} | Change {stockData[0].change} | % Changes {stockData[0].changePercent*100}</span>
                            </Card.Text>
                            <div className="card-buttons">
                                <Button className="card-button">Buy</Button>
                                <Button className="card-button">Sell</Button>
                            </div>
                        </Card.Body>
                    </Card>
                </CardDeck>
                :
                <div></div>
            }
            </div>
    )
}

export default Search;