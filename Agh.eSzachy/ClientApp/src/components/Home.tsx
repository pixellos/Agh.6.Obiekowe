import React, { useState, useEffect } from "react";
import { HubConnectionBuilder, HubConnectionState } from "@aspnet/signalr";
import { RoomHub, Room } from "../Api";
import authService from "./api-authorization/AuthorizeService";

export const Home = () => {
  authService.getAccessToken();

  const [dataState, setDataState] = useState([] as Room[]);

  const [hub, setHub] = useState<RoomHub>({} as RoomHub);
  const [message, setMessage] = useState<string>("");
  const [room, setRoom] = useState<string>("");

  const [roomList, setRoomList] = useState<string[]>([] as string[]);

  useEffect(() => {
    (async () => {
      if (hub instanceof RoomHub) {
        return;
      }

      const token = await authService.getAccessToken();
      if (typeof token !== "string") {
        throw new Error();
      }

      const c = new HubConnectionBuilder()
        .withUrl("/room", { accessTokenFactory: () => token })
        .build();

      const roomHub = new RoomHub(c);
      roomHub.registerCallbacks({
        refresh: (r) => {
          setDataState(r);
        },
        refreshSingle: (s) => {
          setDataState([s]);
        },
        send: (m) => {},
      });

      switch (c.state) {
        case HubConnectionState.Disconnected:
          await c.start();

          setHub(roomHub);
          setMessage("");
          setRoom("");
          break;

        case HubConnectionState.Connected:
          const roomList = await roomHub.getAllRooms();

          setRoomList(roomList);
          break;
      }
    })();
  }, [hub, setDataState]);

  return (
    <>
      {/* <div>{JSON.stringify(dataState)}</div> */}
      <div>{JSON.stringify(roomList)}</div>
      Message
      <input onChange={(x) => setMessage(x.target.value)} type="text" />
      Room
      <input onChange={(x) => setRoom(x.target.value)} type="text" />
      <div />
      <div />
      <div />
      <button onClick={(x) => hub.send(room, message)}>send</button>
      <button onClick={(x) => hub.create(room)}>create</button>
      <button onClick={(x) => hub.join(room)}>join</button>
      <button onClick={(x) => hub.leave(room)}>left</button>
    </>
  );
};
