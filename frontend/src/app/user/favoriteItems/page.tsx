"use client";

import { useState, useEffect } from "react";
import { useSearchParams, useRouter } from "next/navigation";
import { fetchFavoriteItems } from "@/services/user";
import ItemCardContainer from "@/components/home/ItemCardContainer";

export default function FavoriteItems() {
  const [favoriteItems, setFavoriteItems] = useState([]);
  const [paginationMetadata, setPaginationMetadata] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const searchParams = useSearchParams();
  const router = useRouter();

  useEffect(() => {
    const getFavoriteItems = async () => {
      try {
        const page = parseInt(searchParams.get("page") || "1", 10);
        const paginatedResponse = await fetchFavoriteItems(page);
        setFavoriteItems(paginatedResponse.items);
        setPaginationMetadata({
          pageNumber: paginatedResponse.pageNumber,
          pageSize: paginatedResponse.pageSize,
          totalPages: paginatedResponse.totalPages,
          totalCount: paginatedResponse.totalCount,
          hasNext: paginatedResponse.hasNext,
          hasPrevious: paginatedResponse.hasPrevious,
        });
        setLoading(false);
      } catch (err) {
        setError(err.message);
        setLoading(false);
      }
    };

    getFavoriteItems();
  }, [searchParams]);

  const handlePageChange = (page) => {
    const params = new URLSearchParams(searchParams.toString());
    params.set("page", page.toString());
    router.push(`/favorite-items?${params.toString()}`);
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <div>
      <h1>Favorite Items</h1>
      <ItemCardContainer 
        items={favoriteItems} 
        paginationMetadata={paginationMetadata}
        onPageChange={handlePageChange}
      />
    </div>
  );
}