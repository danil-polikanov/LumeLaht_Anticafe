// Shared types for Room entity
export interface RoomImages {
  imageId: string;
  url: string;
  cloudinaryPublicId?: string;
  isMain: boolean;
  roomId: string;
}

export interface RoomResponse {
  roomId?: string;
  name?: string;
  description?: string;
  pricePerHour?: number;
  capacity?: number;
  status?: string;
  createdAt?: string;
  updatedAt?: string;
  address?: AddressResponse;
  activity?: ActivityResponse[];
  images?: RoomImages[];
}

export interface CreateRoomRequest {
  name?: string;
  description?: string;
  pricePerHour?: number;
  capacity?: number;
  status?: string;
  addressId?: string;
  activityIds?: string[];
}

export interface ActivityResponse {
  activityId?: string;
  name?: string;
  description?: string;
  category?: string;
}

export interface AddressResponse {
  addressId?: string;
  addressName?: string;
  city?: string;
  region?: string;
  postalCode?: string;
  country?: string;
  phoneNumber?: string;
}

export interface ProblemDetails {
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  instance?: string;
  [key: string]: string | number | boolean | undefined;
}
