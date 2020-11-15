import React, { useState, useEffect } from 'react'
import { Form, Button, Modal } from 'react-bootstrap';
import { api } from '../API';
import { useUser } from "../../Context/UserContext";
import { useAccount } from "../../Context/AccountContext";
import { useHoldings } from "../../Context/HoldingsContext";

function TransactionModal(props) {
    const [transactionType, setTransactionType] = useState('');
    const [isError, setIsError] = useState(false);
    const [maxDialog, setMaxDialog] = useState('');
    const [modalDialog, setModalDialog] = useState('');
    const [transactionAmount, setTransactionAmount] = useState(0);
    const [currentHoldingStock, setCurrentHoldingStock] = useState(0);
    const [maxTransactionAmount, setMaxTransactionAmount] = useState(0);
    const { user } = useUser();
    const { account, setAccount } = useAccount();
    const { holdings, setHoldings } = useHoldings();

    useEffect(() => {
        if (props.show) {
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
            }
        }
    }

    function transactionSetup() {
        setTransactionAmount(1);
        let max;
        if (props.isBuying) {
            max = Math.floor((account.balance / props.stockData.latestPrice));
            setTransactionType('buy');
            setMaxTransactionAmount(max);
            setMaxDialog(`Maximum quantity purchasable: ${max}`);
            setModalDialog(`How many ${props.stockData.companyName} stock would you like to buy:`);
        } else {
            props.stockData.stockCount ? max = props.stockData.stockCount : max = currentHoldingStock;
            setTransactionType('sell');
            setMaxTransactionAmount(max);
            setMaxDialog(`Maximum quantity sellable: ${max}`);
            setModalDialog(`How many ${props.stockData.companyName} stock would you like to sell:`);
        }     
    }

    function handleChange(e) {
        e.preventDefault();
        setTransactionAmount(parseInt(e.target.value));
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

    function update_Account() {
        let balance = 0;
        let portfolio_balance = 0;
        if (props.isBuying) {
            balance = account.balance - (props.stockData.latestPrice * transactionAmount);
            portfolio_balance = account.portfolio_Balance + (props.stockData.latestPrice * transactionAmount);
        } else {
            balance = account.balance + (props.stockData.latestPrice * transactionAmount);
            portfolio_balance = account.portfolio_Balance - (props.stockData.latestPrice * transactionAmount);
        } 

        try {
            const response = api.post('/update_account', {
                account_id: account.id,
                balance: balance,
                portfolio_balance: portfolio_balance,
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function get_Account() {
        console.log('userID', user.id);
        try {
            const response = api.get('/get_account?', {
                params: {
                    user_id: user.id
                }
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
                company_name: props.stockData.companyName,
                symbol: props.stockData.symbol,
                stock_count: newTransactionAmount,
                latest_cost_per_stock: props.stockData.latestPrice,
                change: props.stockData.change,
                change_percentage: props.stockData.changePercent,
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function get_Holdings() {
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

                        update_Account().then((updateAccountResponse) => {
                            if (updateAccountResponse.status === 200) {

                                get_Holdings().then((getHoldingsResponse) => {
                                    if (getHoldingsResponse.status === 200) {
                                        setHoldings(getHoldingsResponse.data);

                                        get_Account().then((getAccountResponse) => {
                                            if (getAccountResponse.status === 200) {
                                                setAccount(getAccountResponse.data[0]);
                                            }
                                        })
                                    }
                                })
                            }
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