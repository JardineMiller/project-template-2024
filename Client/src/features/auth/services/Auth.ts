import type AuthenticationResponse from "@/features/auth/models/AuthResponse";
import type LoginRequest from "@/features/auth/models/LoginRequest";
import User from "@/features/auth/models/User";
import router from "@/services/router";
import axios from "axios";

const LOGIN_URL = `https://localhost:7097/api/auth/login`;

let authToken: string | null = null;
let user: User | null = null;

const login = async (request: LoginRequest): Promise<void> => {
    return axios
        .post<AuthenticationResponse>(LOGIN_URL, request, {
            headers: { "Content-Type": "application/json" },
        })
        .then(async (response) => {
            const { id, firstName, lastName, email, token } =
                response.data;

            user = new User(id, firstName, lastName, email);
            authToken = token;
            await router.push("/");

            return response;
        })
        .catch((error) => {
            return error;
        });
};

const logout = async (): Promise<void> => {
    user = null;
    authToken = null;
    await router.push("/login");
};

export default {
    user: user,
    token: authToken,
    login: login,
    logout: logout,
};
