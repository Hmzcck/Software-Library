"use client";

import React from "react";
import Button from "@/components/Button";
import Image from "next/image";
import { useRouter } from "next/navigation";
import { Calendar, GitFork, Star } from "lucide-react";
import { formatDate } from "@/lib/helpers/date";
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
    <div className="item-card hover-lift">
      <div className="relative overflow-hidden rounded-t-xl">
        <img className="item-card-image" src={image} alt={name} />
        <button
          onClick={onFavoriteToggle}
          className="absolute top-2 right-2 p-2 bg-white/80 rounded-full hover:bg-white transition-colors duration-200"
        >
          <Star
            size={20}
            className={`${
              isFavorite ? "text-yellow-400 fill-current" : "text-gray-600"
            }`}
          />
        </button>
      </div>
      <div className="item-card-content">
        <h5 className="item-card-title">{name}</h5>
        <p className="text-sm text-muted-foreground mb-1">
          {categoryNames.join(", ")}
        </p>
        <p className="text-sm text-muted-foreground mb-2">
          Published by: {publisher}
        </p>
        <p className="text-sm mb-4 line-clamp-2">{description}</p>

        <div className="flex justify-between items-center text-sm text-muted-foreground mb-4">
          <div className="flex items-center">
            <Star size={16} className="mr-1 text-yellow-400" />
            <span>{stars.toLocaleString()}</span>
          </div>
          <div className="flex items-center">
            <GitFork size={16} className="mr-1 text-blue-500" />
            <span>{forks.toLocaleString()}</span>
          </div>
          <div className="flex items-center">
            <Calendar size={16} className="mr-1 text-green-500" />
            <span>{formatDate(creationDate)}</span>
          </div>
        </div>

        <div className="flex justify-between gap-2">
          <button
            onClick={handleDetailsClick}
            className="flex-1 py-2 px-4 bg-primary text-primary-foreground rounded-md hover:bg-primary/90 transition-colors duration-200 soft-glow"
          >
            Details
          </button>
          <button
            onClick={() => window.open(repository, "_blank")}
            className="flex-1 py-2 px-4 bg-secondary text-secondary-foreground rounded-md hover:bg-secondary/90 transition-colors duration-200 soft-glow"
          >
            Repository
          </button>
        </div>
      </div>
    </div>
  );
}
