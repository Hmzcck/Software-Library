export async function fetchItems(categoryIds: number[] = []) {
    // Category query
    const query = categoryIds.length 
        ? categoryIds.map(id => `CategoryIds=${id}`).join('&')
        : "";


    const response = await fetch(`http://localhost:5079/api/items?${query}`, { 
        cache: "no-cache",
    });
    
    console.log(`Fetching from: http://localhost:5079/api/items?${query}`);
    console.log(`Category IDs: ${categoryIds}`);

    if (!response.ok) {
        throw new Error("Failed to fetch items");
    }

    const data = await response.json();
    return data;
}
