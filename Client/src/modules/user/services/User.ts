import type GetUserDetailsResponse from "@/modules/user/models/GetUserDetailsResponse";
import axios from "axios";

const meta = import.meta.env;

const URLs: { [key: string]: string } = {
    GET_USER_DETAILS: `${meta.VITE_API_URL}/users/details`,
};

const getUserDetails = async () => {
    return axios
        .get<GetUserDetailsResponse>(URLs.GET_USER_DETAILS, {
            withCredentials: true,
            headers: { "Content-Type": "application/json" },
        })
        .then(async (response) => {
            return response.data;
        })
        .catch(async (err) => {
            return err;
        });
};

export default {
    getUserDetails,
};
