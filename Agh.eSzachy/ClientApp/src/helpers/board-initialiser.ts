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

export default function initialiseChessBoard({
  chessBoard,
}: initialiseChessBoardParams) {
  const squares = Array(64).fill(null);

  chessBoard.Pawns.forEach((pawn) => {
    const index = pawn.Row * 8 + pawn.Col;
    const player = pawn.IsPlayerOne ? Player.Two : Player.One;

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
