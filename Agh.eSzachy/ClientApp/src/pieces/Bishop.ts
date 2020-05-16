import Piece from "./Piece";
import { Player } from "../Api";

export default class Bishop extends Piece {
  constructor(player: Player) {
    super(
      player,
      player === Player.One ? "/b/b1/Chess_blt45.svg" : "/9/98/Chess_bdt45.svg"
    );
  }

  isMovePossible(src: number, dest: number): boolean {
    const distance = Math.abs(src - dest);

    return distance % 9 === 0 || distance % 7 === 0;
  }

  getSrcToDestPath(src: number, dest: number): number[] {
    const distance = Math.abs(src - dest);

    let path: number[] = [],
      pathStart,
      pathEnd,
      incrementBy;
    if (src > dest) {
      pathStart = dest;
      pathEnd = src;
    } else {
      pathStart = src;
      pathEnd = dest;
    }
    if (distance % 9 === 0) {
      incrementBy = 9;
      pathStart += 9;
    } else {
      incrementBy = 7;
      pathStart += 7;
    }

    for (let i = pathStart; i < pathEnd; i += incrementBy) {
      path.push(i);
    }
    return path;
  }
}
