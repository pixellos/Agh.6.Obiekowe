import { HubConnection, IStreamResult } from "@aspnet/signalr"

export class RoomHub {
    constructor(private connection: HubConnection) {
    }

    create(roomName: string): Promise<void> {
        return this.connection.invoke('Create', roomName);
    }

    join(roomName: string): Promise<void> {
        return this.connection.invoke('Join', roomName);
    }

    leave(roomName: string): Promise<void> {
        return this.connection.invoke('Leave', roomName);
    }

    send(roomId: string, message: string): Promise<void> {
        return this.connection.invoke('Send', roomId, message);
    }

    registerCallbacks(implementation: IRoomHubCallbacks) {
        this.connection.on('RefreshSingle', (r) => implementation.refreshSingle(r));
        this.connection.on('Refresh', (r) => implementation.refresh(r));
        this.connection.on('Send', (message) => implementation.send(message));
    }

    unregisterCallbacks(implementation: IRoomHubCallbacks) {
        this.connection.off('RefreshSingle', (r) => implementation.refreshSingle(r));
        this.connection.off('Refresh', (r) => implementation.refresh(r));
        this.connection.off('Send', (message) => implementation.send(message));
    }
}

export interface IRoomHubCallbacks {
    refreshSingle(r: Room): void;
    refresh(r: Room[]): void;
    send(message: string): void;
}

export class GameHub {
    constructor(private connection: HubConnection) {
    }

    move(roomName: string, from: string, to: string): Promise<void> {
        return this.connection.invoke('Move', roomName, from, to);
    }

    ready(roomName: string): Promise<void> {
        return this.connection.invoke('Ready', roomName);
    }

    surrender(roomName: string): Promise<void> {
        return this.connection.invoke('Surrender', roomName);
    }

    registerCallbacks(implementation: IGameHubCallbacks) {
    }

    unregisterCallbacks(implementation: IGameHubCallbacks) {
    }
}

export interface IGameHubCallbacks {
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