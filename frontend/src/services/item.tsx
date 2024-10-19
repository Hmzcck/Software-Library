export async function fetchItems({
    categoryIds = [],
    MostStars = false,
    MostForks = false,
    MostRecent = false,
  }) {
    const queryParams = new URLSearchParams();
  
    // Add category IDs
    categoryIds.forEach((id) => queryParams.append("CategoryIds", id));
  
    // Add boolean parameters
    if (MostStars) queryParams.append("MostStars", "true");
    if (MostForks) queryParams.append("MostForks", "true");
    if (MostRecent) queryParams.append("MostRecent", "true");
  
    const url = `http://localhost:5079/api/items?${queryParams.toString()}`;
  
    console.log(`Fetching from: ${url}`);
    console.log(`Category IDs: ${categoryIds}`);
    console.log(
      `Filters: Most Stars: ${MostStars}, Most Forks: ${MostForks}, New Software: ${MostRecent}`
    );
  
    const response = await fetch(url, {
      cache: "no-cache",
    });
  
    if (!response.ok) {
      throw new Error("Failed to fetch items");
    }
  
    const data = await response.json();
    return data;
  }