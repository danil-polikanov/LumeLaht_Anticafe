import React, { useEffect, useState } from 'react';
import axios from 'axios';

interface Room {
    roomId: number;
    name: string;
    pricePerHour: number;
}

const RoomList: React.FC = () => {
    const [rooms, setRooms] = useState<Room[]>([]);

    useEffect(() => {
        axios
            .get('https://localhost:7001/api/Room')
            .then((response) => response.data)
            .then((rooms) => {
                setRooms(rooms);
            })
            .catch((error) => {
                console.error('Ошибка при получении комнат:', error);
            });
    }, []);

    return (
        <div>
            <h2>Список комнат</h2>
            <ul>
                {rooms.map((room) => (
                    <li key={room.roomId}>
                        {room.name} {room.pricePerHour}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default RoomList;
