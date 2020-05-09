import Bishop from "../pieces/Bishop.js";
import King from "../pieces/King.js";
import Knight from "../pieces/Knight.js";
import Pawn from "../pieces/Pawn.js";
import Queen from "../pieces/Queen.js";
import Rook from "../pieces/Rook.js";
import Piece from "../pieces/Piece.js";
import { ChessBoardDto } from "../Api";

type initialiseChessBoardParams = {
  chessBoard: ChessBoardDto;
  playerOneColor: number;
  playerTwoColor: number;
};

export default function initialiseChessBoard({
  chessBoard,
}: initialiseChessBoardParams) {
  const squares = Array(64).fill(null);

  chessBoard.Pawns.forEach((pawn) => {
    const index = pawn.Row * 8 + pawn.Col;
    const player = pawn.IsPlayerOne ? 2 : 1;

    let pieceClass = null as any;

    switch (pawn.Type) {
      case "Pawn":
        pieceClass = Pawn;
        break;

      case "Rook":
        pieceClass = Rook;
        break;

      case "Knight":
        pieceClass = Knight;
        break;

      case "Bishop":
        pieceClass = Bishop;
        break;

      case "Queen":
        pieceClass = Queen;
        break;

      case "King":
        pieceClass = King;
        break;

      default:
        break;
    }

    if (pieceClass) {
      squares[index] = new pieceClass(player);
    }
  });

  return squares;
}
