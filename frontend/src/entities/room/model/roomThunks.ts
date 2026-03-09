// roomsThunks.ts
import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { RoomResponse, CreateRoomRequest, ActivityResponse } from '@/shared/types';
import { PagedRoomsResponse } from '@/shared/types/filters.types';
import { RoomsState } from '@/entities/room/model';

const API_URL = 'https://localhost:7001/api/room'; // base URL

// 1️⃣ Fetch all rooms
export const fetchRooms = createAsyncThunk<
  RoomResponse[], // return type on success
  void, // thunk argument type
  { rejectValue: string }
>('rooms/fetchRooms', async (_, { rejectWithValue }) => {
  try {
    const response = await axios.get<RoomResponse[]>(`${API_URL}`);
    console.log(response.data);
    return response.data;
  } catch (error) {
    return rejectWithValue(getErrorMessage(error));
  }
});

// 2️⃣ Fetch room by id
export const fetchRoomById = createAsyncThunk<RoomResponse, string, { rejectValue: string }>(
  'rooms/fetchRoomById',
  async (roomId, { rejectWithValue }) => {
    try {
      const response = await axios.get<RoomResponse>(`${API_URL}/${roomId}`);
      return response.data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  },
);

// 3️⃣ Create room
export const createRoom = createAsyncThunk<
  RoomResponse,
  CreateRoomRequest,
  { rejectValue: string }
>('rooms/createRoom', async (roomData, { rejectWithValue }) => {
  try {
    const response = await axios.post<RoomResponse>(`${API_URL}`, roomData);
    return response.data;
  } catch (error) {
    return rejectWithValue(getErrorMessage(error));
  }
});

// 4️⃣ Update room
export const updateRoom = createAsyncThunk<
  RoomResponse,
  { id: string; data: CreateRoomRequest },
  { rejectValue: string }
>('rooms/updateRoom', async ({ id, data }, { rejectWithValue }) => {
  try {
    const response = await axios.put<RoomResponse>(`${API_URL}/${id}`, data);
    return response.data;
  } catch (error) {
    return rejectWithValue(getErrorMessage(error));
  }
});

// 5️⃣ Delete room
export const deleteRoom = createAsyncThunk<void, string, { rejectValue: string }>(
  'rooms/deleteRoom',
  async (roomId, { rejectWithValue }) => {
    try {
      await axios.delete(`${API_URL}/${roomId}`);
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  },
);
// ========== REQUEST THROTTLE ==========
let lastFetchTime = 0;
let pendingFetch: Promise<PagedRoomsResponse> | null = null;
const FETCH_THROTTLE = 300; // ms

// 6️⃣ Fetch all activities
export const fetchActivities = createAsyncThunk<ActivityResponse[], void, { rejectValue: string }>(
  'rooms/fetchActivities',
  async (_, { rejectWithValue }) => {
    try {
      const response = await axios.get<ActivityResponse[]>(`${API_URL}/activities`);
      console.log('Activities loaded:', response.data);
      return response.data;
    } catch (error) {
      return rejectWithValue(getErrorMessage(error));
    }
  },
);

// 7️⃣ Fetch rooms by filters (with throttle)
export const fetchRoomsByFilters = createAsyncThunk<
  PagedRoomsResponse,
  void,
  { state: { rooms: RoomsState }; rejectValue: string }
>('rooms/fetchRoomsByFilters', async (_, { getState, rejectWithValue }) => {
  const now = Date.now();

  // reuse pending request if within throttle window
  if (now - lastFetchTime < FETCH_THROTTLE && pendingFetch) {
    console.log('🔄 Throttled: reusing pending request');
    return pendingFetch;
  }

  // throttle passed but request not yet complete — wait for it
  if (pendingFetch) {
    console.log('⏳ Waiting for pending request to complete');
    return pendingFetch;
  }

  try {
    lastFetchTime = now;
    const { filters, sorting, pagination } = getState().rooms;

    const params = {
      roomOptionDTO: filters,
      sortOptions: sorting,
      page: pagination.currentPage,
      pageSize: pagination.pageSize,
    };

    console.log('📤 Sending request:', JSON.stringify(params, null, 2));

    pendingFetch = axios
      .post<PagedRoomsResponse>(`${API_URL}/filters`, params)
      .then((response) => {
        console.log('✅ Response received:', response.data);
        pendingFetch = null; // clear after completion
        return response.data;
      })
      .catch((error) => {
        pendingFetch = null; // clear on error
        throw error;
      });

    return await pendingFetch;
  } catch (error) {
    return rejectWithValue(getErrorMessage(error));
  }
});

//  Addition func for error handler
function getErrorMessage(error: unknown): string {
  if (axios.isAxiosError(error)) {
    return error.message || 'Axios error';
  }
  return 'Unknown error';
}
