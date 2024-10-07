import React from "react";
import ItemCard from "./ItemCard"; // Ensure the path is correct

type ItemCardContainerProps = {
  items: Array<{
    id: number;
    name: string;
    categoryNames: Array<string>;
    publisher: string;
    description: string;
    image: string;
  }>;
};

export default function ItemCardContainer({ items }: ItemCardContainerProps) {
  return (
    <div className="container mx-auto p-4">
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {items.map((item) => (
          <ItemCard
            key={item.id}
            id={item.id}
            name={item.name}
            publisher={item.publisher}
            categoryNames={item.categoryNames}
            description={item.description}
            image={item.image}
          />
        ))}
      </div>
    </div>
  );
}
