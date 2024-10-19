"use client";

import { useEffect, useState } from "react";
import { useParams } from "next/navigation";
import ItemDetail from "@/components/itemDetail/ItemDetail";
import ReviewList from "@/components/itemDetail/ReviewList";
import { ItemDetail as ItemDetailType } from "@/types/Item";

export default function ItemDetails() {
  const { id } = useParams();
  const [item, setItem] = useState<ItemDetailType | null>(null);
  const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);
  const [token, setToken] = useState<string>('');

  useEffect(() => {
    // Check if the user is logged in and set the token
    const userToken = localStorage.getItem('authToken');
    if (userToken) {
      setIsLoggedIn(true);
      setToken(userToken);
    }

    if (id) {
      fetch(`http://localhost:5079/api/items/${id}`)
        .then((response) => response.json())
        .then((data) => setItem(data))
        .catch((error) => console.error("Error fetching item details:", error));
    }
  }, [id]);

  if (!item) {
    return <div>Loading...</div>;
  }

  return (
    <div className="container mx-auto p-4">
      <ItemDetail
        name={item.name}
        image={item.image}
        categoryNames={item.categoryNames}
        stars={item.stars}
        forks={item.forks}
        repository={item.repository}
        creationDate={item.creationDate}
        publisher={item.publisher}
        description={item.description}
      />
      <ReviewList
        reviews={item.reviews}
        itemId={Number(id)}
        isLoggedIn={isLoggedIn}
        token={token}
      />
    </div>
  );
}