import React, { useState, useEffect } from 'react'
import { Form, Button, Modal } from 'react-bootstrap';
import { api } from '../API';
import { useUser } from "../../Context/UserContext";

function TransactionModal(props) {
    const [transactionType, setTransactionType] = useState('');
    const [isError, setIsError] = useState(false);
    const [modalDialog, setModalDialog] = useState('');
    const [stockCount, setStockCount] = useState(0);
    const [account, setAccount] = useState([]);
    const { userAccount } = useUser();

    useEffect(() => {
        handleModalDialog();
        setStockCount(1);
        if (props.isBuying) {
            setTransactionType('buy');
        } else {
            setTransactionType('sell');
        }
        if (userAccount !== null) {
            setAccount(userAccount.account[0]);
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

    function new_transaction() {
        try {
            const response = api.post('/new_transaction', {
                account_id: account.id,
                type: transactionType,
                symbol: props.stockData.symbol,
                stock_count: stockCount,
                cost_per_stock: props.stockData.latestPrice,
                cost_per_transaction: (props.stockData.latestPrice * stockCount)
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function update_holding() {
        try {
            const response = api.post('/update_holding', {
                account_id: account.id,
                symbol: props.stockData.symbol,
                stock_count: stockCount,
                cost_per_stock: props.stockData.latestPrice,
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function handleTransactionButtons() {
        console.log("account id = ", account.id);
        console.log("transaction type = ", transactionType);
        console.log("symbol = ", props.stockData.symbol);
        console.log("stock count = ", stockCount);
        console.log("cost per stock = ", props.stockData.latestPrice);
        console.log("cost per transaction = ", props.stockData.latestPrice * stockCount);
        

        new_transaction().then((response) => {
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