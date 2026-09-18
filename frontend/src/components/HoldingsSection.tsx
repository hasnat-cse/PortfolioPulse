import { useEffect, useState } from "react";
import "./HoldingsSection.css";
import { getHoldings } from "../services/holdingService";

type HoldingRow = {
  symbol: string;
  quantity: number;
  averageCost: number;
};

function HoldingsSection() {
  const [holdings, setHoldings] = useState<HoldingRow[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    getHoldings()
      .then((data) => {
        setHoldings(
          data.map((holding) => ({
            symbol: holding.symbol,
            quantity: holding.quantity,
            averageCost: holding.averageCost,
          })),
        );
        setIsLoading(false);
      })
      .catch((error) => {
        console.error(error);
        setError("Failed to load holdings.");
        setIsLoading(false);
      });
  }, []);

  return (
    <section className="holdings-section">
      <h2>Holdings</h2>

      {isLoading && <p>Loading holdings...</p>}

      {error && <p>{error}</p>}

      <table className="holdings-table">
        <thead>
          <tr>
            <th>Symbol</th>
            <th>Quantity</th>
            <th>Average Cost</th>
          </tr>
        </thead>

        <tbody>
          {holdings.map((holding) => (
            <tr key={holding.symbol}>
              <td>{holding.symbol}</td>
              <td>{holding.quantity}</td>
              <td>${holding.averageCost.toFixed(2)}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </section>
  );
}

export default HoldingsSection;
