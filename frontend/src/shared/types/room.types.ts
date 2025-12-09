// Shared types for Room entity
export interface RoomImages {
    imageId: string;
    url: string;
    isMain: boolean;
    roomId: string;
}

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

export interface ActivityResponse {
    activityId?: string;
    name?: string;
    description?: string;
}

export interface AddressResponse {
    addressId?: string;
    addressName?: string | undefined;
    city?: string | undefined;
    region?: string | undefined;
    postalCode?: string | undefined;
    country?: string | undefined;
    phoneNumber?: string | undefined;
}

export interface ProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
    [key: string]: any;
}
