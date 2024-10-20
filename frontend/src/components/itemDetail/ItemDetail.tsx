// components/ItemDetail.tsx
import React from "react";
import { Star, GitFork, Calendar, ExternalLink } from "lucide-react";
import { formatDate } from "@/lib/helpers/date";
import Button from "@/components/Button";

type ItemDetailHeaderProps = {
  name: string;
  image: string;
  categoryNames: Array<string>;
  stars: number;
  forks: number;
  repository: string;
  creationDate: string;
  publisher: string;
  description: string;
};

const ItemDetail: React.FC<ItemDetailHeaderProps> = ({
  name,
  image,
  categoryNames,
  stars,
  forks,
  repository,
  creationDate,
  publisher,
  description,
}) => {
  return (
    <div className="item-detail-container max-w-4xl mx-auto p-6 bg-card rounded-xl shadow-lg">
    <img src={image} alt={name} className="w-full h-64 object-cover rounded-xl mb-6" />
    <h1 className="text-3xl font-bold mb-2 gradient-text">{name}</h1>
    <p className="text-lg text-muted-foreground mb-4">{categoryNames.join(", ")}</p>
    <p className="text-muted-foreground mb-6">
      <strong>Publisher:</strong> {publisher}
    </p>
    <div className="flex justify-between items-center mb-6 text-muted-foreground">
      <div className="flex items-center">
        <Star size={24} className="mr-2 text-yellow-400" />
        <span className="text-xl font-semibold">{stars.toLocaleString()}</span>
      </div>
      <div className="flex items-center">
        <GitFork size={24} className="mr-2 text-blue-500" />
        <span className="text-xl font-semibold">{forks.toLocaleString()}</span>
      </div>
      <div className="flex items-center">
        <Calendar size={24} className="mr-2 text-green-500" />
        <span className="text-xl">{formatDate(creationDate)}</span>
      </div>
    </div>

    <p className="text-lg mb-8">{description}</p>

    <button
      onClick={() => window.open(repository, "_blank")}
      className="w-full py-3 px-6 bg-primary text-primary-foreground rounded-md hover:bg-primary/90 transition-colors duration-200 text-lg font-semibold"
    >
      View Repository
    </button>
  </div>
  );
};

export default ItemDetail;
