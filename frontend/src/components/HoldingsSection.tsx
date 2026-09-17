import "./HoldingsSection.css";

type Holding = {
  symbol: string;
  quantity: number;
  value: number;
};

const holdings: Holding[] = [
  {
    symbol: "HLAL",
    quantity: 25,
    value: 2385.5,
  },
];

function HoldingsSection() {
  return (
    <section className="holdings-section">
      <h2>Holdings</h2>

      <table className="holdings-table">
        <thead>
          <tr>
            <th>Symbol</th>
            <th>Quantity</th>
            <th>Value</th>
          </tr>
        </thead>

        <tbody>
          {holdings.map((holding) => (
            <tr key={holding.symbol}>
              <td>{holding.symbol}</td>
              <td>{holding.quantity}</td>
              <td>${holding.value.toFixed(2)}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </section>
  );
}

export default HoldingsSection;
