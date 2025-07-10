import axios, { AxiosError } from 'axios';
import { RoomResponse } from '../types/RoomResponse';
import type {
    AxiosInstance,
    AxiosRequestConfig,
    AxiosResponse,
    CancelToken,
} from 'axios';
import { CreateRoomRequest } from '../types/CreateRoomRequest';

const API_URL = 'https://localhost:7001/api'; // заменить на реальный

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

    updateRoom: async (
        id: number,
        room: CreateRoomRequest
    ): Promise<RoomResponse> => {
        try {
            const response = await axios.put<RoomResponse>(
                `${API_URL}/${id}`,
                room
            );
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
    headers: { [key: string]: any };
    result: any;

    constructor(
        message: string,
        status: number,
        response: string,
        headers: { [key: string]: any },
        result: any
    ) {
        super(message);
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
        Object.setPrototypeOf(this, SwaggerException.prototype);
    }

    protected isSwaggerException = true;

    static isSwaggerException(obj: any): obj is SwaggerException {
        return obj?.isSwaggerException === true;
    }
}

function handleError(error: unknown): never {
    if (isAxiosError(error)) {
        const axiosError = error as AxiosError;

        const message = axiosError.message;
        const status = axiosError.response?.status ?? 0;
        const response = axiosError.response?.data ?? '';
        const headers = axiosError.response?.headers ?? {};

        throw new SwaggerException(
            message,
            status,
            JSON.stringify(response),
            headers,
            response
        );
    }

    // если это не axios-ошибка — выбросить как есть
    throw error;
}

function isAxiosError(obj: any): obj is AxiosError {
    return obj && obj.isAxiosError === true;
}
