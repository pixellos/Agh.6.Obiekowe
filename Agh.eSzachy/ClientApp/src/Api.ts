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

    getAllRooms(): Promise<string[]> {
        return this.connection.invoke('GetAllRooms');
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

    move(roomName: string, from: PawnPosition, to: PawnPosition): Promise<void> {
        return this.connection.invoke('Move', roomName, from, to);
    }

    subscribe(roomName: string): Promise<void> {
        return this.connection.invoke('Subscribe', roomName);
    }

    map(model: ChessBoardModel): Promise<ChessBoard> {
        return this.connection.invoke('Map', model);
    }

    mapHistory(model: ChessBoardModel): Promise<ChessBoardHistory> {
        return this.connection.invoke('MapHistory', model);
    }

    ready(roomName: string): Promise<void> {
        return this.connection.invoke('Ready', roomName);
    }

    historicalFor(roomName: string): Promise<ChessBoardHistory[]> {
        return this.connection.invoke('HistoricalFor', roomName);
    }

    surrender(roomName: string): Promise<void> {
        return this.connection.invoke('Surrender', roomName);
    }

    registerCallbacks(implementation: IGameHubCallbacks) {
        this.connection.on('Refresh', (roomName, cb) => implementation.refresh(roomName, cb));
    }

    unregisterCallbacks(implementation: IGameHubCallbacks) {
        this.connection.off('Refresh', (roomName, cb) => implementation.refresh(roomName, cb));
    }
}

export interface IGameHubCallbacks {
    refresh(roomName: string, cb: ChessBoard): void;
}

export interface Room {
    Messages: Message[];
    Id: string;
    Name: string;
    Created: Date;
}

export interface Message {
    Text: string;
    UserId: string;
    Created: Date;
}

export interface PawnPosition {
    Row: number;
    Col: number;
}

export interface ChessBoardModel {
    Board: { [key: string]: BasePawn; };
    Started: Date;
    LastMove: Date;
}

export interface BasePawn {
    player: Player;
}

export enum Player {
    One = 0,
    Two = 1,
}

export interface ChessBoard {
    State: BoardState;
    Pawns: Pawn[];
}

export enum BoardState {
    Idle = 0,
    PlayerOneWins = 1,
    PlayerTwoWins = 2,
    Started = 3,
    Draw = 4,
}

export interface Pawn {
    IsPlayerOne: boolean;
    Type: string;
    Row: number;
    Col: number;
}

export interface ChessBoardHistory {
    History: { [key: string]: ChessBoard; };
}