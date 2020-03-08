import { HubConnection, IStreamResult } from "@aspnet/signalr"

export class ChatHub {
    constructor(private connection: HubConnection) {
    }

    send(message: string): Promise<void> {
        return this.connection.invoke('Send', message);
    }

    registerCallbacks(implementation: IChatHubCallbacks) {
        this.connection.on('Send', (message) => implementation.send(message));
    }

    unregisterCallbacks(implementation: IChatHubCallbacks) {
        this.connection.off('Send', (message) => implementation.send(message));
    }
}

export interface IChatHubCallbacks {
    send(message: string): void;
}

export class RoomHub {
    constructor(private connection: HubConnection) {
    }

    joinRoom(userId: string, roomId: string): Promise<boolean> {
        return this.connection.invoke('JoinRoom', userId, roomId);
    }

    registerCallbacks(implementation: IRoomHubCallbacks) {
        this.connection.on('RoomStatus', (message, messageType) => implementation.roomStatus(message, messageType));
    }

    unregisterCallbacks(implementation: IRoomHubCallbacks) {
        this.connection.off('RoomStatus', (message, messageType) => implementation.roomStatus(message, messageType));
    }
}

export interface IRoomHubCallbacks {
    roomStatus(message: string, messageType: MessageType): void;
}

export enum MessageType {
    Leave = 0,
    Win = 1,
    Joined = 2,
}