import Piece from "./Piece";
import { Player } from "../Api";

type InitialPositions = {
  [playerId in Player]: number[];
};

export default class Pawn extends Piece {
  initialPositions: InitialPositions;

  constructor(player: Player) {
    super(
      player,
      player === Player.One ? "/4/45/Chess_plt45.svg" : "/c/c7/Chess_pdt45.svg"
    );
    this.initialPositions = {
      [Player.One]: [48, 49, 50, 51, 52, 53, 54, 55],
      [Player.Two]: [8, 9, 10, 11, 12, 13, 14, 15],
    };
  }

  isMovePossible(
    src: number,
    dest: number,
    isDestEnemyOccupied: boolean
  ): boolean {
    if (this.player === Player.One) {
      if (
        (dest === src - 8 && !isDestEnemyOccupied) ||
        (dest === src - 16 &&
          this.initialPositions[Player.One].indexOf(src) !== -1)
      ) {
        return true;
      } else if (
        isDestEnemyOccupied &&
        (dest === src - 9 || dest === src - 7)
      ) {
        return true;
      }
    } else if (this.player === Player.Two) {
      if (
        (dest === src + 8 && !isDestEnemyOccupied) ||
        (dest === src + 16 &&
          this.initialPositions[Player.Two].indexOf(src) !== -1)
      ) {
        return true;
      } else if (
        isDestEnemyOccupied &&
        (dest === src + 9 || dest === src + 7)
      ) {
        return true;
      }
    }
    return false;
  }

  getSrcToDestPath(src: number, dest: number): number[] {
    if (dest === src - 16) {
      return [src - 8];
    } else if (dest === src + 16) {
      return [src + 8];
    }
    return [];
  }
}
