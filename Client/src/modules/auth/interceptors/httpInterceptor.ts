import Auth from "@/modules/auth/services/Auth";
import axios from "axios";

export function addJwtInterceptor() {
    axios.interceptors.request.use((request) => {
        // add auth header with jwt if account is logged in and request is to the api url
        const isLoggedIn = Auth.user.value;

        if (!request.url) {
            return request;
        }

        const isApiUrl = request.url.startsWith(
            import.meta.env.VITE_API_URL
        );

        if (isLoggedIn && isApiUrl) {
            request.headers.Authorization = `Bearer ${Auth.token.value}`;
        }

        return request;
    });
}
