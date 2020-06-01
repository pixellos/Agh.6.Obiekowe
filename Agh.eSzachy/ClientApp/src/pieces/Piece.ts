import { Player } from "../Api";

export default class Piece {
  baseIconUrl = "https://upload.wikimedia.org/wikipedia/commons";
  player: Player;
  backgroundImage: string;

  constructor(player: Player, iconEndpoint: string) {
    this.player = player;
    this.backgroundImage = `url("${this.baseIconUrl}${iconEndpoint}")`;
  }
}
