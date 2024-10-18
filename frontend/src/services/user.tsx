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
      cache: 'no-cache',
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

export const addFavoriteItem = async (itemId: string) => {
  try {
    const token = localStorage.getItem('authToken');
    if (!token) {
      throw new Error('No auth token found');
    }

    const response = await fetch(`${API_URL}/userfavoriteitem/${itemId}`, {
      method: 'POST',
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
    console.error('Error adding favorite item:', error);
    throw error;
  }
};

export const removeFavoriteItem = async (itemId: string) => {
  try {
    const token = localStorage.getItem('authToken');
    if (!token) {
      throw new Error('No auth token found');
    }

    const response = await fetch(`${API_URL}/userfavoriteitem/${itemId}`, {
      method: 'DELETE',
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
    console.error('Error removing favorite item:', error);
    throw error;
  }
}