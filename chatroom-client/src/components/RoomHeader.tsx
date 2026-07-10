interface RoomHeaderProps {
    title: string;
    description: string;
}

function RoomHeader({ title, description }: RoomHeaderProps) {
    return (
        <div className='room-header'>
            <h1>{title}</h1>
            <p>{description}</p>
        </div>
    );
}

export default RoomHeader;