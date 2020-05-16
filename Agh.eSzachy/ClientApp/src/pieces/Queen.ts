import Piece from "./Piece";
import { Player } from "../Api";

export default class Queen extends Piece {
  constructor(player: Player) {
    super(
      player,
      player === Player.One ? "/1/15/Chess_qlt45.svg" : "/4/47/Chess_qdt45.svg"
    );
  }

  isMovePossible(src: number, dest: number): boolean {
    const distance = Math.abs(src - dest);

    let mod = src % 8;
    let diff = 8 - mod;

    return (
      distance % 9 === 0 ||
      distance % 7 === 0 ||
      distance % 8 === 0 ||
      (dest >= src - mod && dest < src + diff)
    );
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
    } else if (distance % 9 === 0) {
      incrementBy = 9;
      pathStart += 9;
    } else if (distance % 7 === 0) {
      incrementBy = 7;
      pathStart += 7;
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
