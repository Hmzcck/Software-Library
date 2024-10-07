// components/ItemDetail.tsx
import React from 'react';

type ItemDetailHeaderProps = {
  name: string;
  image: string;
  publisher: string;
  description: string;
};

const ItemDetail: React.FC<ItemDetailHeaderProps> = ({ name, image, publisher, description }) => {
  return (
    <div className="flex flex-col items-center">
      <img src={image} alt={name} className="rounded-lg w-full max-w-md object-cover" />
      <h1 className="text-2xl font-bold my-4">{name}</h1>
      <p className="text-gray-700 mb-4">
        <strong>Publisher:</strong> {publisher}
      </p>
      <p className="text-lg">{description}</p>
    </div>
  );
};

export default ItemDetail;
