"use client";

import React from "react";
import Button from "@/components/Button";
import { useRouter } from "next/navigation";
import { Star } from "lucide-react";
import { ItemCardProps } from "@/types/Item";
import { addFavoriteItem, removeFavoriteItem } from "@/services/user";

interface ExtendedItemCardProps extends ItemCardProps {
  isFavorite: boolean;
  onFavoriteToggle: () => void;
}

export default function ItemCard({
  id,
  name,
  categoryNames,
  publisher,
  description,
  image,
  isFavorite,
  onFavoriteToggle,
}: ExtendedItemCardProps) {
  const router = useRouter();

  const handleDetailsClick = () => {
    router.push(`/items/${id}`);
  };

  const handleFavoriteClick = async () => {
    try {
      if (isFavorite) {
        await removeFavoriteItem(id);
      } else {
        await addFavoriteItem(id);
      }
      onFavoriteToggle();
    } catch (error) {
      console.error("Error updating favorite status:", error);
    }
  };

  return (
    <div className="block rounded-lg bg-white shadow-secondary-1 dark:bg-surface-dark w-full h-[400px] max-w-xs overflow-hidden">
      <div className="relative">
        <img
          className="rounded-t-lg w-full h-48 object-cover"
          src={image}
          alt={name}
        />
        <button
          onClick={handleFavoriteClick}
          className="absolute top-2 right-2 p-2 rounded-full bg-white bg-opacity-50 hover:bg-opacity-75 transition-opacity duration-200"
        >
          <Star
            size={24}
            className={`${
              isFavorite ? 'text-yellow-400 fill-current' : 'text-gray-600'
            }`}
          />
        </button>
      </div>
      <div className="p-4 text-surface flex flex-col justify-between h-[calc(100%-12rem)]">
        <h5 className="text-lg font-medium leading-tight text-primary mb-1">
          {name}
        </h5>
        <p className="text-gray-500 text-sm mb-1">{categoryNames.join(", ")}</p>
        <p className="text-gray-700 text-sm mb-2">Published by: {publisher}</p>
        <p className="text-black text-sm mb-4 overflow-hidden text-ellipsis line-clamp-2">
          {description}
        </p>
        <Button
          variant="primary"
          label="Details"
          onClick={handleDetailsClick}
        />
      </div>
    </div>
  );
}
