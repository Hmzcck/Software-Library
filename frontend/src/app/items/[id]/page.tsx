"use client";

import { useEffect, useState } from "react";
import { useParams } from "next/navigation"; // useParams for App Router
import ItemDetail from "@/components/itemDetail/ItemDetail";
import ReviewList from "@/components/itemDetail/ReviewList";
import { ItemDetail as ItemDetailType } from "@/types/Item";

export default function ItemDetails() {
  const { id } = useParams(); // Use useParams to get the dynamic route parameter
  const [item, setItem] = useState<ItemDetailType | null>(null);

  useEffect(() => {
    if (id) {
      // Fetch the item details by ID from the API
      fetch(`http://localhost:5079/api/items/${id}`)
        .then((response) => response.json())
        .then((data) => setItem(data))
        .catch((error) => console.error("Error fetching item details:", error));
    }
  }, [id]);

  if (!item) {
    return <div>Loading...</div>; // Display a loading state
  }

  return (
    <div className="container mx-auto p-4">
      <ItemDetail
        name={item.name}
        image={item.image}
        categoryNames={item.categoryNames}
        publisher={item.publisher}
        description={item.description}
      />
      <ReviewList reviews={item.reviews} />
    </div>
  );
}
