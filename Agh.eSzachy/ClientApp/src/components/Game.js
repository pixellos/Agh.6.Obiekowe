import React from "react";

import Board from "./Board.js";
import FallenSoldierBlock from "./FallenSoldierBlock.js";
import initialiseChessBoard from "../helpers/board-initialiser";
import { GameHub } from "../Api.ts";
import { HubConnectionBuilder, HubConnectionState } from "@aspnet/signalr";
import authService from "./api-authorization/AuthorizeService";

export class Game extends React.Component {
  constructor(props) {
    super();
    this.state = {
      squares: Array(64).fill(null),
      whiteFallenSoldiers: [],
      blackFallenSoldiers: [],
      player: 1,
      sourceSelection: -1,
      status: "",
      turn: "white",
      gameHub: null,
      roomName: props.match?.params?.name || null,
      ready: false,
    };
  }

  componentDidMount() {
    (async () => {
      if (this.state.hub instanceof GameHub) {
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

          this.setState({ squares: initialiseChessBoard({ chessBoard }) });
        },
        send: (m) => {},
      });

      if (c.state === HubConnectionState.Disconnected) {
        await c.start();
        gameHub.refresh({ name: this.state.roomName });
        // gameHub.ready(this.state.roomName);

        // gameHub.map()
        this.setState({ gameHub });
      }
    })();
  }

  componentDidUpdate() {
    if (this.state.ready) {
      this.state.gameHub.ready(this.state.roomName);
      this.setState({ ready: false });
    }
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

          this.state.gameHub.move(
            this.state.roomName,
            { Col: columnFrom, Row: rowFrom },
            { Col: columnTo, Row: rowTo }
          );

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
    return (
      <div>
        <div style={{ marginBottom: "25px" }}>
          <button
            onClick={() => {
              this.setState({ ready: true });
            }}
          >
            Ready!
          </button>
        </div>

        <div className="game">
          <div className="game-board">
            <Board
              squares={this.state.squares}
              onClick={(i) => this.handleClick(i)}
            />
          </div>
          <div className="game-info">
            <h3>Turn</h3>
            <div
              id="player-turn-box"
              style={{ backgroundColor: this.state.turn }}
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
      </div>
    );
  }
}
