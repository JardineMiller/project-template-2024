import * as SignalR from "@microsoft/signalr";

const BASE_HUB_URL = "/hubs";
const GAME_HUB_URL = `${BASE_HUB_URL}/game`;

const gameHub = new SignalR.HubConnectionBuilder()
    .withUrl(GAME_HUB_URL)
    .build();

export { gameHub };
