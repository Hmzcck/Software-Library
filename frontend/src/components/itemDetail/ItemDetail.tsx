// components/ItemDetail.tsx
import React from "react";
import { Star, GitFork, Calendar, ExternalLink } from 'lucide-react';
import { formatDate } from '@/lib/helpers/Date';
import Button from '@/components/Button';

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
    <div className="flex flex-col items-center max-w-2xl mx-auto p-4">
      <img
        src={image}
        alt={name}
        className="rounded-lg w-full max-w-md object-cover"
      />
      <h1 className="text-2xl font-bold my-4">{name}</h1>
      <p className="text-gray-500 text-sm mb-1">{categoryNames.join(", ")}</p>
      <p className="text-gray-700 mb-4">
        <strong>Publisher:</strong> {publisher}
      </p>
      <div className="flex justify-between items-center w-full max-w-md mb-4">
        <div className="flex items-center">
          <Star size={20} className="mr-1 text-yellow-400" />
          <span className="font-semibold">{stars.toLocaleString()}</span>
        </div>
        <div className="flex items-center">
          <GitFork size={20} className="mr-1 text-blue-500" />
          <span className="font-semibold">{forks.toLocaleString()}</span>
        </div>
        <div className="flex items-center">
          <Calendar size={20} className="mr-1 text-green-500" />
          <span>{formatDate(creationDate)}</span>
        </div>
      </div>

      <p className="text-lg mb-4">{description}</p>

      <Button
        variant="primary"
        label="View Repository"
        onClick={() => window.open(repository, "_blank")}
      />
    </div>
  );
};

export default ItemDetail;
