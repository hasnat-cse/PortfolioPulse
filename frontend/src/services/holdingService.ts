import type { Holding } from "../types/Holding";

const API_BASE_URL = "http://localhost:5114";

export async function getHoldings(): Promise<Holding[]> {
  const response = await fetch(`${API_BASE_URL}/api/Holdings`);

  if (!response.ok) {
    throw new Error("Failed to fetch holdings.");
  }

  return response.json();
}
