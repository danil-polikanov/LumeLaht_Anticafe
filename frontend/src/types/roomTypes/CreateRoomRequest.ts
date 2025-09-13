import { AddressResponse } from './AddressResponse';
import { ActivityResponse } from './ActivityResponse';
import { RoomImages } from './RoomImages';
export interface CreateRoomRequest {
    name?: string | undefined;
    description?: string | undefined;
    pricePerHour?: number;
    status?: string;
    addressId?: string;
    address?: AddressResponse;
    activities?: ActivityResponse[] | undefined;
    roomImages?: RoomImages[] | undefined;
}
