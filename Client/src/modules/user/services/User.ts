import type GetUserDetailsResponse from "@/modules/user/models/GetUserDetailsResponse";
import type UpdateUserRequest from "@/modules/user/models/UpdateUserRequest";
import axios from "axios";

const meta = import.meta.env;

export const URLs: { [key: string]: string } = {
    GET_USER_DETAILS: `${meta.VITE_API_URL}/users/details`,
    UPLOAD_IMAGE: `${meta.VITE_API_URL}/users/uploadavatar`,  
    UPDATE_USER: `${meta.VITE_API_URL}/users`,
    DELETE_AVATAR: `${meta.VITE_API_URL}/users/deleteavatar`,
};

const getUserDetails = async (): Promise<GetUserDetailsResponse> => {
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

const updateUser = async ( 
    payload: UpdateUserRequest
): Promise<GetUserDetailsResponse> => {
    return axios
        .put<GetUserDetailsResponse>(URLs.UPDATE_USER, payload, {
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

const deleteAvatar = async (fileName: string): Promise<GetUserDetailsResponse> => {
        return axios
            .delete<GetUserDetailsResponse>(URLs.DELETE_AVATAR, {
                withCredentials: true,
                headers: { "Content-Type": "application/json" },
                params: { fileName },
            })
            .then(async (response) => response.data)
            .catch(async (err) => err);
    }

export default {
    getUserDetails,
    updateUser,
    deleteAvatar
};
