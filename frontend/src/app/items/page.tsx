"use client";

import { useState, useEffect } from "react";
import { useSearchParams } from "next/navigation";
import ItemCardContainer from "@/components/home/ItemCardContainer";
import { fetchItems } from "@/services/item";

export default function Items() {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const searchParams = useSearchParams();

  useEffect(() => {
    const getItems = async () => {
      setLoading(true);
      setError(null);

      const categoryIds = searchParams.getAll("category").map(Number);
      const MostStars = searchParams.get("sort") === "stars";
      const MostForks = searchParams.get("sort") === "forks";
      const MostRecent = searchParams.get("sort") === "new";

      try {
        const data = await fetchItems({
          categoryIds,
          MostStars,
          MostForks,
          MostRecent,
        });
        setItems(data);
      } catch (error) {
        console.error("Failed to fetch items:", error);
        setError("Failed to fetch items. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    getItems();
  }, [searchParams]);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div>
      <ItemCardContainer items={items} />
    </div>
  );
}
