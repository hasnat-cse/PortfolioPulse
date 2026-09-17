import AppHeader from "../components/AppHeader";
import HoldingsSection from "../components/HoldingsSection";
import PageHeader from "../components/PageHeader";
import PortfolioSummary from "../components/PortfolioSummary";

function DashboardPage() {
  return (
    <main>
      <AppHeader title="PortfolioPulse" />

      <PageHeader
        title="Dashboard"
        description="Your Portfolio overview will appear here."
      />

      <PortfolioSummary
        totalValue={25430.5}
        dailyChange={125.4}
        totalReturn={2430.5}
      />

      <HoldingsSection />
    </main>
  );
}

export default DashboardPage;
