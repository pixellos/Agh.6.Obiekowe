import Piece from "./Piece";
import { Player } from "../Api";

export default class Rook extends Piece {
  constructor(player: Player) {
    super(
      player,
      player === Player.One ? "/7/72/Chess_rlt45.svg" : "/f/ff/Chess_rdt45.svg"
    );
  }

  isMovePossible(src: number, dest: number): boolean {
    const distance = Math.abs(src - dest);

    let mod = src % 8;
    let diff = 8 - mod;
    return distance % 8 === 0 || (dest >= src - mod && dest < src + diff);
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
    if (distance % 8 === 0) {
      incrementBy = 8;
      pathStart += 8;
    } else {
      incrementBy = 1;
      pathStart += 1;
    }

    for (let i = pathStart; i < pathEnd; i += incrementBy) {
      path.push(i);
    }
    return path;
  }
}
