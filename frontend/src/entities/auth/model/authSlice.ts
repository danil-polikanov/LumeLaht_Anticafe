import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { AuthResponse } from '@/shared/types';

interface AuthState {
  token: string | null;
  userId: string | null;
  email: string | null;
  role: string | null;
}

const storedToken = localStorage.getItem('authToken');
const storedUser = localStorage.getItem('authUser');
const parsedUser = storedUser ? JSON.parse(storedUser) : null;

const initialState: AuthState = {
  token: storedToken,
  userId: parsedUser?.userId ?? null,
  email: parsedUser?.email ?? null,
  role: parsedUser?.role ?? null,
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setCredentials: (state, action: PayloadAction<AuthResponse>) => {
      const { token, userId, email, role } = action.payload;
      state.token = token;
      state.userId = userId;
      state.email = email;
      state.role = role;
      localStorage.setItem('authToken', token);
      localStorage.setItem('authUser', JSON.stringify({ userId, email, role }));
    },
    logout: (state) => {
      state.token = null;
      state.userId = null;
      state.email = null;
      state.role = null;
      localStorage.removeItem('authToken');
      localStorage.removeItem('authUser');
    },
  },
});

export const { setCredentials, logout } = authSlice.actions;
export const authReducer = authSlice.reducer;
