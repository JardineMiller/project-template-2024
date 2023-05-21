import type AuthenticationResponse from "@/features/auth/models/AuthResponse";
import type LoginRequest from "@/features/auth/models/LoginRequest";
import User from "@/features/auth/models/User";
import router from "@/services/router";
import axios from "axios";

const LOGIN_URL = `https://localhost:7097/api/auth/login`;

let _authToken: string | null = null;
let _user: User | null = null;

const isAuthenticated = () => {
    return Boolean(_user) && Boolean(_authToken);
};

const login = async (request: LoginRequest): Promise<void> => {
    return axios
        .post<AuthenticationResponse>(LOGIN_URL, request, {
            headers: { "Content-Type": "application/json" },
        })
        .then(async (response) => {
            const { id, firstName, lastName, email, token } =
                response.data;

            _user = new User(id, firstName, lastName, email);
            _authToken = token;
            await router.push("/");

            return response;
        })
        .catch((error) => {
            return error;
        });
};

const logout = async (): Promise<void> => {
    _user = null;
    _authToken = null;
    await router.push("/login");
};

const user = () => {
    return _user;
};

const token = () => {
    return _authToken;
};

export default {
    user: user,
    token: token,
    isAuthenticated: isAuthenticated,
    login: login,
    logout: logout,
};
