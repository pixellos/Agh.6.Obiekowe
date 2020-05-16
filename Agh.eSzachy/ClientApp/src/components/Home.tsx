import React, { useState, useEffect, useCallback } from "react";
import { HubConnectionBuilder, HubConnectionState } from "@aspnet/signalr";
import { RoomHub, Room } from "../Api";
import { Game } from "./Game";
import authService from "./api-authorization/AuthorizeService";
import * as css from "./Home.module.css";
import { withRouter } from "react-router-dom";

export const Home = withRouter(({ history }) => {
  authService.getAccessToken();

  const [hub, setHub] = useState<RoomHub | null>(null);
  const [roomList, setRoomList] = useState<string[]>([] as string[]);
  const [userRooms, setUserRooms] = useState<Room[]>([]);
  const [newRoom, setNewRoom] = useState<string>("");
  const [message, setMessage] = useState<string>("");
  const [selected, setSelected] = useState<Room | null>(null);

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
        refresh: async (r: Room[]) => {
          setUserRooms(r);
          setSelected((oldSelected) => {
            const candidate = r.find((x) => x.Id === oldSelected?.Id);
            if (candidate) {
              return candidate;
            } else {
              return oldSelected;
            }
          });
          const rooms = await roomHub.getAllRooms();
          setRoomList(rooms);
        },
        refreshSingle: (room) => {
          // history.push(`/room/${room.Name}`);
          // console.log("registerCallbacks.refreshSingle", room);
        },
        send: (m) => {},
      });

      switch (c.state) {
        case HubConnectionState.Disconnected:
          await c.start();
          const rooms = await roomHub.getAllRooms();
          setRoomList(rooms);
          setHub(roomHub);
          break;
      }
    })();
  }, [hub, history]);

  const renderRooms = () => {
    return (
      <>
        <div className="grid-item">
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
                  if (newRoom === "" || !hub) {
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
        </div>

        <div className="grid-item">
          {roomList.map((roomName, index) => (
            <div key={index}>
              <span>
                <button onClick={(x) => hub && hub.join(roomName)}>Join</button>
              </span>
              <span>Room: {roomName}</span>
            </div>
          ))}
        </div>
        <div className="grid-item">
          <h2>User Rooms</h2>
          {userRooms.map((room, index) => (
            <div key={index}>
              <span>
                <button onClick={(x) => hub && hub.join(room.Name)}>
                  Join
                </button>
                <button
                  onClick={(x) => {
                    setSelected(room);
                  }}
                >
                  Select
                </button>
              </span>
              <span>Room: {room.Name}</span>
            </div>
          ))}
        </div>
      </>
    );
  };

  const renderSelected = () =>
    selected ? (
      <div style={{ display: "inline-flex" }}>
        <div style={{ width: "1200px", display: "inline-block" }}>
          <Game roomName={selected.Name} />
        </div>
        <div style={{ width: "300px", display: "inline-block" }}>
          Messages
          {selected.Messages.map((m, i) => (
            <div key={i}>
              {m.Created} {m.UserName}:{m.Text}
            </div>
          ))}
          <div>
            <input
              type="text"
              onChange={(x) => {
                setMessage(x.target.value);
              }}
            />
            <span>
              <button
                onClick={(x) => {
                  if (message && hub) {
                    hub.send(selected.Name, message);
                  }
                }}
              >
                Send
              </button>
            </span>
          </div>
        </div>
      </div>
    ) : (
      <div></div>
    );

  return (
    <>
      <div className="grid-container">{renderRooms()}</div>
      <div className="grid-container">{renderSelected()}</div>
    </>
  );
});
