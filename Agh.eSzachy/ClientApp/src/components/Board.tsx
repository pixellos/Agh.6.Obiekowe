import React, { ReactElement } from "react";
import classNames from "classnames";
import styles from "./Board.module.scss";

type BoardProps = {
  squares: {
    className: string;
    piece: {
      backgroundImage: string;
    } | null;
  }[];
  sourceSelection: number;
  onClick: Function;
};

const Board = ({ squares, onClick, sourceSelection }: BoardProps) => {
  const board: ReactElement[] = [];

  for (let column = 0; column < 8; column++) {
    const squareRows: ReactElement[] = [];

    for (let row = 0; row < 8; row++) {
      const index = column * 8 + row;

      squareRows.push(
        <div
          className={classNames(styles.Cell, {
            [styles.Selected]: index === sourceSelection,
          })}
          onClick={() => onClick(index)}
          style={{
            backgroundImage: squares[index].piece?.backgroundImage,
          }}
          key={row}
        />
      );
    }

    board.push(
      <div className={styles.Row} key={column}>
        {squareRows}
      </div>
    );
  }

  return <div className={styles.Board}>{board}</div>;
};

export default Board;
