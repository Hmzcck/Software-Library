
const BASE_URL = "http://localhost:5079/api/user"; //  base URL

export const authService = {
  login: async (username, password) => {
    const response = await fetch(`${BASE_URL}/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ username, password }),
    });
    if (!response.ok) throw new Error("Login failed");
    const data = await response.json();
    // Store the token in localStorage
    localStorage.setItem("authToken", data.token);
    return data;
  },

  register: async (username, email, password) => {
    const response = await fetch(`${BASE_URL}/register`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ username, email, password }),
    });
    if (!response.ok) throw new Error("Registration failed");
    return response.json();
  },

  logout: async () => {
    localStorage.removeItem("authToken");
  },

  isLoggedIn: () => {
    return !!localStorage.getItem("authToken");
  },

  getToken: () => {
    return localStorage.getItem("authToken");
  },
};
