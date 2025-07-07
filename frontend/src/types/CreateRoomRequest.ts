import { AddressResponse } from './AddressResponse';
import { ActivityResponse } from './ActivityResponse';
export interface CreateRoomRequest {
    name?: string | undefined;
    description?: string | undefined;
    pricePerHour?: number;
    isActive?: boolean;
    addressId?: number;
    address?: AddressResponse;
    activities?: ActivityResponse[] | undefined;
}
