"use client";

import React from "react";
import Button from "@/components/Button";
import Image from "next/image";
import { useRouter } from "next/navigation";
import { Calendar, GitFork, Star } from "lucide-react";
import { formatDate } from "@/lib/helpers/Date";
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
  stars,
  forks,
  repository,
  image,
  creationDate,
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
    <div className="block rounded-lg bg-white shadow-secondary-1 dark:bg-surface-dark w-full h-[450px] max-w-xs overflow-hidden">
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
              isFavorite ? "text-yellow-400 fill-current" : "text-gray-600"
            }`}
          />
        </button>
      </div>
      <div className="p-4 text-surface flex flex-col justify-between h-[calc(100%-12rem)]">
        <div>
          <h5 className="text-lg font-medium leading-tight text-primary mb-1">
            {name}
          </h5>
          <p className="text-gray-500 text-sm mb-1">
            {categoryNames.join(", ")}
          </p>
          <p className="text-gray-700 text-sm mb-2">
            Published by: {publisher}
          </p>
          <p className="text-black text-sm mb-2 overflow-hidden text-ellipsis line-clamp-2">
            {description}
          </p>
        </div>

        <div className="flex justify-between items-center text-sm text-gray-600 mb-2">
          <div className="flex items-center">
            <Star size={16} className="mr-1" />
            <span>{stars.toLocaleString()}</span>
          </div>
          <div className="flex items-center">
            <GitFork size={16} className="mr-1" />
            <span>{forks.toLocaleString()}</span>
          </div>
          <div className="flex items-center">
            <Calendar size={16} className="mr-1" />
            <span>{formatDate(creationDate)}</span>
          </div>
        </div>

        <div className="flex justify-between">
          <Button
            variant="primary"
            label="Details"
            onClick={handleDetailsClick}
          />
          <Button
            variant="secondary"
            label="Repository"
            onClick={() => window.open(repository, "_blank")}
          />
        </div>
      </div>
    </div>
  );
}
