import type { Holding } from "../types/Holding";

const API_BASE_URL = "http://localhost:5114";

export async function getHoldings(): Promise<Holding[]> {
  const response = await fetch(`${API_BASE_URL}/api/Holdings`);

  if (!response.ok) {
    throw new Error("Failed to fetch holdings.");
  }

  return response.json();
}

export async function getAccountHoldings(
  accountId: number,
): Promise<Holding[]> {
  const response = await fetch(
    `${API_BASE_URL}/api/Accounts/${accountId}/Holdings`,
  );

  if (!response.ok) {
    throw new Error("Failed to fetch account holdings.");
  }

  return response.json();
}
