import React, { ReactElement } from "react";
import Square from "./Square";

const isEven = (num) => num % 2 === 0;

type BoardProps = {
  squares: {
    style: Record<string, string>;
  }[];
  onClick: Function;
};

export default class Board extends React.Component<BoardProps> {
  render() {
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
            style={
              this.props.squares[index] ? this.props.squares[index].style : null
            }
            shade={squareShade}
            onClick={() => this.props.onClick(index)}
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
  }
}
