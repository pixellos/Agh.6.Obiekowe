import React, { ReactElement } from "react";
import classNames from "classnames";

const Square = (props: any): ReactElement => (
  <button
    className={classNames("square", props.shade)}
    onClick={props.onClick}
    style={props.style}
  />
);

export default Square;
