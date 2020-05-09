// @ts-nocheck
import React from "react";

import Board from "./Board.js";
import FallenSoldierBlock from "./FallenSoldierBlock";
import initialiseChessBoard from "../helpers/board-initialiser";
import { GameHub, ChessBoardDto, BoardState, Player } from "../Api";
import { HubConnectionBuilder, HubConnectionState } from "@aspnet/signalr";
import authService from "./api-authorization/AuthorizeService";
import "./Game.module.css";

type stateType = {
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

export class Game extends React.Component<any, stateType> {
  constructor(props) {
    super(props);
    const s = {
      squares: Array(64).fill(null),
      whiteFallenSoldiers: [],
      blackFallenSoldiers: [],
      player: 1,
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
            player: chessBoard.Player,
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

  handleClick(i) {
    const squares = this.state.squares.slice();

    if (this.state.sourceSelection === -1) {
      // if (!squares[i] || squares[i].player !== this.state.player) {
      //   this.setState({
      //     status:
      //       "Wrong selection. Choose player " + this.state.player + " pieces.",
      //   });
      //   if (squares[i]) {
      //     squares[i].style = { ...squares[i].style, backgroundColor: "" };
      //   }
      // } else {
      squares[i].style = {
        ...squares[i].style,
        backgroundColor: "rgb(111, 143, 114)",
      };

      // for (let index = 0; index < 64; index++) {
      //   if (index === i) continue;

      //   const isMovePossible = squares[i].isMovePossible(i, index);
      //   const srcToDestPath = squares[i].getSrcToDestPath(i, index);
      //   const isMoveLegal = this.isMoveLegal(srcToDestPath);

      //   if (isMovePossible && isMoveLegal) {
      //     squares[index].style = {
      //       ...squares[index].style,
      //       backgroundColor: "rgb(222, 143, 114)",
      //     };
      //   }
      // }

      this.setState({
        status: "Choose destination for the selected piece",
        sourceSelection: i,
      });
      // }
    } else if (this.state.sourceSelection > -1) {
      squares[this.state.sourceSelection].style = {
        ...squares[this.state.sourceSelection].style,
        backgroundColor: "",
      };
      if (squares[i] && squares[i].player === this.state.player) {
        this.setState({
          status: "Wrong selection. Choose valid source and destination again.",
          sourceSelection: -1,
        });
      } else {
        const squares = this.state.squares.slice();
        const whiteFallenSoldiers = this.state.whiteFallenSoldiers.slice();
        const blackFallenSoldiers = this.state.blackFallenSoldiers.slice();
        const isDestEnemyOccupied = squares[i] ? true : false;
        const isMovePossible = squares[
          this.state.sourceSelection
        ].isMovePossible(this.state.sourceSelection, i, isDestEnemyOccupied);
        const srcToDestPath = squares[
          this.state.sourceSelection
        ].getSrcToDestPath(this.state.sourceSelection, i);
        const isMoveLegal = this.isMoveLegal(srcToDestPath);

        if (isMovePossible && isMoveLegal) {
          if (squares[i] !== null) {
            if (squares[i].player === 1) {
              whiteFallenSoldiers.push(squares[i]);
            } else {
              blackFallenSoldiers.push(squares[i]);
            }
          }
          squares[i] = squares[this.state.sourceSelection];
          squares[this.state.sourceSelection] = null;

          const columnFrom = this.state.sourceSelection % 8;
          const rowFrom = Math.floor(this.state.sourceSelection / 8);

          const columnTo = i % 8;
          const rowTo = Math.floor(i / 8);
          console.log(columnFrom, rowFrom, columnTo, rowTo);
          try {
            this.state.gameHub.move(
              this.state.roomName,
              { Col: columnFrom, Row: rowFrom },
              { Col: columnTo, Row: rowTo }
            );
          } catch (e) {
            console.debug("move", e);
          }

          const player = this.state.player === 1 ? 2 : 1;
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
        } else {
          this.setState({
            status:
              "Wrong selection. Choose valid source and destination again.",
            sourceSelection: -1,
          });
        }
      }
    }
  }

  /**
   * Check all path indices are null. For one steps move of pawn/others or jumping moves of knight array is empty, so  move is legal.
   * @param  {[type]}  srcToDestPath [array of board indices comprising path between src and dest ]
   * @return {Boolean}
   */
  isMoveLegal(srcToDestPath) {
    let isLegal = true;
    for (let i = 0; i < srcToDestPath.length; i++) {
      if (this.state.squares[srcToDestPath[i]] !== null) {
        isLegal = false;
      }
    }
    return isLegal;
  }

  render() {
    const playerSection = (
      <div class="grid-container">
        <div class="grid-item">
          <div id="player-turn-box" style={{ backgroundColor: "white" }}></div>
          <div>First Player:</div>
          <div>{this.state.chessBoard?.PlayerOne?.Name}</div>
        </div>
        <div class="grid-item">
          <div id="player-turn-box" style={{ backgroundColor: "black" }}></div>
          <div>Second Player:</div>
          <div>{this.state.chessBoard?.PlayerTwo?.Name}</div>
        </div>
      </div>
    );

    const content =
      this.state?.chessBoard?.State != BoardState.Started ? (
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
              {this.state.chessBoard.Player == Player.One
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
