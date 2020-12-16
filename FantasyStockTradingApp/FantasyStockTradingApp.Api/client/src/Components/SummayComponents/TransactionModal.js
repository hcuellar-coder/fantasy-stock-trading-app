import React, { useState, useEffect } from 'react'
import { Form, Button, Modal } from 'react-bootstrap';
import { api } from '../API';
import { useAccount } from "../../Context/AccountContext";
import { useHoldings } from "../../Context/HoldingsContext";

function TransactionModal(props) {
    const [transactionType, setTransactionType] = useState('');
    const [isError, setIsError] = useState(false);
    const [error, setError] = useState('');
    const [maxDialog, setMaxDialog] = useState('');
    const [modalDialog, setModalDialog] = useState('');
    const [transactionAmount, setTransactionAmount] = useState(0);
    const [currentHoldingStock, setCurrentHoldingStock] = useState(0);
    const [holdingId, setHoldingId] = useState(0);
    const [maxTransactionAmount, setMaxTransactionAmount] = useState(0);

    const { account, setAccount } = useAccount();
    const { holdings, setHoldings } = useHoldings();

    useEffect(() => {
        if (props.show) {
            setIsError(false);
            getCurrentHoldingStock();
            transactionSetup();
        }
    }, [props.show])

    function getCurrentHoldingStock() {
        for (let i = 0; i < holdings.length; i++) {
            if (holdings[i].symbol === props.stockData.symbol) {
                setCurrentHoldingStock(holdings[i].stockCount);
                setHoldingId(i);
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
            setModalDialog(`${props.stockData.companyName} stock to buy:`);
        } else {
            props.stockData.stockCount ? max = props.stockData.stockCount : max = currentHoldingStock;
            setTransactionType('sell');
            setMaxTransactionAmount(max);
            setMaxDialog(`Maximum quantity sellable: ${max}`);
            setModalDialog(`${props.stockData.companyName} stock to sell:`);
        }     
    }

    function handleChange(e) {
        e.preventDefault();
        setTransactionAmount(parseInt(e.target.value));
    }

    function new_transaction() {
        try {
            const response = api.post('/new_transaction', {
                AccountId: account.id,
                Type: transactionType,
                Symbol: props.stockData.symbol,
                StockCount: transactionAmount,
                CostPerStock: props.stockData.latestPrice,
                CostPerTransaction: (props.stockData.latestPrice * transactionAmount)
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function update_Account() {
        let balance = 0;
        let portfolioBalance = 0;

        if (props.isBuying) {
            balance = account.balance - (props.stockData.latestPrice * transactionAmount);
            portfolioBalance = account.portfolioBalance + (props.stockData.latestPrice * transactionAmount);
        } else {
            balance = account.balance + (props.stockData.latestPrice * transactionAmount);
            portfolioBalance = account.portfolioBalance - (props.stockData.latestPrice * transactionAmount);
        } 

        try {
            let updatedAccountInformation = {
                id: account.id,
                userId: account.userId,
                balance: balance,
                portfolioBalance: portfolioBalance,
            }
            setAccount(updatedAccountInformation);

            const response = api.post('/update_account', {
                AccountId: account.id,
                Balance: balance,
                PortfolioBalance: portfolioBalance,
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
                AccountId: account.id,
                CompanyName: props.stockData.companyName,
                Symbol: props.stockData.symbol,
                StockCount: newTransactionAmount,
                LatestCostPerStock: props.stockData.latestPrice,
                Change: props.stockData.change,
                ChangePercentage: props.stockData.changePercent,
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function get_Holdings() {
        try {
            const response = api.get('/get_holdings?', {
                params: {
                    AccountId: account.id
                }
            });
            return response;
        } catch (error) {
            console.error(error);
        }
    }

    function handleTransactionButtons() {
        if (maxTransactionAmount < transactionAmount) {
            setIsError(true);
            if (props.isBuying) {
                setError('You cannot purchase more than the max amount!');
            } else {
                setError('You cannot sell more than the max amount!');
            }
        } else {
            new_transaction();
            update_Account();
            update_holding().then((updateHoldingResponse) => {
                if (updateHoldingResponse.status === 200) {
                    if (holdingId !== 0) {
                        let tempHoldings = [...holdings];
                        if (props.isBuying) {
                            tempHoldings[holdingId].stockCount = currentHoldingStock + transactionAmount;
                        } else {
                            if (currentHoldingStock - transactionAmount === 0) {
                                tempHoldings.splice(holdingId, 1);
                            } else {
                                tempHoldings[holdingId].stockCount = currentHoldingStock - transactionAmount;
                            }
                        }
                        setHoldings(tempHoldings);
                    } else {
                        get_Holdings().then((getHoldingsResponse) => {
                            if (getHoldingsResponse.status === 200) {
                                setHoldings(getHoldingsResponse.data);
                            }
                        });
                    }
                } 
            });
            props.handleClose();
        }
    }

    return (
        <Modal show={props.show} onHide={props.handleClose} centered>
            <Modal.Header id="transaction-modal-header" closeButton>
                <Modal.Title>{props.stockData.companyName}</Modal.Title>
            </Modal.Header>
            <Modal.Body id="transaction-modal-body">
                <div>
                    <span>{maxDialog}</span>
                    <br />
                    <span>Balance: {props.isBuying ?
                        (account.balance - (transactionAmount * props.stockData.latestPrice)).toFixed(2)
                        :
                        (account.balance + (transactionAmount * props.stockData.latestPrice)).toFixed(2)
                    } </span>

                    <span>Cost: {(transactionAmount * props.stockData.latestPrice).toFixed(2)}</span>
                    <br />
                    <span>{modalDialog}</span>
                    {
                        !isError ? <div></div> :
                            <Form.Text className="error-text">
                                {error}
                        </Form.Text>
                    }
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
                <div id="transaction-modal-buttons">
                    <Button
                        className="transaction-modal-button"
                        variant="Buy"
                        disabled={!props.isBuying}
                        onClick={handleTransactionButtons}>
                        Buy
                    </Button>
                    <Button
                        className="transaction-modal-button"
                        variant="Sell"
                        disabled={props.isBuying}
                        onClick={handleTransactionButtons}>
                            Sell
                    </Button>
                </div>
            </Modal.Body>
        </Modal>
        )
}


export default TransactionModal;