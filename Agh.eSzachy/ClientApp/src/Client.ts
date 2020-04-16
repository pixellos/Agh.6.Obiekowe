import {HubConnectionBuilder} from '@aspnet/signalr'

let connection = new HubConnectionBuilder()
    .withUrl("/room")
    .build();
 
connection.on("send", data => {
    console.log(data);
});
 
connection.start()
    .then(() => connection.invoke("send", "Hello"));
