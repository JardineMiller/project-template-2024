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
    handler: (
        user: string,
        message: string,
        timestamp: Date,
        messageId: string
    ) => void
) => {
    gameHub.on(
        GameEvents.ReceiveMessage,
        (
            user: string,
            message: string,
            timestamp: string,
            messageId: string
        ) => {
            console.debug(
                `[MESSAGE RECEIVED] - User: ${user}, Message: ${message}`
            );
            handler(user, message, new Date(timestamp), messageId);
        }
    );
};

const registerReceiveLikeMessageHandler = (
    handler: (messageId: string) => void
) => {
    gameHub.on(GameEvents.ReceiveLike, (messageId: string) => {
        console.debug(`[MESSAGED LIKED] - Message: ${messageId}`);
        handler(messageId);
    });
};

const registerReceiveUnlikeMessageHandler = (
    handler: (messageId: string) => void
) => {
    gameHub.on(GameEvents.ReceiveUnlike, (messageId: string) => {
        console.debug(`[MESSAGED UNLIKED] - Message: ${messageId}`);
        handler(messageId);
    });
};

const registerPlayerConnectedHandler = (
    handler: (playerName: string, playerId: string) => void
) => {
    gameHub.on(
        GameEvents.PlayerConnected,
        (playerName: string, playerId: string) => {
            console.debug(
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
            console.debug(
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

const likeMessage = (gameCode: string, messageId: string) => {
    gameHub.invoke(GameEvents.LikeMessage, gameCode, messageId);
};

const unlikeMessage = (gameCode: string, messageId: string) => {
    gameHub.invoke(GameEvents.UnlikeMessage, gameCode, messageId);
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
    registerReceiveLikeMessageHandler,
    registerReceiveUnlikeMessageHandler,
    registerPlayerConnectedHandler,
    registerPlayerDisconnectedHandler,
    sendMessage,
    likeMessage,
    unlikeMessage,
    joinGame,
    leaveGame,
    connectionState,
};
