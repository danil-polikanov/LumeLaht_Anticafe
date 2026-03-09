import axios from 'axios';

export const API_BASE_URL = 'https://localhost:7001/api';

export const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor
apiClient.interceptors.request.use(
  (config) => {
    // Add auth token if exists
    const token = localStorage.getItem('authToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error),
);

// Response interceptor
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      // Handle unauthorized
      localStorage.removeItem('authToken');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  },
);

export class ApiError extends Error {
  status: number;
  response: string;
  headers: { [key: string]: string };
  result: unknown;

  constructor(
    message: string,
    status: number,
    response: string,
    headers: { [key: string]: string },
    result: unknown,
  ) {
    super(message);
    this.status = status;
    this.response = response;
    this.headers = headers;
    this.result = result;
    Object.setPrototypeOf(this, ApiError.prototype);
  }
}
