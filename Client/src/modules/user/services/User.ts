import type GetUserDetailsResponse from "@/modules/user/models/GetUserDetailsResponse";
import type UploadImageReponse from "@/modules/user/models/ImageUploadResponse";
import axios from "axios";

const meta = import.meta.env;

const URLs: { [key: string]: string } = {
    GET_USER_DETAILS: `${meta.VITE_API_URL}/users/details`,
    UPLOAD_IMAGE: `${meta.VITE_API_URL}/users/upload`,
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

const uploadImage = async (base64data): Promise<UploadImageReponse> => {
    return axios
        .post<UploadImageReponse>(URLs.UPLOAD_IMAGE, {
            withCredentials: true,
            headers: { "Content-Type": "multipart/form-data" },
            file: base64data,
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
    uploadImage,
};
