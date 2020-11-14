import React, { useState, useEffect } from 'react'
import { Form, Button, Modal } from 'react-bootstrap';
import { api } from '../API';
import { useAccount } from "../../Context/AccountContext";
import { useHoldings } from "../../Context/HoldingsContext";

function TransactionModal(props) {
    const [transactionType, setTransactionType] = useState('');
    const [isError, setIsError] = useState(false);
    const [maxDialog, setMaxDialog] = useState('');
    const [modalDialog, setModalDialog] = useState('');
    const [stockCount, setStockCount] = useState(0);
    const [transactionAmount, setTransactionAmount] = useState(0);
    const [currentHoldingStock, setCurrentHoldingStock] = useState(0);
    const [maxTransactionAmount, setMaxTransactionAmount] = useState(0);
    const { account } = useAccount();
    const { holdings, setHoldings } = useHoldings();

    useEffect(() => {
        if (props.show) {
            handleModalDialog();
            console.log('holdings = ', holdings);
            console.log('account = ', account);
            console.log('props.stockData = ', props.stockData);
            searchHoldings();
            transactionSetup();
        }
    }, [props.show])

    function searchHoldings() {
        for (let i = 0; i < holdings.length; i++) {
            if (holdings[i].symbol === props.stockData.symbol) {
                setCurrentHoldingStock(holdings[i].stock_Count);
                console.log('holdings[i].stock_Count = ', holdings[i].stock_Count);
            }
        }
    }

    function transactionSetup() {
        setTransactionAmount(1);
        let max = Math.floor((account.balance / props.stockData.latestPrice));
        if (props.isBuying) {
            setTransactionType('buy');
            setMaxTransactionAmount(max);
        } else {
            setTransactionType('sell');
            setMaxTransactionAmount(currentHoldingStock);
        }     
    }

    function handleChange(e) {
        e.preventDefault();
        setTransactionAmount(parseInt(e.target.value));
    }

    function handleModalDialog() {
        if (props.isBuying) {
            setMaxDialog(`Maximum quantity purchasable: ${maxTransactionAmount}`);
            setModalDialog(`How many ${ props.stockData.companyName } stock would you like to buy:`);
        } else {
            setMaxDialog(`Maximum quantity sellable: ${maxTransactionAmount}`);
            setModalDialog(`How many ${props.stockData.companyName} stock would you like to sell:`);
        }
    }

    function new_transaction() {
        try {
            const response = api.post('/new_transaction', {
                account_id: account.id,
                type: transactionType,
                symbol: props.stockData.symbol,
                stock_count: transactionAmount,
                cost_per_stock: props.stockData.latestPrice,
                cost_per_transaction: (props.stockData.latestPrice * transactionAmount)
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function update_holding() {
        let newTransactionAmount = 0;
        if (props.isBuying) {
            newTransactionAmount = (currentHoldingStock + transactionAmount);
        } else {
            newTransactionAmount = (currentHoldingStock - transactionAmount);
        } 
        try {
            const response = api.post('/update_holding', {
                account_id: account.id,
                symbol: props.stockData.symbol,
                stock_count: newTransactionAmount,
                latest_cost_per_stock: props.stockData.latestPrice,
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function getHoldings() {
        console.log('accountID', account.id);
        try {
            const response = api.get('/get_holdings?', {
                params: {
                    account_id: account.id
                }
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
        console.log("transactionAmount = ", transactionAmount);
        console.log("cost per stock = ", props.stockData.latestPrice);
        console.log("cost per transaction = ", props.stockData.latestPrice * transactionAmount);
        

        new_transaction().then((transacitonResponse) => {
            console.log(transacitonResponse.data);
            if (transacitonResponse.status === 200) {

                update_holding().then((updateHoldingResponse) => {
                    if (updateHoldingResponse.status === 200) {

                        getHoldings().then((getHoldingsResponse) => {
                            setHoldings(getHoldingsResponse.data);
                        })
                    }          
                });
            } else {
                setIsError(true);
            }
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
                    <span>{maxDialog}</span>
                    <br />
                    <span>{modalDialog}</span>
                    <Form.Control
                        required
                        id="transaction-modal-buy"
                        type="number"
                        step="1"
                        min="1"
                        max={maxTransactionAmount}
                        placeholder="0"
                        name="stock"
                        value={transactionAmount}
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