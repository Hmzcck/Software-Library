"use client";

import { useState, useEffect } from "react";
import { fetchFavoriteItems } from "@/services/user";
import ItemCardContainer from "@/components/home/ItemCardContainer";

export default function FavoriteItems() {
  const [favoriteItems, setFavoriteItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const getFavoriteItems = async () => {
      try {
        const data = await fetchFavoriteItems();
        setFavoriteItems(data);
        setLoading(false);
      } catch (err) {
        setError(err.message);
        setLoading(false);
      }
    };

    getFavoriteItems();
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <div>
      <h1>Favorite Items</h1>
      <ItemCardContainer items={favoriteItems} />
    </div>
  );
}
