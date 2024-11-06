// ITEM.TSX
import { ItemCardProps, PaginatedResponse } from "@/types/Item";

export async function fetchItems({
  categoryIds = [],
  MostStars = false,
  MostForks = false,
  MostRecent = false,
  PageNumber = 1,
  search = "",
}: {
  categoryIds?: number[];
  MostStars?: boolean;
  MostForks?: boolean;
  MostRecent?: boolean;
  PageNumber?: number;
  search?: string;
}): Promise<PaginatedResponse<ItemCardProps>> {
  const queryParams = new URLSearchParams();

  // Add category IDs
  categoryIds.forEach((id) => queryParams.append("CategoryIds", id.toString()));

  // Add boolean parameters
  if (MostStars) queryParams.append("MostStars", "true");
  if (MostForks) queryParams.append("MostForks", "true");
  if (MostRecent) queryParams.append("MostRecent", "true");

  // Add page number
  if (PageNumber) queryParams.append("PageNumber", PageNumber.toString());

  // Add search parameter
  if (search) queryParams.append("search", search);

  const url = `http://localhost:5079/api/items?${queryParams.toString()}`;

  console.log(`Fetching from: ${url}`);
  console.log(`Category IDs: ${categoryIds}`);
  console.log(
    `Filters: Most Stars: ${MostStars}, Most Forks: ${MostForks}, New Software: ${MostRecent}, Page Number: ${PageNumber}, Search: ${search}`
  );

  const response = await fetch(url, {
    cache: "no-cache",
  });

  if (!response.ok) {
    throw new Error("Failed to fetch items");
  }

  const data: PaginatedResponse<ItemCardProps> = await response.json();
  console.log("API response:", data); // Add this for debugging

  return data; // Return the entire response
}