"use client";

import React, { useEffect, useState } from "react";
import { useRouter, useSearchParams } from "next/navigation";
import Button from "./Button";
import { Category } from "@/types/Category";
import { ChevronDown, Check } from 'lucide-react';

export default function Categories(): React.JSX.Element {
  const [categories, setCategories] = useState<Category[]>([]);
  const [selectedCategories, setSelectedCategories] = useState<number[]>([]);
  const [isVisible, setIsVisible] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const router = useRouter();
  const searchParams = useSearchParams();

  // Fetch categories from the API
  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const response = await fetch("http://localhost:5079/api/categories", {
          cache: "no-cache",
        });
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        const data = await response.json();
        setCategories(data);
      } catch (error: any) {
        setError(error.message);
      } finally {
        setLoading(false);
      }
    };

    fetchCategories();
  }, []);

  // Initialize selected categories from URL
  useEffect(() => {
    const categoryParams = searchParams.getAll("category");
    setSelectedCategories(categoryParams.map(Number));
  }, [searchParams]);

  const toggleCategories = () => {
    setIsVisible(!isVisible);
  };

  // Handle selecting/unselecting categories
  const toggleCategorySelection = (categoryId: number) => {
    setSelectedCategories((prev) =>
      prev.includes(categoryId)
        ? prev.filter((id) => id !== categoryId)
        : [...prev, categoryId]
    );
  };

  // Navigate to the items page with the selected category IDs in the URL
  const handleViewSelected = () => {
    if (selectedCategories.length > 0) {
      const params = new URLSearchParams(searchParams);
      params.delete("category"); // Remove existing category params
      selectedCategories.forEach((id) =>
        params.append("category", id.toString())
      );
      router.push(`/items?${params.toString()}`);
    }
  };

  if (loading) {
    return <div className="text-center py-4">Loading categories...</div>;
  }

  if (error) {
    return <div className="text-center py-4 text-red-500">Error: {error}</div>;
  }

  return (
    <div className="relative">
    <button
      onClick={toggleCategories}
      className="flex items-center justify-between w-full px-4 py-2 text-sm font-medium text-left bg-card text-card-foreground rounded-lg shadow-md hover:bg-muted focus:outline-none focus-visible:ring focus-visible:ring-primary focus-visible:ring-opacity-75 transition-colors duration-200"
    >
      <span>Categories</span>
      <ChevronDown
        className={`w-5 h-5 ml-2 -mr-1 text-muted-foreground transition-transform duration-200 ${
          isVisible ? 'transform rotate-180' : ''
        }`}
        aria-hidden="true"
      />
    </button>

    {isVisible && (
      <div className="absolute z-10 w-full mt-2 origin-top-right bg-card rounded-md shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
        <div className="py-1 max-h-60 overflow-auto">
          {categories.length === 0 ? (
            <p className="px-4 py-2 text-sm text-muted-foreground">No categories found.</p>
          ) : (
            categories.map((category) => (
              <div
                key={category.id}
                className="flex items-center px-4 py-2 text-sm text-card-foreground hover:bg-muted cursor-pointer transition-colors duration-200"
                onClick={() => toggleCategorySelection(category.id)}
              >
                <div className={`w-4 h-4 mr-2 rounded border ${
                  selectedCategories.includes(category.id)
                    ? 'bg-primary border-primary'
                    : 'border-muted-foreground'
                } flex items-center justify-center transition-colors duration-200`}>
                  {selectedCategories.includes(category.id) && (
                    <Check className="w-3 h-3 text-primary-foreground" />
                  )}
                </div>
                <span>{category.name || "Unnamed Category"}</span>
              </div>
            ))
          )}
        </div>
        <div className="border-t border-border">
          <button
            onClick={handleViewSelected}
            className={`block w-full text-left px-4 py-2 text-sm ${
              selectedCategories.length > 0
                ? 'text-primary hover:bg-muted focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary transition-colors duration-200'
                : 'text-muted-foreground cursor-not-allowed'
            }`}
            disabled={selectedCategories.length === 0}
          >
            View Selected ({selectedCategories.length})
          </button>
        </div>
      </div>
    )}
  </div>
  );
}
