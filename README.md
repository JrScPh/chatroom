# ChatroomAPI

A real-time chat application built as a guided learning project, covering a full-stack flow from a .NET Web API backend through SignalR real-time messaging to a React/TypeScript frontend.

**Status: Alpha** — core chat flow (send, broadcast, receive, render) is working end-to-end with a themed UI.

## Stack

- **Backend:** ASP.NET Core Web API (.NET 8), C#
- **Real-time:** SignalR (`ChatHub` at `/hubs/chat`)
- **Database:** SQL Server 2022 (Docker container), Entity Framework Core 8
- **Frontend:** React + TypeScript (Vite), `@microsoft/signalr` client

## Features (Alpha)

- Room title/description display, pinned at the top of the page
- Message history loaded on page load via REST
- Real-time message send/receive via SignalR (no page refresh required)
- Messages visually aligned left/right based on sender, DM-style
- Dark, monospace-accented UI theme
- Simple nickname prompt on load (no auth yet)

## Project Structure

```
ChatroomAPI/
├── Models/
│   ├── Room.cs
│   └── Message.cs
├── Data/
│   └── AppDbContext.cs
├── DTOs/
│   ├── CreateMessageDto.cs
│   ├── RoomDto.cs
│   └── MessageDto.cs
├── Controllers/
│   └── RoomsController.cs      — GetRoom, GetMessages, PostMessage
├── Hubs/
│   └── ChatHub.cs              — SendMessage: saves to DB, broadcasts via SignalR
├── Program.cs
├── appsettings.json
├── docker-compose.yml          — SQL Server 2022 container
└── chatroom-client/            — Vite + React + TypeScript frontend
    └── src/
        ├── types.ts
        ├── App.tsx             — root state (room, messages, nickname), SignalR connection
        ├── App.css             — layout + design tokens
        └── components/
            ├── RoomHeader.tsx
            ├── MessageList.tsx
            └── MessageInput.tsx
```

## How Messages Flow

1. `MessageInput` calls `connection.invoke('SendMessage', roomId, nickname, content)` over the SignalR connection.
2. `ChatHub.SendMessage` saves the message to the database via EF Core, then broadcasts it to all connected clients with `Clients.All.SendAsync("ReceiveMessage", messageDto)`.
3. Every connected client's `connection.on('ReceiveMessage', ...)` handler fires, appending the new message into React state.
4. React re-renders `MessageList` with the updated message list.

Note: sending is handled exclusively through the SignalR hub, not the REST `PostMessage` endpoint — the REST endpoint persists data but does not broadcast, so it's not used for the live send flow.

## Running Locally

1. Start the database:
   ```bash
   docker compose up -d
   ```
2. Run EF Core migrations (if not already applied) to create `ChatroomDB`.
3. Start the API (from the project root):
   ```bash
   dotnet run
   ```
4. Start the frontend:
   ```bash
   cd chatroom-client
   npm install
   npm run dev
   ```
5. Open the app in two browser windows to see real-time updates between "users."

## Roadmap

- [ ] `Sidebar` component (room list / switching)
- [ ] Auto-scroll to newest message in `MessageList`
- [ ] Empty state for `MessageList` (no messages yet)
- [ ] AI bot users via Anthropic API
- [ ] Content moderation (G/PG threshold via LLM)
- [ ] Docker Compose for full stack (API + React + SQL Server together)
- [ ] GitHub Actions CI/CD pipeline
- [ ] Azure deployment
- [ ] User authentication (replacing the nickname prompt)

## Known Issues

- In development, the nickname prompt fires twice due to React 18 Strict Mode double-invoking effects on mount. Cosmetic only, not present in production builds. Fix: guard the effect with a `useRef` flag.
