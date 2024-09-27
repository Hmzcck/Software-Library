"use client";

import React from "react";
import Button from "@/components/Button"; // Ensure the path is correct

export default function ItemCard() {
  return (
    <div className="block rounded-lg bg-white shadow-secondary-1 dark:bg-surface-dark">
      <a href="#!">
        <img
          className="rounded-t-lg"
          src="https://tecdn.b-cdn.net/img/new/standard/nature/184.jpg"
          alt=""
        />
      </a>
      <div className="p-6 text-surface dark:text-white">
        <h5 className="mb-2 text-xl font-medium leading-3 text-primary">
          Card title
        </h5>
        <p className="mb-4 text-black">
          Some quick example text to build on the card title and make up the
          bulk of the card's content.
        </p>
        <Button
          variant="primary"
          label="Button"
          onClick={() => console.log("Item card clicked")}
        />
      </div>
    </div>
  );
}
