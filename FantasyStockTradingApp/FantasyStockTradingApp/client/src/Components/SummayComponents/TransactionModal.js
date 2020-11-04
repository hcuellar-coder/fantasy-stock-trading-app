import React, { useState, useEffect } from 'react'
import { Form, Button, Modal } from 'react-bootstrap';
import { api } from '../API';

function TransactionModal(props) {
    const [transactionType, setTransactionType] = useState('');
    const [isError, setIsError] = useState(false);
    const [modalDialog, setModalDialog] = useState('');
    const [stockCount, setStockCount] = useState(0);

    useEffect(() => {
        handleModalDialog();
        setStockCount(1);
        if (props.isBuying) {
            setTransactionType('buy');
        } else {
            setTransactionType('sell');
        }
        
    }, [props.show])

    function handleChange(e) {
        e.preventDefault();
        setStockCount(e.target.value);
    }

    function handleModalDialog() {
        if (props.isBuying) {
            setModalDialog(`How many ${ props.stockData.companyName } stock would you like to buy:`);
        } else {
            setModalDialog(`How many ${props.stockData.companyName} stock would you like to sell:`);
        }
    }

    function handleTransactionButtons() {
        console.log(stockCount);
        stock_transaction().then((response) => {
            if (response.status === 200) {
                console.log(response.data);
            } else {
                setIsError(true);
            }
            console.log(response);
        }).catch(e => {
            setIsError(true);
        });
        props.handleClose();
    }

    function stock_transaction() {
        try {
            const response = api.post('/stock_transaction', {
                type: transactionType,
                stock_count: stockCount,
                cost: props.stockData.latestPrice,
                symbol: props.stockData.symbol
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    return (
        <Modal show={props.show} onHide={props.handleClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>{props.stockData.companyName}</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <div>
                    <span>{modalDialog}</span>
                    <Form.Control
                        required
                        id="transaction-modal-buy"
                        type="number"
                        step="1"
                        min="0"
                        placeholder="0"
                        name="stock"
                        value={stockCount}
                        onChange={handleChange} />
                </div>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="Buy" disabled={!props.isBuying} onClick={handleTransactionButtons}>
                    Buy
          </Button>
                <Button variant="Sell" disabled={props.isBuying} onClick={handleTransactionButtons}>
                    Sell
          </Button>
            </Modal.Footer>
        </Modal>
        )
}


export default TransactionModal;