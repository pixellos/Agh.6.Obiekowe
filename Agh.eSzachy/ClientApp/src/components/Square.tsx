import React, { ReactElement } from "react";
import classNames from "classnames";

const Square = ({ className, onClick, style }: any): ReactElement => (
  <button
    className={classNames("square", className)}
    onClick={onClick}
    style={style}
  />
);

export default Square;
