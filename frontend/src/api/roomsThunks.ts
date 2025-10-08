// roomsThunks.ts
import { createAsyncThunk } from '@reduxjs/toolkit';
import axios, { AxiosError } from 'axios';
import { RoomResponse } from '../types/roomTypes/RoomResponse';
import { ActivityResponse } from '../types/roomTypes/ActivityResponse';
import { CreateRoomRequest } from '../types/roomTypes/CreateRoomRequest';
import {
    PagedRoomsResponse,
    RoomFilters,
} from '../types/roomShowTypes/RoomFilters';
import { RoomsState } from '../redux/rooms/RoomsState';

const API_URL = 'https://localhost:7001/api/room'; // базовый путь

// 1️⃣ Получить все комнаты
export const fetchRooms = createAsyncThunk<
    RoomResponse[], // что возвращаем при успехе
    void, // аргументы, если бы они были
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

// 2️⃣ Получить комнату по id
export const fetchRoomById = createAsyncThunk<
    RoomResponse,
    string,
    { rejectValue: string }
>('rooms/fetchRoomById', async (roomId, { rejectWithValue }) => {
    try {
        const response = await axios.get<RoomResponse>(`${API_URL}/${roomId}`);
        return response.data;
    } catch (error) {
        return rejectWithValue(getErrorMessage(error));
    }
});

// 3️⃣ Создать комнату
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

// 4️⃣ Обновить комнату
export const updateRoom = createAsyncThunk<
    RoomResponse,
    { id: string; data: CreateRoomRequest },
    { rejectValue: string }
>('rooms/updateRoom', async ({ id, data }, { rejectWithValue }) => {
    try {
        const response = await axios.put<RoomResponse>(
            `${API_URL}/${id}`,
            data
        );
        return response.data;
    } catch (error) {
        return rejectWithValue(getErrorMessage(error));
    }
});

// 5️⃣ Удалить комнату
export const deleteRoom = createAsyncThunk<
    void,
    string,
    { rejectValue: string }
>('rooms/deleteRoom', async (roomId, { rejectWithValue }) => {
    try {
        await axios.delete(`${API_URL}/${roomId}`);
    } catch (error) {
        return rejectWithValue(getErrorMessage(error));
    }
});
//  6️⃣ Вывести все активности
export const fetchActivities = createAsyncThunk<
    ActivityResponse[],
    void,
    { rejectValue: string }
>('rooms/fetchActivities', async (_, { rejectWithValue }) => {
    try {
        debugger;
        const response = await axios.get<ActivityResponse[]>(
            `${API_URL}/activities `
        );
        console.log(response.data);
        return response.data;
    } catch (error) {
        return rejectWithValue(getErrorMessage(error));
    }
});
//  7️⃣ Вывести комнаты по фильтрам
export const fetchRoomsByFilters = createAsyncThunk<
    PagedRoomsResponse,
    void,
    { state: { rooms: RoomsState }; rejectValue: string }
>('rooms/fetchRoomsByFilters', async (_, { getState, rejectWithValue }) => {
    try {
        const { filters, sorting, pagination } = getState().rooms;
        const params = {
            roomOptionDTO: filters,
            sortOptions: sorting,
            page: pagination.currentPage,
            pageSize: pagination.pageSize,
        };
        const response = await axios.post<PagedRoomsResponse>(
            `${API_URL}/filters`,
            params
        );

        console.log(response.data);
        return response.data;
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
