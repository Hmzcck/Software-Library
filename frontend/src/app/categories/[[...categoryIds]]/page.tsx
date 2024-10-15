'use client';

import { useEffect, useState, useMemo } from 'react';
import { fetchItems } from "@/services/item";
import ItemCardContainer from "@/components/home/ItemCardContainer";

export default function CategoryPage({ params }: { params: { categoryIds?: string } }) {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Category query
  const categoryIds = useMemo(() => {
    if (params.categoryIds && params.categoryIds.length > 0) {
      // Decode URL-encoded string, then split it into an array of integers
      const decoded = decodeURIComponent(params.categoryIds[0]);
      console.log('Decoded categoryIds:', decoded);  // Check the decoded value

      // Split the decoded string by commas and map them to integers
      return decoded.split(',').map(id => parseInt(id, 10));
    }
    return [];
  }, [params.categoryIds]);

  useEffect(() => {
    async function loadItems() {
      if (categoryIds.length > 0) {
        try {
          setLoading(true);
          console.log('Fetching items for category IDs:', categoryIds);  // Log category IDs

          const data = await fetchItems(categoryIds);  // Pass categoryIds array to fetchItems
          setItems(data);
        } catch (err: any) {
          setError(err.message);
        } finally {
          setLoading(false);
        }
      } else {
        setItems([]);
        setLoading(false);
      }
    }

    loadItems(); // Trigger the function only when categoryIds changes
  }, [categoryIds]);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div>
      <ItemCardContainer items={items} />
    </div>
  );
}
