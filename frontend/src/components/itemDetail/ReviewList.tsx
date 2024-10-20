import React, { useState, useEffect } from "react";
import { postReview, getReviews } from "@/services/review";
import { Review, ReviewListProps, ReviewData } from "@/types/review";
import { Star } from 'lucide-react';

const ReviewList: React.FC<ReviewListProps> = ({
  reviews: initialReviews,
  itemId,
  isLoggedIn,
  token,
}) => {
  const [reviews, setReviews] = useState<Review[]>(initialReviews || []);
  const [name, setName] = useState<string>("");
  const [rating, setRating] = useState<number>(5);
  const [comment, setComment] = useState<string>("");
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

  useEffect(() => {
    if (!initialReviews) {
      (async () => {
        try {
          const fetchedReviews = await getReviews(itemId);
          setReviews(Array.isArray(fetchedReviews) ? fetchedReviews : []);
        } catch (error) {
          console.error("Error fetching reviews:", error);
        }
      })();
    }
  }, [initialReviews, itemId]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!isLoggedIn) {
      alert("You must be logged in to post a review.");
      return;
    }

    setIsSubmitting(true);

    const reviewData: ReviewData = {
      itemId,
      name,
      rating,
      comment,
    };

    try {
      // Post the review
      const newReview = await postReview(reviewData, token);

      // Option 1: Optimistically update the reviews list by appending the new review
      setReviews((prevReviews) => [
        ...prevReviews,
        { ...newReview, createdAt: new Date().toISOString(), createdBy: "You" },
      ]);

      setName("");
      setRating(5);
      setComment("");
    } catch (error) {
      console.error("Error posting review:", error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="review-container max-w-4xl mx-auto p-6 bg-card rounded-xl shadow-lg mt-8">
      <h2 className="text-2xl font-semibold mb-6 gradient-text">Reviews</h2>
      {isLoggedIn && (
        <form onSubmit={handleSubmit} className="mb-8 space-y-4">
          <div>
            <label
              htmlFor="name"
              className="block text-sm font-medium text-muted-foreground mb-1"
            >
              Header:
            </label>
            <input
              type="text"
              id="name"
              value={name}
              onChange={(e) => setName(e.target.value)}
              className="w-full p-2 border rounded-md bg-input text-foreground"
              required
            />
          </div>
          <div>
            <label
              htmlFor="rating"
              className="block text-sm font-medium text-muted-foreground mb-1"
            >
              Rating:
            </label>
            <select
              id="rating"
              value={rating}
              onChange={(e) => setRating(Number(e.target.value))}
              className="p-2 border rounded-md bg-input text-foreground"
            >
              {[1, 2, 3, 4, 5].map((value) => (
                <option key={value} value={value}>
                  {value} {value === 1 ? "Star" : "Stars"}
                </option>
              ))}
            </select>
          </div>
          <div>
            <label
              htmlFor="comment"
              className="block text-sm font-medium text-muted-foreground mb-1"
            >
              Comment:
            </label>
            <textarea
              id="comment"
              value={comment}
              onChange={(e) => setComment(e.target.value)}
              className="w-full p-2 border rounded-md bg-input text-foreground"
              rows={4}
              required
            />
          </div>
          <button
            type="submit"
            className="w-full py-2 px-4 bg-primary text-primary-foreground rounded-md hover:bg-primary/90 transition-colors duration-200 disabled:opacity-50"
            disabled={isSubmitting}
          >
            {isSubmitting ? "Submitting..." : "Submit Review"}
          </button>
        </form>
      )}
      <ul className="space-y-6">
        {reviews.length > 0 ? (
          reviews.map((review) => (
            <li key={review.id} className="border-b border-border pb-4">
              <div className="flex justify-between items-center mb-2">
                <h3 className="font-semibold">{review.name}</h3>
                <div className="flex items-center">
                  {[...Array(5)].map((_, i) => (
                    <Star
                      key={i}
                      size={16}
                      className={
                        i < review.rating
                          ? "text-yellow-400 fill-current"
                          : "text-gray-300"
                      }
                    />
                  ))}
                </div>
              </div>
              <p className="text-muted-foreground mb-2">{review.comment}</p>
              <p className="text-xs text-muted-foreground">
                By {review.createdBy} on{" "}
                {new Date(review.createdAt).toLocaleDateString()}
              </p>
            </li>
          ))
        ) : (
          <p className="text-muted-foreground">
            No reviews available for this item.
          </p>
        )}
      </ul>
    </div>
  );
};

export default ReviewList;
