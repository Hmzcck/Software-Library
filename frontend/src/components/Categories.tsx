"use client";

import React, { useEffect, useState } from "react";
import { useRouter, useSearchParams } from "next/navigation";
import Button from "./Button";
import { Category } from "@/types/Category";

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
    const categoryParams = searchParams.getAll('category');
    setSelectedCategories(categoryParams.map(Number));
  }, [searchParams]);

  const toggleCategories = () => {
    setIsVisible(!isVisible);
  };

  // Handle selecting/unselecting categories
  const toggleCategorySelection = (categoryId: number) => {
    setSelectedCategories(prev =>
      prev.includes(categoryId)
        ? prev.filter(id => id !== categoryId)
        : [...prev, categoryId]
    );
  };

  // Navigate to the items page with the selected category IDs in the URL
  const handleViewSelected = () => {
    if (selectedCategories.length > 0) {
      const params = new URLSearchParams(searchParams);
      params.delete('category'); // Remove existing category params
      selectedCategories.forEach(id => params.append('category', id.toString()));
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
    <div className="relative w-full max-w-md mx-auto">
      <Button
        variant="primary"
        label="Categories"
        onClick={toggleCategories}
        className="w-full flex justify-between items-center"
      >
        <span className={`transform transition-transform duration-200 ${isVisible ? "rotate-180" : ""}`}>
          ▼
        </span>
      </Button>

      {isVisible && (
        <div className="absolute w-full bg-white shadow-lg rounded-lg mt-2 overflow-hidden z-10">
          <div className="max-h-80 overflow-y-auto p-4">
            {categories.length === 0 ? (
              <p className="text-center text-gray-500 text-xs">No categories found.</p>
            ) : (
              <ul className="space-y-2">
                {categories.map((category) => (
                  <li key={category.id} className="flex items-center">
                    <label className="flex items-center space-x-3 w-full cursor-pointer hover:bg-gray-100 p-2 rounded">
                      <input
                        type="checkbox"
                        checked={selectedCategories.includes(category.id)}
                        onChange={() => toggleCategorySelection(category.id)}
                        className="form-checkbox h-4 w-4 text-blue-500"
                      />
                      <span className="text-gray-700 text-xs">
                        {category.name || "Unnamed Category"}
                      </span>
                    </label>
                  </li>
                ))}
              </ul>
            )}
          </div>
          <div className="p-4 bg-gray-50 border-t">
            <button
              onClick={handleViewSelected}
              className={`w-full text-center p-2 rounded-lg transition-colors duration-200 text-xs ${
                selectedCategories.length > 0
                  ? "bg-green-500 text-white hover:bg-green-600"
                  : "bg-gray-300 text-gray-500 cursor-not-allowed"
              }`}
              disabled={selectedCategories.length === 0}
            >
              View Selected Categories ({selectedCategories.length})
            </button>
          </div>
        </div>
      )}
    </div>
  );
}