// services/favoriteItemsService.js

const API_URL = 'http://localhost:5079/api';


export const fetchFavoriteItems = async () => {
  try {
    const token = localStorage.getItem('authToken');
    if (!token) {
      throw new Error('No auth token found');
    }

    const response = await fetch(`${API_URL}/userfavoriteitem`, {
      method: 'GET',
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json', 
      },
    });


    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    const data = await response.json(); 
    return data;
  } catch (error) {
    console.error('Error fetching favorite items:', error);
    throw error;
  }
};
