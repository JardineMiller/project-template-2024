import type AuthenticationResponse from "@/modules/auth/models/common/AuthResponse";
import type RegisterRequest from "@/modules/auth/models/register/RegisterRequest";
import type LoginRequest from "@/modules/auth/models/login/LoginRequest";
import User from "@/modules/auth/models/common/User";
import router from "@/router/router";
import { type Ref, ref } from "vue";
import axios from "axios";

const meta = import.meta.env;

const URLs: { [key: string]: string } = {
    LOGIN: `${meta.VITE_API_URL}/auth/login`,
    REGISTER: `${meta.VITE_API_URL}/auth/register`,
    CONFIRM: `${meta.VITE_API_URL}/auth/confirm`,
    REFRESH_TOKEN: `${meta.VITE_API_URL}/auth/refreshToken`,
    REVOKE_TOKEN: `${meta.VITE_API_URL}/auth/revokeToken`,
};

const user: Ref<User | null> = ref(null);
const authToken: Ref<string | null> = ref(null);

let refreshTokenTimeout: number | undefined = undefined;

function startRefreshTokenTimer() {
    // parse json object from base64 encoded jwt token
    if (!authToken.value) {
        return;
    }

    const jwtBase64 = authToken.value.split(".")[1];
    const jwtToken = JSON.parse(atob(jwtBase64));

    // set a timeout to refresh the token a minute before it expires
    const expires = new Date(jwtToken.exp * 1000);
    const timeout = expires.getTime() - Date.now() - 60 * 1000;
    refreshTokenTimeout = window.setTimeout(refreshToken, timeout);
}

function stopRefreshTokenTimer() {
    clearTimeout(refreshTokenTimeout);
}

const confirm = async (token: string, email: string) => {
    return axios
        .get<AuthenticationResponse>(URLs.CONFIRM, {
            withCredentials: true,
            params: { email: email, token: token },
            headers: { "Content-Type": "application/json" },
        })
        .then(async (response) => {
            const { id, firstName, lastName, email, token } =
                response.data;

            user.value = new User(id, firstName, lastName, email);
            authToken.value = token;

            startRefreshTokenTimer();

            await router.push("/");
            return response;
        })
        .catch(async (_) => {
            await router.push("/login");
            return;
        });
};

const login = async (request: LoginRequest) => {
    return axios
        .post<AuthenticationResponse>(URLs.LOGIN, request, {
            withCredentials: true,
            headers: { "Content-Type": "application/json" },
        })
        .then(async (response) => {
            const { id, firstName, lastName, email, token } =
                response.data;

            user.value = new User(id, firstName, lastName, email);
            authToken.value = token;

            startRefreshTokenTimer();

            await router.push("/");
        });
};

const register = async (request: RegisterRequest) => {
    return axios
        .post<AuthenticationResponse>(URLs.REGISTER, request, {
            headers: { "Content-Type": "application/json" },
        })
        .then(async (response) => {})
        .catch((err) => {
            console.table(err);
        });
};

const logout = async (): Promise<void> => {
    return axios
        .get(URLs.REVOKE_TOKEN, {
            withCredentials: true,
            headers: { "Content-Type": "application/json" },
        })
        .then((response) => {
            user.value = null;
            authToken.value = null;

            stopRefreshTokenTimer();
        })
        .finally(async () => {
            await router.push("/login");
        });
};

const refreshToken = async (): Promise<void> => {
    return axios
        .get<AuthenticationResponse>(URLs.REFRESH_TOKEN, {
            withCredentials: true,
            headers: { "Content-Type": "application/json" },
        })
        .then(async (response) => {
            const { id, firstName, lastName, email, token } =
                response.data;

            if (response.status == 200) {
                user.value = new User(id, firstName, lastName, email);
                authToken.value = token;

                startRefreshTokenTimer();

                await router.push("/");
            }
        });
};

export default {
    user: user,
    token: authToken,
    login: login,
    logout: logout,
    register: register,
    confirm: confirm,
    refreshToken: refreshToken,
};
