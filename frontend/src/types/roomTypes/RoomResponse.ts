import { AddressResponse } from './AddressResponse';
import { ActivityResponse } from './ActivityResponse';
import { RoomImages } from './RoomImages';

export interface RoomResponse {
    roomId?: string;
    name?: string | undefined;
    description?: string | undefined;
    pricePerHour?: number;
    status?: string;
    address?: AddressResponse;
    activity?: ActivityResponse[] | undefined;
    images?: RoomImages[] | undefined;
}
