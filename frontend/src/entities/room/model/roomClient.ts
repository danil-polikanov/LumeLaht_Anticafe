import axios, { AxiosError } from 'axios';
import { RoomResponse } from '@/shared/types/room.types';
import { CreateRoomRequest } from '@/shared/types/room.types';

const API_URL = 'https://localhost:7001/api'; // replace with actual URL

export const RoomClient = {
  getAllRooms: async (): Promise<RoomResponse[]> => {
    try {
      const response = await axios.get<RoomResponse[]>(API_URL + '/room');
      return response.data;
    } catch (error) {
      handleError(error);
    }
  },

  getRoomById: async (id: number): Promise<RoomResponse> => {
    try {
      const response = await axios.get<RoomResponse>(`${API_URL}/${id}`);
      return response.data;
    } catch (error) {
      handleError(error);
    }
  },

  createRoom: async (room: CreateRoomRequest): Promise<RoomResponse> => {
    try {
      const response = await axios.post<RoomResponse>(API_URL, room);
      return response.data;
    } catch (error) {
      handleError(error);
    }
  },

  updateRoom: async (id: number, room: CreateRoomRequest): Promise<RoomResponse> => {
    try {
      const response = await axios.put<RoomResponse>(`${API_URL}/${id}`, room);
      return response.data;
    } catch (error) {
      handleError(error);
    }
  },

  deleteRoom: async (id: number): Promise<void> => {
    try {
      await axios.delete(`${API_URL}/${id}`);
    } catch (error) {
      handleError(error);
    }
  },
};
export class SwaggerException extends Error {
  status: number;
  response: string;
  headers: Record<string, unknown>;
  result: unknown;

  constructor(
    message: string,
    status: number,
    response: string,
    headers: Record<string, unknown>,
    result: unknown,
  ) {
    super(message);
    this.status = status;
    this.response = response;
    this.headers = headers;
    this.result = result;
    Object.setPrototypeOf(this, SwaggerException.prototype);
  }

  protected isSwaggerException = true;

  static isSwaggerException(obj: unknown): obj is SwaggerException {
    return (obj as Record<string, unknown>)?.isSwaggerException === true;
  }
}

function handleError(error: unknown): never {
  if (isAxiosError(error)) {
    const axiosError = error as AxiosError;

    const message = axiosError.message;
    const status = axiosError.response?.status ?? 0;
    const response = axiosError.response?.data ?? '';
    const headers = axiosError.response?.headers ?? {};

    throw new SwaggerException(message, status, JSON.stringify(response), headers, response);
  }

  // not an axios error — rethrow as is
  throw error;
}

function isAxiosError(obj: unknown): obj is AxiosError {
  return !!(obj && (obj as Record<string, unknown>).isAxiosError === true);
}
