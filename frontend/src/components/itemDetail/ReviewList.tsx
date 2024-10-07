// components/ReviewList.tsx
import React from 'react';

type Review = {
  id: number;
  name: string;
  rating: number;
  comment: string;
  createdAt: string;
  createdBy: string;
};

type ReviewListProps = {
  reviews: Review[];
};

const ReviewList: React.FC<ReviewListProps> = ({ reviews }) => {
  if (reviews.length === 0) {
    return <p className="mt-8">No reviews available for this item.</p>;
  }

  return (
    <div className="mt-8 w-full max-w-lg">
      <h2 className="text-xl font-semibold">Reviews</h2>
      <ul>
        {reviews.map((review) => (
          <li key={review.id} className="border-b py-2">
            <p>
              <strong>{review.name}</strong> ({review.rating} stars)
            </p>
            <p>{review.comment}</p>
            <p className="text-xs text-gray-500">
              By {review.createdBy} on {new Date(review.createdAt).toLocaleDateString()}
            </p>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ReviewList;
