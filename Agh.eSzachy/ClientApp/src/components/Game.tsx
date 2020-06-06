// @ts-nocheck
import React from "react";
import classNames from "classnames";
import { format } from "timeago.js";
import Board from "./Board";
import FallenSoldierBlock from "./FallenSoldierBlock";
import initialiseChessBoard from "../helpers/board-initialiser";
import { GameHub, RoomHub, ChessBoardDto, BoardState, Player } from "../Api";
import { HubConnectionBuilder, HubConnectionState } from "@aspnet/signalr";
import authService from "./api-authorization/AuthorizeService";
import styles from "./Game.module.scss";

type GameState = {
  squares: any[];
  whiteFallenSoldiers: never[];
  blackFallenSoldiers: never[];
  sourceSelection: number;
  status: string;
  gameHub: any;
  roomHub: any;
  roomName: any;
  chessBoard: ChessBoardDto;
  userId: string;
  userName: string;
  messages: string[];
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
      gameHub: null,
      roomHub: null,
      roomName: props.roomName || props.match?.params?.name || null,
      chessBoard: null as ChessBoardDto,
      messages: [],
    };
    this.state = s;
    this.messagesRef = React.createRef();
    this.messageInputRef = React.createRef();
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

      const user = await authService.getUser();

      this.setState({ userId: user.sub, userName: user.name });

      const gameConnection = new HubConnectionBuilder()
        .withUrl("/game", { accessTokenFactory: () => token })
        .build();

      const gameHub = new GameHub(gameConnection);
      gameHub.registerCallbacks({
        refresh: (roomName, chessBoard) => {
          const initialisedChessBoard = initialiseChessBoard({ chessBoard });

          this.setState({
            chessBoard,
            squares: initialisedChessBoard.squares,
            status: "",
          });
        },
      });

      if (gameConnection.state === HubConnectionState.Disconnected) {
        await gameConnection.start();
        gameHub.refresh({ name: this.state.roomName });
        this.setState({ gameHub });
      }

      if (this.state.roomHub instanceof RoomHub) {
        return;
      }

      const roomConnection = new HubConnectionBuilder()
        .withUrl("/room", { accessTokenFactory: () => token })
        .build();

      const roomHub = new RoomHub(roomConnection);
      roomHub.registerCallbacks({
        refresh: async (rooms: Room[]) => {
          const room = rooms.find((room) => room.Name === this.state.roomName);

          this.setState({
            messages: room.Messages.sort((a, b) => {
              const timeA = new Date(a.Created).getTime();
              const timeB = new Date(b.Created).getTime();

              return timeA - timeB;
            }),
          });

          this.messagesRef.current.scrollTo({
            top: this.messagesRef.current.scrollHeight,
          });
        },
      });

      switch (roomConnection.state) {
        case HubConnectionState.Disconnected:
          await roomConnection.start();

          this.setState({ roomHub });
          break;
      }
    })();
  }

  handleClick(i): void {
    const squares = this.state.squares.slice();
    const source = this.state.sourceSelection;

    const currentUserId =
      this.state.chessBoard.Player === Player.One
        ? this.state.chessBoard.PlayerOne.Id
        : this.state.chessBoard.PlayerTwo.Id;

    if (this.state.userId !== currentUserId) {
      return this.setState({
        status: "It's your opponent's turn",
      });
    }

    if (source === -1) {
      if (!squares[i].piece) {
        return this.setState({
          status: "",
        });
      }

      if (squares[i].piece.player !== this.state.chessBoard.Player) {
        return this.setState({
          status: "Choose your piece",
        });
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

    this.state.gameHub.move(
      this.state.roomName,
      { Col: columnFrom, Row: rowFrom },
      { Col: columnTo, Row: rowTo }
    );

    this.setState({
      sourceSelection: -1,
      squares,
      whiteFallenSoldiers,
      blackFallenSoldiers,
      status: "",
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

  sendMessage() {
    if (this.state.message && this.state.roomHub) {
      this.state.roomHub.send(this.state.roomName, this.state.message);

      this.setState({ message: "" });
    }
  }

  render() {
    const hasGameStarted = this.state?.chessBoard?.State === BoardState.Started;

    return (
      <>
        <div
          className={styles.BackButton}
          onClick={() => this.props.history.push("/")}
        >
          ⬅ Back to room list
        </div>

        <div className={styles.Players}>
          <div
            className={classNames(styles.PlayerInfo, {
              [styles.PlayerSelected]:
                this.state.chessBoard?.Player === Player.One,
            })}
          >
            <div className={classNames(styles.PlayerColor, styles.White)}></div>
            <div>{this.state.chessBoard?.PlayerOne?.Name}</div>
          </div>
          <div
            className={classNames(styles.PlayerInfo, {
              [styles.PlayerSelected]:
                this.state.chessBoard?.Player === Player.Two,
            })}
          >
            <div className={classNames(styles.PlayerColor, styles.Black)}></div>
            <div>{this.state.chessBoard?.PlayerTwo?.Name}</div>
          </div>
        </div>

        {(!hasGameStarted && (
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
        )) || (
          <>
            <div className={styles.Status}>{this.state.status}</div>
            <Board
              sourceSelection={this.state.sourceSelection}
              squares={this.state.squares}
              onClick={(i) => this.handleClick(i)}
            />
            <div className="game-info">
              <div className="fallen-soldier-block">
                <FallenSoldierBlock
                  whiteFallenSoldiers={this.state.whiteFallenSoldiers}
                  blackFallenSoldiers={this.state.blackFallenSoldiers}
                />
              </div>
            </div>

            <div className={styles.MessagesContainer}>
              <ol className={styles.Messages} ref={this.messagesRef}>
                {this.state.messages.map((message) => (
                  <li
                    className={classNames(styles.Message, {
                      [styles.Mine]:
                        this.state.userName.toLowerCase() ===
                        message.UserName.toLowerCase(),
                    })}
                    key={message.Created}
                    title={new Date(message.Created).toUTCString()}
                  >
                    {message.Text}
                  </li>
                ))}
              </ol>
            </div>

            <div className={styles.SendWrapper}>
              <input
                ref={this.messageInputRef}
                className={styles.MessageInput}
                type="text"
                value={this.state.message}
                onChange={(x) => {
                  this.setState({ message: x.target.value });
                }}
                onKeyPress={(e) => {
                  if (e.key === "Enter") {
                    this.setState(
                      {
                        message: this.messageInputRef.current.value,
                      },
                      this.sendMessage
                    );
                  }
                }}
              />
              <button onClick={(x) => this.sendMessage()}>Send</button>
            </div>
          </>
        )}
      </>
    );
  }
}
