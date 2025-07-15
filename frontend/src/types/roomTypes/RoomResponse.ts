import { AddressResponse } from './AddressResponse';
import { ActivityResponse } from './ActivityResponse';

export interface RoomResponse {
    roomId?: number;
    name?: string | undefined;
    description?: string | undefined;
    pricePerHour?: number;
    isActive?: boolean;
    address?: AddressResponse;
    activity?: ActivityResponse[] | undefined;
}
