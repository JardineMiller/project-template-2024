import GameEvents from "@/modules/game/signalr/GameEvents";
import * as SignalR from "@microsoft/signalr";

const GAME_HUB_URL = `${import.meta.env.VITE_HUB_URL}/game`;

const gameHub: SignalR.HubConnection =
    new SignalR.HubConnectionBuilder()
        .withUrl(GAME_HUB_URL)
        .withAutomaticReconnect()
        .build();

const connect = async () => {
    return await gameHub.start();
};

const connectionState = () => {
    return gameHub.state;
};

const disconnect = async () => {
    return await gameHub.stop();
};

const registerReceiveMessageHandler = (
    handler: (user: string, message: string) => void
) => {
    gameHub.on(
        GameEvents.ReceiveMessage,
        (user: string, message: string) => {
            console.log(`User: ${user}, Message: ${message}`);
            handler(user, message);
        }
    );
};

const registerPlayerConnectedHandler = (
    handler: (playerName: string, playerId: string) => void
) => {
    gameHub.on(
        GameEvents.PlayerConnected,
        (playerName: string, playerId: string) => {
            console.log(
                `[CONNECTED] - Player name: ${playerName}, Player Id: ${playerId}`
            );
            handler(playerName, playerId);
        }
    );
};

const registerPlayerDisconnectedHandler = (
    handler: (playerName: string, playerId: string) => void
) => {
    gameHub.on(
        GameEvents.PlayerDisconnected,
        (playerName: string, playerId: string) => {
            console.log(
                `[DISCONNECTED] - Player name: ${playerName}, Player Id: ${playerId}`
            );
            handler(playerName, playerId);
        }
    );
};

const sendMessage = (
    gameCode: string,
    user: string,
    message: string
) => {
    gameHub.invoke(GameEvents.SendMessage, gameCode, user, message);
};

const joinGame = (gameCode: string, user: string) => {
    gameHub.invoke(GameEvents.JoinGame, gameCode, user);
};

const leaveGame = (gameCode: string, user: string) => {
    gameHub.invoke(GameEvents.LeaveGame, gameCode, user);
};

export default {
    connect,
    disconnect,
    registerReceiveMessageHandler,
    registerPlayerConnectedHandler,
    registerPlayerDisconnectedHandler,
    sendMessage,
    joinGame,
    leaveGame,
    connectionState,
};
