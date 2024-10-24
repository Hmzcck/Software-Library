"use client";

import React, { useState, useEffect } from "react";
import ItemCard from "./ItemCard";
import { ItemCardContainerProps } from "@/types/Item";
import { fetchFavoriteItems } from "@/services/user";

export default function ItemCardContainer({ items }: ItemCardContainerProps) {
  const [favoriteItemIds, setFavoriteItemIds] = useState<string[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const loadFavorites = async () => {
      try {
        if (!localStorage.getItem("authToken")) {
          return;
        }
        const favorites = await fetchFavoriteItems();
        setFavoriteItemIds(favorites.map((item) => item.id));
      } catch (error) {
        console.error("Error fetching favorite items:", error);
      } finally {
        setIsLoading(false);
      }
    };

    loadFavorites();
  }, []);

  const handleFavoriteToggle = (itemId: string) => {
    setFavoriteItemIds((prevFavorites) =>
      prevFavorites.includes(itemId)
        ? prevFavorites.filter((id) => id !== itemId)
        : [...prevFavorites, itemId]
    );
  };

  if (isLoading) {
    return <div>Loading...</div>;
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        {items.map((item) => (
          <ItemCard
            key={item.id}
            {...item}
            isFavorite={favoriteItemIds.includes(item.id)}
            onFavoriteToggle={() => handleFavoriteToggle(item.id)}
          />
        ))}
      </div>
    </div>
  );
}
