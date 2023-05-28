import { addJwtInterceptor } from "@/modules/auth/interceptors/httpInterceptor";
import Auth from "@/modules/auth/services/Auth";
import Tooltip from "primevue/tooltip";
import PrimeVue from "primevue/config";
import router from "@/router/router";
import { createApp } from "vue";
import App from "./App.vue";
import "./assets/main.css";

async function startApp() {
    const app = createApp(App);

    app.use(router);
    app.use(PrimeVue);

    app.directive("tooltip", Tooltip);

    // attempt to auto refresh token before startup
    try {
        await Auth.refreshToken();
    } catch {
        // catch error to start app on success or failure
    }

    app.mount("#app");
}

addJwtInterceptor();
startApp();
