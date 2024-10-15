"use client";

import React from "react";
import Button from "@/components/Button"; // Ensure the path is correct
import { useRouter } from "next/navigation"; // Use Next.js router for navigation
import { ItemCardProps } from "@/types/Item"; // Import the ItemCardProps type


export default function ItemCard({
  id,
  name,
  categoryNames,
  publisher,
  description,
  image,
}: ItemCardProps) {
  const router = useRouter(); // Hook for navigating

  const handleDetailsClick = () => {
    router.push(`/items/${id}`); // Navigate to dynamic item page
  };

  return (
    <div className="block rounded-lg bg-white shadow-secondary-1 dark:bg-surface-dark w-full h-[400px] max-w-xs overflow-hidden">
      <a href="#!">
        <img
          className="rounded-t-lg w-full h-48 object-cover"
          src={image}
          alt={name}
        />
      </a>
      <div className="p-4 text-surface flex flex-col justify-between h-[calc(100%-12rem)]">
        {/* Item Name */}
        <h5 className="text-lg font-medium leading-tight text-primary mb-1">
          {name}
        </h5>

        {/* Categories */}
        <p className="text-gray-500 text-sm mb-1">{categoryNames.join(", ")}</p>

        {/* Publisher */}
        <p className="text-gray-700 text-sm mb-2">Published by: {publisher}</p>

        {/* Description */}
        <p className="text-black text-sm mb-4 overflow-hidden text-ellipsis line-clamp-2">
          {description}
        </p>

        {/* Details Button */}
        <Button
          variant="primary"
          label="Details"
          onClick={handleDetailsClick}
        />
      </div>
    </div>
  );
}
