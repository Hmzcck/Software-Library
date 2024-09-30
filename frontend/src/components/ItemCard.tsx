"use client";

import React from "react";
import Button from "@/components/Button"; // Ensure the path is correct

type ItemCardProps = {
  id: number;
  name: string;
  description: string;
  image: string;
};

export default function ItemCard({ id, name, description, image }: ItemCardProps) {
  return (
    <div className="block rounded-lg bg-white shadow-secondary-1 dark:bg-surface-dark">
      <a href="#!">
        <img
          className="rounded-t-lg"
          src={image}
          alt={name}
        />
      </a>
      <div className="p-6 text-surface dark:text-white">
        <h5 className="mb-2 text-xl font-medium leading-3 text-primary">
          {name}
        </h5>
        <p className="mb-4 text-black">
          {description}
        </p>
        <Button
          variant="primary"
          label="Details"
          onClick={() => console.log(`Item ${id} clicked`)}
        />
      </div>
    </div>
  );
}
