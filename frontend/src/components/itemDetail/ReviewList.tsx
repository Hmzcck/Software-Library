import React, { useState, useEffect } from 'react';
import { postReview, getReviews } from '@/services/review';
import { Review, ReviewListProps, ReviewData } from '@/types/review';

const ReviewList: React.FC<ReviewListProps> = ({ reviews: initialReviews, itemId, isLoggedIn, token }) => {
  const [reviews, setReviews] = useState<Review[]>(initialReviews || []); 
  const [name, setName] = useState<string>(''); 
  const [rating, setRating] = useState<number>(5);
  const [comment, setComment] = useState<string>('');
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

 
  useEffect(() => {
    if (!initialReviews) {
      (async () => {
        try {
          const fetchedReviews = await getReviews(itemId);
          setReviews(Array.isArray(fetchedReviews) ? fetchedReviews : []);
        } catch (error) {
          console.error('Error fetching reviews:', error);
        }
      })();
    }
  }, [initialReviews, itemId]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!isLoggedIn) {
      alert('You must be logged in to post a review.');
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
        { ...newReview, createdAt: new Date().toISOString(), createdBy: 'You' }, 
      ]);

      setName('');
      setRating(5);
      setComment('');
    } catch (error) {
      console.error('Error posting review:', error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="mt-8 w-full max-w-lg">
      <h2 className="text-xl font-semibold">Reviews</h2>
      {isLoggedIn && (
        <form onSubmit={handleSubmit} className="mt-4">
          <div className="mt-2">
            <label htmlFor="name">Header:</label>
            <input
              type="text"
              id="name"
              value={name}
              onChange={(e) => setName(e.target.value)}
              className="w-full p-2 border rounded text-black"
              required
            />
          </div>
          <div className="mt-2">
            <label htmlFor="rating">Rating:</label>
            <select
              id="rating"
              value={rating}
              onChange={(e) => setRating(Number(e.target.value))}
              className="ml-2 text-black"
            >
              {[1, 2, 3, 4, 5].map((value) => (
                <option key={value} value={value}>
                  {value}
                </option>
              ))}
            </select>
          </div>
          <div className="mt-2">
            <label htmlFor="comment">Comment:</label>
            <textarea
              id="comment"
              value={comment}
              onChange={(e) => setComment(e.target.value)}
              className="w-full p-2 border rounded text-black"
              required
            />
          </div>
          <button type="submit" className="mt-2 px-4 py-2 bg-blue-500 text-white rounded" disabled={isSubmitting}>
            {isSubmitting ? 'Submitting...' : 'Submit Review'}
          </button>
        </form>
      )}
      <ul className="mt-4">
        {reviews.length > 0 ? (
          reviews.map((review) => (
            <li key={review.id} className="border-b py-2">
              <p>
                <strong>{review.name}</strong> ({review.rating} stars)
              </p>
              <p>{review.comment}</p>
              <p className="text-xs text-gray-500">
                By {review.createdBy} on {new Date(review.createdAt).toLocaleDateString()}
              </p>
            </li>
          ))
        ) : (
          <p>No reviews available for this item.</p>
        )}
      </ul>
    </div>
  );
};

export default ReviewList;
