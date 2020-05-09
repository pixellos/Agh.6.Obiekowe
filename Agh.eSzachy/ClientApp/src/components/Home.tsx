import React, { useState, useEffect } from "react";
import { HubConnectionBuilder, HubConnectionState } from "@aspnet/signalr";
import { RoomHub, Room } from "../Api";
import authService from "./api-authorization/AuthorizeService";

import { withRouter } from "react-router-dom";

export const Home = withRouter(({ history }) => {
  authService.getAccessToken();

  const [hub, setHub] = useState<RoomHub>({} as RoomHub);
  const [roomList, setRoomList] = useState<Room[]>([] as Room[]);
  const [newRoom, setNewRoom] = useState<string>("");

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
          setRoomList(r);
        },
        refreshSingle: (room) => {
          history.push(`/room/${room.Name}`);
          console.log("registerCallbacks.refreshSingle", room);
        },
        send: (m) => {},
      });

      switch (c.state) {
        case HubConnectionState.Disconnected:
          await c.start();

          setHub(roomHub);
          break;
      }
    })();
  }, [hub, history]);

  return (
    <>
      <h2>Rooms</h2>
      <div>
        <input
          type="text"
          onChange={(x) => {
            setNewRoom(x.target.value);
          }}
        />

        <span>
          <button
            onClick={(x) => {
              if (newRoom === "") {
                return;
              }

              hub.create(newRoom);
              setNewRoom("");
            }}
          >
            Create new room
          </button>
        </span>
      </div>
      <>
        {roomList.map((room) => (
          <div key={room.Id}>
            <span>
              <button onClick={(x) => hub.join(room.Name)}>Join</button>
            </span>

            {/* <span>
              <button onClick={(x) => hub.leave(room.Name)}>leave</button>
            </span> */}

            <span>Room: {room.Name}</span>
          </div>
        ))}
      </>
    </>
  );
});
