import * as signalR from "@microsoft/signalr";

let connection = null;

export async function connect(token, onReceiveMessage) {

    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5122/chat", {
            accessTokenFactory: () => token
        })
        .withAutomaticReconnect()
        .build();

    connection.on("ReceiveMessage", onReceiveMessage);

    await connection.start();
}

export async function sendMessage(message) {

    await connection.invoke(
        "SendMessage",
        message
    );

}

export async function disconnect() {

    if (connection) {
        await connection.stop();
        connection = null;
    }

}