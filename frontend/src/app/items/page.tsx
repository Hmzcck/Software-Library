"use client";

import { useState, useEffect } from "react";
import { useSearchParams, useRouter } from "next/navigation";
import ItemCardContainer from "@/components/home/ItemCardContainer";
import { fetchItems } from "@/services/item";

export default function Items() {
  const [items, setItems] = useState([]);
  const [paginationMetadata, setPaginationMetadata] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const searchParams = useSearchParams();
  const router = useRouter();

  useEffect(() => {
    const getItems = async () => {
      setLoading(true);
      setError(null);

      const categoryIds = searchParams.getAll("category").map(Number);
      const MostStars = searchParams.get("sort") === "stars";
      const MostForks = searchParams.get("sort") === "forks";
      const MostRecent = searchParams.get("sort") === "new";
      const PageNumber = parseInt(searchParams.get("page") || "1", 10);

      try {
        const paginatedResponse = await fetchItems({
          categoryIds,
          MostStars,
          MostForks,
          MostRecent,
          PageNumber,
        });
        console.log("Fetched paginatedResponse:", paginatedResponse);
        setItems(paginatedResponse.items);
        setPaginationMetadata({
          pageNumber: paginatedResponse.pageNumber,
          pageSize: paginatedResponse.pageSize,
          totalPages: paginatedResponse.totalPages,
          totalCount: paginatedResponse.totalCount,
          hasNext: paginatedResponse.hasNext,
          hasPrevious: paginatedResponse.hasPrevious,
        });
      } catch (error) {
        console.error("Failed to fetch items:", error);
        setError("Failed to fetch items. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    getItems();
  }, [searchParams]);

  const handlePageChange = (page) => {
    const params = new URLSearchParams(searchParams.toString());
    params.set("page", page.toString());
    router.push(`/items?${params.toString()}`);
  };

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div>
      <ItemCardContainer
        items={items}
        paginationMetadata={paginationMetadata}
        onPageChange={handlePageChange}
      />
    </div>
  );
}
