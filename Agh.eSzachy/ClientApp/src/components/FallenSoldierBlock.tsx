import React, { ReactElement } from "react";
import Square from "./Square";

type FallenSoldierBlockProps = {
  whiteFallenSoldiers: any[];
  blackFallenSoldiers: any[];
};

const FallenSoldierBlock = (props: FallenSoldierBlockProps): ReactElement => (
  <div>
    <div className="board-row">
      {props.whiteFallenSoldiers.map((piece) => (
        <Square piece={piece} style={piece.style} />
      ))}
    </div>

    <div className="board-row">
      {props.blackFallenSoldiers.map((piece) => (
        <Square piece={piece} style={piece.style} />
      ))}
    </div>
  </div>
);

export default FallenSoldierBlock;
