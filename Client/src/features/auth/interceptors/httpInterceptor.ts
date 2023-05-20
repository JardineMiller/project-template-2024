import AuthService from "@/features/auth/services/authService";
import axios from "axios";

export function addJwtInterceptor() {
    axios.interceptors.request.use((request) => {
        // add auth header with jwt if account is logged in and request is to the api url
        const isLoggedIn = AuthService.isLoggedIn;

        if (!request.url) {
            return request;
        }

        const isApiUrl = request.url.startsWith(
            import.meta.env.VITE_API_URL
        );

        if (isLoggedIn && isApiUrl) {
            request.headers.Authorization = `Bearer ${AuthService.token}`;
        }

        return request;
    });
}
