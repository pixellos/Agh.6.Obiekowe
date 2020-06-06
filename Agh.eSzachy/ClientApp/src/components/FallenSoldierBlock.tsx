import React, { ReactElement } from "react";

type FallenSoldierBlockProps = {
  whiteFallenSoldiers: any[];
  blackFallenSoldiers: any[];
};

const FallenSoldierBlock = ({
  whiteFallenSoldiers,
  blackFallenSoldiers,
}: FallenSoldierBlockProps): ReactElement => (
  <div>
    <div className="board-row">
      {whiteFallenSoldiers.map((piece, index) => (
        <div
          style={{
            backgroundImage: piece.backgroundImage,
          }}
          key={index}
        />
      ))}
    </div>

    <div className="board-row">
      {blackFallenSoldiers.map((piece, index) => (
        <div
          style={{
            backgroundImage: piece.backgroundImage,
          }}
          key={index}
        />
      ))}
    </div>
  </div>
);

export default FallenSoldierBlock;
