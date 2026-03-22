import { apiClient, ApiError } from '@/shared/api/config';
import { RoomResponse, CreateRoomRequest, ActivityResponse } from '@/shared/types/room.types';
import {
  PagedRoomsResponse,
  RoomFilters,
  RoomSorting,
  RoomsPagination,
} from '@/shared/types/filters.types';
import axios, { AxiosError } from 'axios';

const ROOM_ENDPOINT = '/room';

export const RoomApi = {
  /**
   * Get all rooms
   */
  getAllRooms: async (): Promise<RoomResponse[]> => {
    try {
      const response = await apiClient.get<RoomResponse[]>(ROOM_ENDPOINT);
      return response.data;
    } catch (error) {
      throw handleError(error);
    }
  },

  /**
   * Get room by ID
   */
  getRoomById: async (id: string): Promise<RoomResponse> => {
    try {
      const response = await apiClient.get<RoomResponse>(`${ROOM_ENDPOINT}/${id}`);
      return response.data;
    } catch (error) {
      throw handleError(error);
    }
  },

  /**
   * Create new room
   */
  createRoom: async (room: CreateRoomRequest): Promise<RoomResponse> => {
    try {
      const response = await apiClient.post<RoomResponse>(ROOM_ENDPOINT, room);
      return response.data;
    } catch (error) {
      throw handleError(error);
    }
  },

  /**
   * Update existing room
   */
  updateRoom: async (id: string, room: CreateRoomRequest): Promise<RoomResponse> => {
    try {
      const response = await apiClient.put<RoomResponse>(`${ROOM_ENDPOINT}/${id}`, room);
      return response.data;
    } catch (error) {
      throw handleError(error);
    }
  },

  /**
   * Delete room
   */
  deleteRoom: async (id: string): Promise<void> => {
    try {
      await apiClient.delete(`${ROOM_ENDPOINT}/${id}`);
    } catch (error) {
      throw handleError(error);
    }
  },

  /**
   * Get rooms with filters
   */
  getRoomsByFilters: async (
    filters: RoomFilters,
    sorting: RoomSorting,
    pagination: RoomsPagination,
  ): Promise<PagedRoomsResponse> => {
    try {
      const params = {
        roomOptionDTO: filters,
        sortOptions: sorting,
        page: pagination.currentPage,
        pageSize: pagination.pageSize,
      };

      const response = await apiClient.post<PagedRoomsResponse>(`${ROOM_ENDPOINT}/filters`, params);
      return response.data;
    } catch (error) {
      throw handleError(error);
    }
  },

  /**
   * Get all activities
   */
  getActivities: async (): Promise<ActivityResponse[]> => {
    try {
      const response = await apiClient.get<ActivityResponse[]>(`${ROOM_ENDPOINT}/activities`);
      return response.data;
    } catch (error) {
      throw handleError(error);
    }
  },
};

function handleError(error: unknown): Error {
  if (axios.isAxiosError(error)) {
    const axiosError = error as AxiosError;
    const message = axiosError.message;
    const status = axiosError.response?.status ?? 0;
    const response = axiosError.response?.data ?? '';
    const headers = axiosError.response?.headers ?? {};

    return new ApiError(message, status, JSON.stringify(response), headers, response);
  }
  return error as Error;
}
