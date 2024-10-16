// services/review.ts
import { ReviewData } from '@/types/review';

const API_BASE_URL = 'http://localhost:5079/api';



export const postReview = async (reviewData: ReviewData, token: string): Promise<any> => {
  try {
    console.log('posting review:', reviewData);
    const response = await fetch(`${API_BASE_URL}/reviews/${reviewData.itemId}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`,
      },
      body: JSON.stringify(reviewData),
    });

    if (!response.ok) {
      throw new Error(`Failed to post review: ${response.statusText}`);
    }

    const data = await response.json();
    return data;
  } catch (error) {
    console.error('Error posting review:', error);
    throw error;
  }
};

export const getReviews = async (itemId: number): Promise<any> => {
  try {
    const response = await fetch(`${API_BASE_URL}/reviews/${itemId}`, {
      method: 'GET',
    });

    if (!response.ok) {
      throw new Error(`Failed to fetch reviews: ${response.statusText}`);
    }

    const data = await response.json();
    return data;
  } catch (error) {
    console.error('Error fetching reviews:', error);
    throw error;
  }
};
