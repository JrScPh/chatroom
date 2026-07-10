import type { Message } from '../types';

interface MessageListProps {
    currentUserNickname: string;
    messages: Message[];
}

function MessageList({ messages, currentUserNickname }: MessageListProps) {
    return (
        <div className='message-list'>
            {
                messages.map((message) => {
                    const readableTime = new Date(message.timestamp).toLocaleTimeString();
                    const isOwnMessage = message.nickname === currentUserNickname;
                    return (
                        <div key={message.id} className={isOwnMessage ? 'message own' : 'message other' }>
                            <strong>{message.nickname}</strong>
                            <p>{message.content}</p>
                            <span>{readableTime}</span>
                        </div>
                    );
                })
            }
        </div>
    );
}

export default MessageList;