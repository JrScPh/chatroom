import * as signalR from '@microsoft/signalr';
import { useState } from 'react';
interface MessageInputProps {
    connectionRef: React.RefObject<signalR.HubConnection | null>;
    roomId: number;
    nickname: string;
}

function MessageInput({ connectionRef, roomId, nickname }: MessageInputProps) {
    const [content, setContent] = useState('');

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        connectionRef.current?.invoke('SendMessage', roomId, nickname, content);
        setContent('');
    };

    return (
        <form onSubmit={handleSubmit} className="message-input-form">
            <input value={content} onChange={(e) => setContent(e.target.value)} placeholder="Type a message..." />
            <button type="submit">Send</button>
        </form>
    )
}

export default MessageInput;