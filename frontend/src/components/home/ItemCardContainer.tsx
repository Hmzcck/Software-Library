"use client";

import React, { useState, useEffect } from "react";
import ItemCard from "./ItemCard";
import { ItemCardContainerProps } from "@/types/Item";
import { fetchFavoriteItems } from "@/services/user";
import Pagination from "@/components/Pagination";

export default function ItemCardContainer({
  items,
  paginationMetadata,
  onPageChange,
}: ItemCardContainerProps) {
  const [favoriteItemIds, setFavoriteItemIds] = useState<number[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const loadFavorites = async () => {
      try {
        if (!localStorage.getItem("authToken")) {
          setIsLoading(false);
          return;
        }
        const favoriteItemsResponse = await fetchFavoriteItems();
        console.log("Fetched favorite items response:", favoriteItemsResponse);
        if (favoriteItemsResponse && Array.isArray(favoriteItemsResponse.items)) {
          const ids = favoriteItemsResponse.items.map((item) => item.id);
          console.log("Extracted favorite item IDs:", ids);
          setFavoriteItemIds(ids);
        } else {
          console.error("Unexpected response structure:", favoriteItemsResponse);
        }
      } catch (error) {
        console.error("Error fetching favorite items:", error);
      } finally {
        setIsLoading(false);
      }
    };

    loadFavorites();
  }, []);

  useEffect(() => {
    console.log("Current favoriteItemIds:", favoriteItemIds);
  }, [favoriteItemIds]);

  const handleFavoriteToggle = (itemId: number) => {
    setFavoriteItemIds((prevFavorites) =>
      prevFavorites.includes(itemId)
        ? prevFavorites.filter((id) => id !== itemId)
        : [...prevFavorites, itemId]
    );
  };

  if (isLoading) {
    return <div>Loading...</div>;
  }

  console.log("Rendering items:", items);

  return (
    <div className="container mx-auto px-4 py-8">
      {items && items.length > 0 ? (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {items.map((item) => {
            console.log(
              "Rendering item:",
              item.id,
              "Is favorite:",
              favoriteItemIds.includes(item.id)
            );
            return (
              <ItemCard
                key={item.id}
                {...item}
                isFavorite={favoriteItemIds.includes(item.id)}
                onFavoriteToggle={() => handleFavoriteToggle(item.id)}
              />
            );
          })}
        </div>
      ) : (
        <div>No items to display</div>
      )}
      {paginationMetadata && onPageChange && (
        <Pagination metadata={paginationMetadata} onPageChange={onPageChange} />
      )}
    </div>
  );
}