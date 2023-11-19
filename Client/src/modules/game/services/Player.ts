import type CreatePlayerResonse from "@/modules/game/models/CreatePlayerResponse";
import type CreatePlayerRequest from "@/modules/game/models/CreatePlayerRequest";
import axios from "axios";

const meta = import.meta.env;

const URLs: { [key: string]: string } = {
    CREATE_PLAYER: `${meta.VITE_API_URL}/player/create`,
};

const create = async (request: CreatePlayerRequest) => {
    return axios.post<CreatePlayerResonse>(
        URLs.CREATE_PLAYER,
        request,
        {
            withCredentials: true,
            headers: { "Content-Type": "application/json" },
        }
    );
};

export default {
    create: create,
};
