import { Player } from "../Api";

export default class Piece {
  baseIconUrl = "https://upload.wikimedia.org/wikipedia/commons";
  player: Player;
  style: Record<string, string>;

  constructor(player: Player, iconEndpoint: string) {
    this.player = player;
    this.style = {
      backgroundImage: `url("${this.baseIconUrl}${iconEndpoint}")`,
    };
  }
}
