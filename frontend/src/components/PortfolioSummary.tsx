import "./PortfolioSummary.css";

type PortfolioSummaryProps = {
  totalValue: number;
  dailyChange: number;
  totalReturn: number;
};

function PortfolioSummary({
  totalValue,
  dailyChange,
  totalReturn,
}: PortfolioSummaryProps) {
  return (
    <section className="summary">
      <div className="summary-card">
        <h3>Total Value</h3>
        <p>${totalValue.toFixed(2)}</p>
      </div>

      <div className="summary-card">
        <h3>Today's Change</h3>
        <p>${dailyChange.toFixed(2)}</p>
      </div>

      <div className="summary-card">
        <h3>Total Return</h3>
        <p>${totalReturn.toFixed(2)}</p>
      </div>
    </section>
  );
}

export default PortfolioSummary;
