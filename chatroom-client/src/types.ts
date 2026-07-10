export interface Room {
    id: number;
    title: string;
    description: string;
}

export interface Message {
    id: number;
    nickname: string;
    content: string;
    timestamp: string;
}