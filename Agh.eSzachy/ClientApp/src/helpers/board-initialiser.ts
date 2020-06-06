import Bishop from "../pieces/Bishop";
import King from "../pieces/King";
import Knight from "../pieces/Knight";
import Pawn from "../pieces/Pawn";
import Queen from "../pieces/Queen";
import Rook from "../pieces/Rook";
import { ChessBoardDto, Player } from "../Api";

type initialiseChessBoardParams = {
  chessBoard: ChessBoardDto;
  playerOneColor: number;
  playerTwoColor: number;
};

const initialiseChessBoard = ({ chessBoard }: initialiseChessBoardParams) => {
  const squares = Array.from({ length: 64 }, () => ({
    className: "",
    available: false,
    piece: null,
  }));

  chessBoard.Pawns.forEach((pawn) => {
    const index = pawn.Row * 8 + pawn.Col;
    const player = pawn.IsPlayerOne ? Player.Two : Player.One;

    let PieceClass = null as any;

    switch (pawn.Type) {
      case "Pawn":
        PieceClass = Pawn;
        break;

      case "Rook":
        PieceClass = Rook;
        break;

      case "Knight":
        PieceClass = Knight;
        break;

      case "Bishop":
        PieceClass = Bishop;
        break;

      case "Queen":
        PieceClass = Queen;
        break;

      case "King":
        PieceClass = King;
        break;

      default:
        break;
    }

    if (PieceClass) {
      const piece = new PieceClass(player);
      squares[index].piece = piece;
    }
  });

  return { squares };
};

export default initialiseChessBoard;
