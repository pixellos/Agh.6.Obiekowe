import Piece from "./Piece";
import { Player } from "../Api";

export default class Knight extends Piece {
  constructor(player: Player) {
    super(
      player,
      player === Player.One ? "/7/70/Chess_nlt45.svg" : "/e/ef/Chess_ndt45.svg"
    );
  }

  isMovePossible(src: number, dest: number): boolean {
    return (
      src - 17 === dest ||
      src - 10 === dest ||
      src + 6 === dest ||
      src + 15 === dest ||
      src - 15 === dest ||
      src - 6 === dest ||
      src + 10 === dest ||
      src + 17 === dest
    );
  }

  getSrcToDestPath(src: number, dest: number): number[] {
    return [];
  }
}
