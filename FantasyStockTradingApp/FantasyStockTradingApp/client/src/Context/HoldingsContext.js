import { createContext, useContext } from 'react'

export const HoldingsContext = createContext();

export function useHoldings() {
    return useContext(HoldingsContext);
}