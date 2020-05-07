import React, { useState, useEffect } from "react";
import { HubConnectionBuilder, HubConnectionState } from "@aspnet/signalr";
import { RoomHub, Room } from "../Api";
import authService from "./api-authorization/AuthorizeService";

const initial = {
  hub: {} as RoomHub,
  message: "",
  room: "",
};

export const Home = () => {
  authService.getAccessToken();

  const [state, setState] = useState(initial);
  const [dataState, setDataState] = useState([] as Room[]);

  useEffect(() => {
    (async () => {
      if (state.hub instanceof RoomHub) {
        return;
      }
      const token = await authService.getAccessToken();
      if (typeof token !== "string") {
        throw new Error();
      }
      const c = new HubConnectionBuilder()
        .withUrl("/room", { accessTokenFactory: () => token })
        .build();
      const hub = new RoomHub(c);
      hub.registerCallbacks({
        refresh: (r) => {
          setDataState(r);
        },
        refreshSingle: (s) => {
          setDataState([s]);
        },
        send: (m) => {},
      });
      if (c.state === HubConnectionState.Disconnected) {
        await c.start();
        // debugger;
        setState({ room: "", message: "", hub: hub });
      }
    })();
  }, [state.hub, setDataState]);

  return (
    <div>
      <h1>Hello, world!</h1>
      <div>{JSON.stringify(dataState)}</div>
      Message
      <input
        onChange={(x) => setState({ ...state, message: x.target.value })}
        type="text"
      />
      Room
      <input
        onChange={(x) => setState({ ...state, room: x.target.value })}
        type="text"
      />
      <button onClick={(x) => state.hub.send(state.room, state.message)}>
        send
      </button>
      <button onClick={(x) => state.hub.create(state.room)}>create</button>
      <button onClick={(x) => state.hub.join(state.room)}>join</button>
      <button onClick={(x) => state.hub.leave(state.room)}>left</button>
      <p>Welcome to your new single-page application, built with:</p>
      <ul>
        <li>
          <a href="https://get.asp.net/">ASP.NET Core</a> and{" "}
          <a href="https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx">
            C#
          </a>{" "}
          for cross-platform server-side code
        </li>
        <li>
          <a href="https://facebook.github.io/react/">React</a> for client-side
          code
        </li>
        <li>
          <a href="http://getbootstrap.com/">Bootstrap</a> for layout and
          styling
        </li>
      </ul>
      <p>To help you get started, we have also set up:</p>
      <ul>
        <li>
          <strong>Client-side navigation</strong>. For example, click{" "}
          <em>Counter</em> then <em>Back</em> to return here.
        </li>
        <li>
          <strong>Development server integration</strong>. In development mode,
          the development server from <code>create-react-app</code> runs in the
          background automatically, so your client-side resources are
          dynamically built on demand and the page refreshes when you modify any
          file.
        </li>
        <li>
          <strong>Efficient production builds</strong>. In production mode,
          development-time features are disabled, and your{" "}
          <code>dotnet publish</code> configuration produces minified,
          efficiently bundled JavaScript files.
        </li>
      </ul>
      <p>
        The <code>ClientApp</code> subdirectory is a standard React application
        based on the <code>create-react-app</code> template. If you open a
        command prompt in that directory, you can run <code>npm</code> commands
        such as <code>npm test</code> or <code>npm install</code>.
      </p>
    </div>
  );
};
