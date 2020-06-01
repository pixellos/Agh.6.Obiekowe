import React, { ReactElement } from "react";
import classNames from "classnames";
import Square from "./Square";

const isEven = (num: number) => num % 2 === 0;

type BoardProps = {
  squares: {
    className: string;
    piece: {
      backgroundImage: string;
    } | null;
  }[];
  onClick: Function;
};

const Board = ({ squares, onClick }: BoardProps) => {
  const board: ReactElement[] = [];

  for (let column = 0; column < 8; column++) {
    const squareRows: ReactElement[] = [];

    for (let row = 0; row < 8; row++) {
      const squareShade =
        (isEven(column) && isEven(row)) || (!isEven(column) && !isEven(row))
          ? "light-square"
          : "dark-square";

      const index = column * 8 + row;

      squareRows.push(
        <Square
          style={{
            backgroundImage: squares[index].piece?.backgroundImage,
          }}
          className={classNames(squares[index].className, squareShade)}
          onClick={() => onClick(index)}
          key={row}
        />
      );
    }

    board.push(
      <div className="board-row" key={column}>
        {squareRows}
      </div>
    );
  }

  return <div>{board}</div>;
};

export default Board;
