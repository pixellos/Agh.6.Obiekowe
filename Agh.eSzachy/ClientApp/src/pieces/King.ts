import Piece from "./Piece";
import { Player } from "../Api";

export default class King extends Piece {
  constructor(player: Player) {
    super(
      player,
      player === Player.One ? "/4/42/Chess_klt45.svg" : "/f/f0/Chess_kdt45.svg"
    );
  }

  isMovePossible(src: number, dest: number): boolean {
    return (
      src - 9 === dest ||
      src - 8 === dest ||
      src - 7 === dest ||
      src + 1 === dest ||
      src + 9 === dest ||
      src + 8 === dest ||
      src + 7 === dest ||
      src - 1 === dest
    );
  }

  getSrcToDestPath(src: number, dest: number): number[] {
    return [];
  }
}
