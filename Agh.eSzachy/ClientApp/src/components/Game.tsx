// @ts-nocheck
import React from "react";

import Board from "./Board";
import FallenSoldierBlock from "./FallenSoldierBlock";
import initialiseChessBoard from "../helpers/board-initialiser";
import { GameHub, ChessBoardDto, BoardState, Player } from "../Api";
import { HubConnectionBuilder, HubConnectionState } from "@aspnet/signalr";
import authService from "./api-authorization/AuthorizeService";
import "./Game.module.css";

type GameState = {
  squares: any[];
  whiteFallenSoldiers: never[];
  blackFallenSoldiers: never[];
  player: number;
  sourceSelection: number;
  status: string;
  turn: string;
  gameHub: null;
  roomName: any;
  chessBoard: ChessBoardDto;
};

export class Game extends React.Component<any, GameState> {
  constructor(props) {
    super(props);
    const s = {
      squares: Array(64).fill(null),
      whiteFallenSoldiers: [],
      blackFallenSoldiers: [],
      sourceSelection: -1,
      status: "",
      turn: "white",
      gameHub: null,
      roomName: props.roomName || props.match?.params?.name || null,
      chessBoard: null as ChessBoardDto,
    };
    this.state = s;
  }

  componentDidMount() {
    (async () => {
      if (this.state.gameHub instanceof GameHub) {
        return;
      }

      const token = await authService.getAccessToken();
      if (typeof token !== "string") {
        throw new Error();
      }

      const c = new HubConnectionBuilder()
        .withUrl("/game", { accessTokenFactory: () => token })
        .build();

      const gameHub = new GameHub(c);
      gameHub.registerCallbacks({
        refresh: (roomName, chessBoard) => {
          // setRoomList(r);
          console.log(
            "gameHub.registerCallbacks.refresh",
            roomName,
            chessBoard
          );

          this.setState({
            chessBoard: chessBoard,
            squares: initialiseChessBoard({ chessBoard }),
          });
        },
        send: (m) => {},
      });

      if (c.state === HubConnectionState.Disconnected) {
        await c.start();
        gameHub.refresh({ name: this.state.roomName });
        this.setState({ gameHub });
      }
    })();
  }

  handleClick(i): void {
    const squares = this.state.squares.slice();
    const source = this.state.sourceSelection;

    if (source === -1) {
      if (!squares[i].piece) {
        return;
      }

      squares[i].className = "selected";

      return this.setState({
        status: "Choose destination for the selected piece",
        sourceSelection: i,
      });
    }

    squares[i].className = "";

    if (source === i) {
      return this.setState({
        sourceSelection: -1,
        status: "",
      });
    }

    if (
      squares[i].piece &&
      squares[i].piece.player === this.state.chessBoard.Player
    ) {
      return this.setState({
        status: "Wrong selection. Choose valid source and destination again.",
        sourceSelection: -1,
      });
    }

    const whiteFallenSoldiers = this.state.whiteFallenSoldiers.slice();
    const blackFallenSoldiers = this.state.blackFallenSoldiers.slice();
    const isDestEnemyOccupied = squares[i].piece ? true : false;
    const isMovePossible = squares[source].piece.isMovePossible(
      source,
      i,
      isDestEnemyOccupied
    );
    const srcToDestPath = squares[source].piece.getSrcToDestPath(source, i);
    const isMoveLegal = this.isMoveLegal(srcToDestPath);

    if (!isMovePossible) {
      return this.setState({
        status: "This move is not possible, select destination again",
      });
    }

    if (!isMoveLegal) {
      return this.setState({
        status: "This move is not legal, select destination again",
      });
    }

    if (squares[i].piece !== null) {
      if (squares[i].piece.player === Player.One) {
        whiteFallenSoldiers.push(squares[i].piece);
      } else {
        blackFallenSoldiers.push(squares[i].piece);
      }
    }

    squares[i].piece = squares[source].piece;
    squares[source].piece = null;

    const columnFrom = source % 8;
    const rowFrom = Math.floor(source / 8);

    const columnTo = i % 8;
    const rowTo = Math.floor(i / 8);

    squares[source].className = "";

    try {
      this.state.gameHub.move(
        this.state.roomName,
        { Col: columnFrom, Row: rowFrom },
        { Col: columnTo, Row: rowTo }
      );
    } catch (e) {
      console.debug("move", e);
    }

    const player =
      this.state.chessBoard.Player === Player.One ? Player.Two : Player.One;

    const turn = this.state.turn === "white" ? "black" : "white";
    this.setState({
      sourceSelection: -1,
      squares,
      whiteFallenSoldiers,
      blackFallenSoldiers,
      player,
      status: "",
      turn,
    });
  }

  isMoveLegal(srcToDestPath) {
    let isLegal = true;

    for (let i = 0; i < srcToDestPath.length; i++) {
      if (this.state.squares[srcToDestPath[i]].piece !== null) {
        isLegal = false;
      }
    }
    return isLegal;
  }

  render() {
    const playerSection = (
      <div className="grid-container">
        <div className="grid-item">
          <div id="player-turn-box" style={{ backgroundColor: "white" }}></div>
          <div>First Player:</div>
          <div>{this.state.chessBoard?.PlayerOne?.Name}</div>
        </div>
        <div className="grid-item">
          <div id="player-turn-box" style={{ backgroundColor: "black" }}></div>
          <div>Second Player:</div>
          <div>{this.state.chessBoard?.PlayerTwo?.Name}</div>
        </div>
      </div>
    );

    const content =
      this.state?.chessBoard?.State !== BoardState.Started ? (
        <div>
          <button
            onClick={() => {
              this.state.gameHub.ready(this.state.roomName);
            }}
          >
            Ready!
          </button>
          <div>Waiting for 2. player</div>
        </div>
      ) : (
        <div className="game">
          <div className="game-board">
            <Board
              squares={this.state.squares}
              onClick={(i) => this.handleClick(i)}
            />
          </div>
          <div className="game-info">
            <h3>
              Turn{" "}
              {this.state.chessBoard.Player === Player.One
                ? this.state.chessBoard.PlayerOne.Name
                : this.state.chessBoard.PlayerTwo.Name}
            </h3>
            <div
              id="player-turn-box"
              style={{
                backgroundColor:
                  this.state.chessBoard.Player === Player.One
                    ? "white"
                    : "black",
              }}
            ></div>

            <div className="game-status">{this.state.status}</div>
            <div className="fallen-soldier-block">
              {
                <FallenSoldierBlock
                  whiteFallenSoldiers={this.state.whiteFallenSoldiers}
                  blackFallenSoldiers={this.state.blackFallenSoldiers}
                />
              }
            </div>
          </div>
        </div>
      );

    return (
      <div>
        <div style={{ marginBottom: "25px" }}></div>
        {playerSection}
        <div style={{ marginBottom: "25px" }}></div>
        {content}
      </div>
    );
  }
}
