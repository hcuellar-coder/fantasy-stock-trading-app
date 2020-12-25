import React, { useState } from 'react'
import { Form, Button, Card, CardDeck } from 'react-bootstrap';
import { iexApi } from '../API';

function Search(props) {
    const [isError, setIsError] = useState(false);
    const [error, setError] = useState('');
    const [search, setSearch] = useState('');
    const [searchValid, setSearchValid] = useState(false);
    const [validated, setValidated] = useState(false);
    const [stockData, setStockData] = useState([]);

    function searchSymbol() {
        try {
            const response = iexApi.get('/get_quote', {
                params: {
                    symbol: search
                }
            });
            return response;
        } catch (error) {
            console.error(error);
            return error.response;
        }
    }

    function getHistory(symbol) {
        try {
            const response = iexApi.get('/get_history', {
                params: {
                    symbol: symbol
                }
            });
            return response;
        } catch (error) {
            console.error(error);
            return error.response;
        }
    }

    function handleChange(e) {
        e.preventDefault();
        setSearch(e.target.value);
    }

    function handleSubmit(e) {
        e.preventDefault();
        setIsError(false);
        const form = e.target;
        if (form.checkValidity() !== false) {
            searchSymbol().then((searchSymbolResponse) => {
                if (searchSymbolResponse.status === 200) {
                    setStockData(searchSymbolResponse.data);
                    setSearchValid(true);
                } else {
                    setError(searchSymbolResponse.data.Message);
                    setIsError(true);
                }
            });
        }
        setValidated(true);
    }

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

    return (
        <div>
            <Form className="search-form" noValidate validated={validated} onSubmit={handleSubmit}>
                <Form.Group>
                    <Form.Label className="search-label">Search for Stock</Form.Label>
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
                    {
                        !isError ? <div></div> :
                        <Form.Text className="error-text">
                            Symbol could not be found, try a different symbol!
                        </Form.Text>
                    }
                </Form.Group>
                <Button id="search-submit-button" variant="primary" type="submit">
                        Submit
                    </Button>
            </Form >
            {searchValid && !isError?
                <CardDeck>
                    <Card>
                        <Card.Header>{stockData.symbol}</Card.Header>
                        <Card.Body>
                            <Card.Title>{stockData.companyName}</Card.Title>
                            <Card.Text>
                                <span>Price ${stockData.latestPrice} | Change {stockData.change} | % Changes {stockData.changePercent*100}</span>
                            </Card.Text>
                            <div className="card-buttons">
                                <Button className="card-button" onClick={() => { handleViewButton(stockData.symbol) }}>View</Button>
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