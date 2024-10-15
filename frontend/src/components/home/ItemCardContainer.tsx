import React from "react";
import ItemCard from "./ItemCard"; // Ensure the path is correct
import { ItemCardContainerProps } from "@/types/Item"; // Import the Item type

export default function ItemCardContainer({ items }: ItemCardContainerProps) {
  return (
    <div className="container mx-auto p-4">
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {items.map((item) => (
          <ItemCard
            key={item.id}
            id={item.id}
            name={item.name}
            categoryNames={item.categoryNames}
            publisher={item.publisher}
            description={item.description}
            image={item.image}
          />
        ))}
      </div>
    </div>
  );
}
