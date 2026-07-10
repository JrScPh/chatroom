import { useState, useEffect, useRef } from 'react';
import type { Room, Message } from './types';
import * as signalR from '@microsoft/signalr';
import './App.css';
import RoomHeader from './components/RoomHeader';
import MessageList from './components/MessageList';
import MessageInput from './components/MessageInput';

function App() {
    const [room, setRoom] = useState<Room | null>(null);
    const [messages, setMessages] = useState<Message[]>([]);
    const [nickname, setNickname] = useState<string>('');
    const connectionRef = useRef<signalR.HubConnection | null>(null);

    // load initial data, then signalR will start
    useEffect(() => {
        const fetchData = async () => {
            const roomRes = await fetch('https://localhost:7008/api/rooms/1');
            const roomData = await roomRes.json();
            setRoom(roomData);

            const messagesRes = await fetch('https://localhost:7008/api/rooms/1/messages');
            const messagesData = await messagesRes.json();
            setMessages(messagesData);
        };

        fetchData();
    }, []);

    useEffect(() => {
        // create connection but don't start until after initial fetch completed
        const connection = new signalR.HubConnectionBuilder()
            .withUrl('https://localhost:7008/hubs/chat')
            .withAutomaticReconnect() // built-in reconnect
            .configureLogging(signalR.LogLevel.Information)
            .build();
        connectionRef.current = connection;

        connection.on('ReceiveMessage', (message: Message) => {
            setMessages(prev => [...prev, message]);
        });

        // start with retry/backoff on transient negotiate aborts
        let isMounted = true;
        const startWithRetry = async (attempt = 0) => {
            if (!isMounted) return;
            try {
                await connection.start();
                console.log('SignalR connected');
            } catch (err) {
                console.error(`SignalR connection error (attempt ${attempt}):`, err);
                // negotiation aborted often happens during dev reloads; retry with backoff
                const delay = Math.min(30000, 1000 * Math.pow(2, attempt)); // cap 30s
                setTimeout(() => startWithRetry(attempt + 1), delay);
            }
        };

        // Start only once initial room data is present (avoid starting while Vite reloads)
        if (room !== null) {
            startWithRetry();
        } else {
            // fallback: if room not loaded yet, start when it becomes available
            const observer = new MutationObserver(() => {
                if (room !== null) {
                    startWithRetry();
                    observer.disconnect();
                }
            });
            // dummy observe to allow later start; will disconnect when started
            observer.observe(document, { childList: true, subtree: true });
        }

        return () => {
            isMounted = false;
            connection.stop().catch(() => { /* ignore stop errors on unmount */ });
        };
    }, [room]); // depend on room so start happens after initial fetch

    useEffect(() => {
        const name = prompt('Enter your nickname:') || 'Anonymous';
        setNickname(name);
    }, []
    );

    return (
        <div className='app-container'>
            <RoomHeader title={room?.title ?? 'Loading...'} description={room?.description ?? ''} />
            <MessageList messages={messages} currentUserNickname={nickname} />
            <MessageInput connectionRef={connectionRef} roomId={room?.id} nickname={nickname} />
        </div>
  );
}

export default App;
