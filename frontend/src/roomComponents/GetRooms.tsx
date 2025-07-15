import { useEffect, useState } from 'react';
import { RoomClient } from '../api/RoomClient';
import { RoomResponse } from '../types/roomTypes/RoomResponse';
import './GetRooms.css';
export default function GetRooms() {
    const [rooms, setRooms] = useState<RoomResponse[]>([]);

    useEffect(() => {
        RoomClient.getAllRooms()
            .then((data) => setRooms(data))
            .catch((err) => console.error('Error', err));
    }, []);

    return (
        <div className="container">
            <h1 className="text-2xl font-bold mb-4">Комнаты</h1>
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                {rooms.map((room) => (
                    <div key={room.roomId} className="p-4 border rounded">
                        <h2 className="text-lg font-semibold">{room.name}</h2>
                        <p>{room.description}</p>
                    </div>
                ))}
            </div>
        </div>
    );
}
