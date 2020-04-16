import { HubConnection, IStreamResult } from "@aspnet/signalr"

export class RoomHub {
    constructor(private connection: HubConnection) {
    }

    join(roomId: string): Promise<void> {
        return this.connection.invoke('Join', roomId);
    }

    leave(roomId: string): Promise<void> {
        return this.connection.invoke('Leave', roomId);
    }

    send(roomId: string, message: string): Promise<void> {
        return this.connection.invoke('Send', roomId, message);
    }

    registerCallbacks(implementation: IRoomHubCallbacks) {
        this.connection.on('Refresh', (r) => implementation.refresh(r));
        this.connection.on('Send', (message) => implementation.send(message));
    }

    unregisterCallbacks(implementation: IRoomHubCallbacks) {
        this.connection.off('Refresh', (r) => implementation.refresh(r));
        this.connection.off('Send', (message) => implementation.send(message));
    }
}

export interface IRoomHubCallbacks {
    refresh(r: Room[]): void;
    send(message: string): void;
}

export interface Room {
    Messages: Message[] | undefined;
    Id: string | undefined;
    Name: string | undefined;
    Created: Date;
}

export interface Message {
    Text: string | undefined;
    UserId: string | undefined;
    Created: Date;
}